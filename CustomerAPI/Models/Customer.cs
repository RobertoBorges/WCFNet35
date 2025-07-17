using System.ComponentModel.DataAnnotations;

namespace CustomerAPI.Models
{
    /// <summary>
    /// Customer entity model for the REST API
    /// </summary>
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        public DateTime DateOfBirth { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Country { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        public Customer()
        {
        }

        public Customer(int id, string firstName, string lastName, string email, DateTime dateOfBirth, string city, string country)
        {
            Id = id;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            DateOfBirth = dateOfBirth;
            City = city;
            Country = country;
            IsActive = true;
        }
    }
}