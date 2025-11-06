using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ShopSim.Data;
using ShopSim.DTOs;
using ShopSim.Models;

namespace ShopSim.Services;

public class OrderService : IOrderService
{
    private readonly ShopSimContext _context;
    private readonly IMapper _mapper;

    public OrderService(ShopSimContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<OrderReadDto>> GetAllOrdersAsync(PaginationFilter filter)
    {
        var query = _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .AsQueryable();

        // Apply search filter
        if (!string.IsNullOrWhiteSpace(filter.SearchTerm))
        {
            query = query.Where(o => o.Status.Contains(filter.SearchTerm) ||
                                   o.User.Email.Contains(filter.SearchTerm) ||
                                   o.User.FirstName.Contains(filter.SearchTerm) ||
                                   o.User.LastName.Contains(filter.SearchTerm));
        }

        // Apply sorting
        query = filter.SortDirection.ToLower() == "desc"
            ? query.OrderByDescending(GetSortExpression(filter.SortBy))
            : query.OrderBy(GetSortExpression(filter.SortBy));

        var totalCount = await query.CountAsync();

        var orders = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var orderDtos = orders.Select(MapToOrderReadDto).ToList();

        return new PagedResult<OrderReadDto>
        {
            Data = orderDtos,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<PagedResult<OrderReadDto>> GetUserOrdersAsync(int userId, PaginationFilter filter)
    {
        var query = _context.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .AsQueryable();

        // Apply sorting
        query = filter.SortDirection.ToLower() == "desc"
            ? query.OrderByDescending(GetSortExpression(filter.SortBy))
            : query.OrderBy(GetSortExpression(filter.SortBy));

        var totalCount = await query.CountAsync();

        var orders = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync();

        var orderDtos = orders.Select(MapToOrderReadDto).ToList();

        return new PagedResult<OrderReadDto>
        {
            Data = orderDtos,
            TotalCount = totalCount,
            Page = filter.Page,
            PageSize = filter.PageSize
        };
    }

    public async Task<OrderReadDto> GetOrderByIdAsync(int id)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            throw new KeyNotFoundException("Order not found");
        }

        return MapToOrderReadDto(order);
    }

    public async Task<OrderReadDto> CreateOrderAsync(int userId, OrderCreateDto orderCreateDto)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // Validate user exists
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            // Validate products and calculate total
            decimal totalAmount = 0;
            var orderItems = new List<OrderItem>();

            foreach (var itemDto in orderCreateDto.Items)
            {
                var product = await _context.Products.FindAsync(itemDto.ProductId);
                if (product == null)
                {
                    throw new KeyNotFoundException($"Product with ID {itemDto.ProductId} not found");
                }

                if (product.StockQuantity < itemDto.Quantity)
                {
                    throw new InvalidOperationException($"Insufficient stock for product {product.Name}");
                }

                var orderItem = new OrderItem
                {
                    ProductId = itemDto.ProductId,
                    Quantity = itemDto.Quantity,
                    UnitPrice = product.Price
                };

                orderItems.Add(orderItem);
                totalAmount += orderItem.TotalPrice;

                // Update stock
                product.StockQuantity -= itemDto.Quantity;
                product.UpdatedAt = DateTime.UtcNow;
            }

            // Create order
            var order = new Order
            {
                UserId = userId,
                TotalAmount = totalAmount,
                ShippingAddress = orderCreateDto.ShippingAddress,
                OrderItems = orderItems
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return await GetOrderByIdAsync(order.Id);
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    public async Task<bool> UpdateOrderStatusAsync(int id, OrderUpdateStatusDto statusDto)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return false;
        }

        var validStatuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
        if (!validStatuses.Contains(statusDto.Status))
        {
            throw new ArgumentException("Invalid order status");
        }

        order.Status = statusDto.Status;
        order.UpdatedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelOrderAsync(int id, int userId)
    {
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Product)
            .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

        if (order == null)
        {
            return false;
        }

        if (order.Status != "Pending")
        {
            throw new InvalidOperationException("Only pending orders can be cancelled");
        }

        using var transaction = await _context.Database.BeginTransactionAsync();
        
        try
        {
            // Restore stock
            foreach (var item in order.OrderItems)
            {
                item.Product.StockQuantity += item.Quantity;
                item.Product.UpdatedAt = DateTime.UtcNow;
            }

            order.Status = "Cancelled";
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();
            
            return true;
        }
        catch
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    private OrderReadDto MapToOrderReadDto(Order order)
    {
        return new OrderReadDto
        {
            Id = order.Id,
            UserId = order.UserId,
            TotalAmount = order.TotalAmount,
            Status = order.Status,
            ShippingAddress = order.ShippingAddress,
            CreatedAt = order.CreatedAt,
            UpdatedAt = order.UpdatedAt,
            User = _mapper.Map<UserReadDto>(order.User),
            Items = order.OrderItems.Select(oi => new OrderItemReadDto
            {
                Id = oi.Id,
                ProductId = oi.ProductId,
                ProductName = oi.Product.Name,
                Quantity = oi.Quantity,
                UnitPrice = oi.UnitPrice,
                TotalPrice = oi.TotalPrice
            }).ToList()
        };
    }

    private static System.Linq.Expressions.Expression<Func<Order, object>> GetSortExpression(string sortBy)
    {
        return sortBy.ToLower() switch
        {
            "totalamount" => o => o.TotalAmount,
            "status" => o => o.Status,
            "createdat" => o => o.CreatedAt,
            "updatedat" => o => o.UpdatedAt ?? DateTime.MinValue,
            _ => o => o.Id
        };
    }
}
