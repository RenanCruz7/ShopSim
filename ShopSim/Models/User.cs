using System.ComponentModel.DataAnnotations;

namespace ShopSim.Models;

public class User
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
    
    [Required]
    [StringLength(100)]
    public string LastName { get; set; }
    
    [Required]
    [EmailAddress]
    [StringLength(255)]
    public string Email { get; set; }
    
    [Required]
    public string PasswordHash { get; set; }
    
    [Required]
    [StringLength(50)]
    public string Role { get; set; } = "Customer";
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
    public bool IsActive { get; set; } = true;
    
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
