﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopSim.Models;

public class Product
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(200)]
    public string Name { get; set; }
    
    [StringLength(1000)]
    public string Description { get; set; }
    
    [Required]
    [Column(TypeName = "decimal(10,2)")]
    public decimal Price { get; set; }
    
    [Required]
    public int StockQuantity { get; set; }
    
    [Required]
    public int CategoryId { get; set; }
    
    [StringLength(500)]
    public string ImageUrl { get; set; }
    
    [StringLength(100)]
    public string SKU { get; set; }
    
    public bool IsActive { get; set; } = true;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    
    [ForeignKey("CategoryId")]
    public Category Category { get; set; }
    
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}