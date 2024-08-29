using System.ComponentModel.DataAnnotations;

namespace RentalManager.Domain.Models
{
    public class Product
    {
        [Key] public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public decimal Price { get; set; }
        public int QuantityInStock { get; set; }
        public int ProductTypeId { get; set; }

        public ProductType ProductType { get; set; } = null!;
    }
}