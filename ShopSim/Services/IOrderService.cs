using ShopSim.DTOs;

namespace ShopSim.Services;

public interface IOrderService
{
    Task<PagedResult<OrderReadDto>> GetAllOrdersAsync(PaginationFilter filter);
    Task<PagedResult<OrderReadDto>> GetUserOrdersAsync(int userId, PaginationFilter filter);
    Task<OrderReadDto> GetOrderByIdAsync(int id);
    Task<OrderReadDto> CreateOrderAsync(int userId, OrderCreateDto orderCreateDto);
    Task<bool> UpdateOrderStatusAsync(int id, OrderUpdateStatusDto statusDto);
    Task<bool> CancelOrderAsync(int id, int userId);
}
