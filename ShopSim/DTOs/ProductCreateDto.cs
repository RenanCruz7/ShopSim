using System.ComponentModel.DataAnnotations;

namespace ShopSim.DTOs;

public class ProductCreateDto
{
    [Required]
    public string Name { get; set; }
    public string Description { get; set; }
    [Required]
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
}