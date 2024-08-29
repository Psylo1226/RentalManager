using System.ComponentModel.DataAnnotations;

namespace RentalManager.Domain.Models
{
    public class Rental
    {
        [Key] public int RentalId { get; set; }
        public int CustomerId { get; set; }
        public DateTime RentalDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public decimal FullPrice { get; set; }

        public Customer Customer { get; set; } = null!;
        public List<RentalDetail> RentalDetails { get; set; } = new();
    }
}