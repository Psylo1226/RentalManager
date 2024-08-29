using System.ComponentModel.DataAnnotations;

namespace RentalManager.Domain.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        public string FirstName { get; set; } = null!;
        public string Surname { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string PhoneNumber { get; set; } = null!;

        public List<Rental> Rentals { get; set; } = new();
    }
}
