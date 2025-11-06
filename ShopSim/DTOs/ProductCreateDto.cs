﻿using System.ComponentModel.DataAnnotations;

namespace ShopSim.DTOs;

public class ProductCreateDto
{
    [Required]
    [StringLength(200)]
    public string Name { get; set; }
    
    [StringLength(1000)]
    public string Description { get; set; }
    
    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }
    
    [Required]
    [Range(0, int.MaxValue, ErrorMessage = "Stock quantity cannot be negative")]
    public int StockQuantity { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    
    [StringLength(500)]
    public string ImageUrl { get; set; }
    
    [StringLength(100)]
    public string SKU { get; set; }
}