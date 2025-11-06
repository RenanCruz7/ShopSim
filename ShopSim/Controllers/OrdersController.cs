using Microsoft.AspNetCore.Mvc;
using ShopSim.DTOs;
using ShopSim.Services;

namespace ShopSim.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllOrders([FromQuery] PaginationFilter filter)
    {
        try
        {
            // This would typically require admin authorization
            var result = await _orderService.GetAllOrdersAsync(filter);
            return Ok(ApiResponse<PagedResult<OrderReadDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving orders"));
        }
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetUserOrders(int userId, [FromQuery] PaginationFilter filter)
    {
        try
        {
            // This would typically require user authentication and authorization
            var result = await _orderService.GetUserOrdersAsync(userId, filter);
            return Ok(ApiResponse<PagedResult<OrderReadDto>>.SuccessResponse(result));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving user orders"));
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOrderById(int id)
    {
        try
        {
            var order = await _orderService.GetOrderByIdAsync(id);
            return Ok(ApiResponse<OrderReadDto>.SuccessResponse(order));
        }
        catch (KeyNotFoundException)
        {
            return NotFound(ApiResponse<object>.ErrorResponse("Order not found"));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while retrieving the order"));
        }
    }

    [HttpPost("user/{userId}")]
    public async Task<IActionResult> CreateOrder(int userId, [FromBody] OrderCreateDto orderCreateDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            var createdOrder = await _orderService.CreateOrderAsync(userId, orderCreateDto);
            
            return CreatedAtAction(nameof(GetOrderById), 
                new { id = createdOrder.Id }, 
                ApiResponse<OrderReadDto>.SuccessResponse(createdOrder, "Order created successfully"));
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while creating the order"));
        }
    }

    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderUpdateStatusDto statusDto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Invalid input",
                    ModelState.Values.SelectMany(v => v.Errors.Select(e => e.ErrorMessage)).ToList()));
            }

            var updated = await _orderService.UpdateOrderStatusAsync(id, statusDto);

            if (!updated)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Order not found"));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "Order status updated successfully"));
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while updating the order status"));
        }
    }

    [HttpDelete("{id}/user/{userId}")]
    public async Task<IActionResult> CancelOrder(int id, int userId)
    {
        try
        {
            var cancelled = await _orderService.CancelOrderAsync(id, userId);

            if (!cancelled)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Order not found or you don't have permission to cancel it"));
            }

            return Ok(ApiResponse<object>.SuccessResponse(null, "Order cancelled successfully"));
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ApiResponse<object>.ErrorResponse(ex.Message));
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("An error occurred while cancelling the order"));
        }
    }
}
