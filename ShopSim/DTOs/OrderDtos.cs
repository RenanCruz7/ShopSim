using System.ComponentModel.DataAnnotations;

namespace ShopSim.DTOs;

public class OrderCreateDto
{
    [Required]
    public List<OrderItemCreateDto> Items { get; set; } = new List<OrderItemCreateDto>();
    
    [Required]
    [StringLength(500)]
    public string ShippingAddress { get; set; }
}

public class OrderItemCreateDto
{
    [Required]
    public int ProductId { get; set; }
    
    [Required]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be at least 1")]
    public int Quantity { get; set; }
}

public class OrderReadDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal TotalAmount { get; set; }
    public string Status { get; set; }
    public string ShippingAddress { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public List<OrderItemReadDto> Items { get; set; } = new List<OrderItemReadDto>();
    public UserReadDto User { get; set; }
}

public class OrderItemReadDto
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string ProductName { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalPrice { get; set; }
}

public class OrderUpdateStatusDto
{
    [Required]
    [StringLength(50)]
    public string Status { get; set; }
}
