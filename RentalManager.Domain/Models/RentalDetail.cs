using System.ComponentModel.DataAnnotations;

namespace RentalManager.Domain.Models
{
    public class RentalDetail
    {
        [Key] public int RentalDetailId { get; set; }
        public int RentalId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        public Rental Rental { get; set; } = null!;
        public Product Product { get; set; } = null!;
    }
}