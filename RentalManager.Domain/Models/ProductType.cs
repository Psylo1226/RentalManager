using System.ComponentModel.DataAnnotations;

namespace RentalManager.Domain.Models;

public class ProductType
{
    [Key]
    public int ProductTypeId { get; set; }
    public string TypeName { get; set; } = null!;
    public List<Product> Products { get; set; } = new();
}