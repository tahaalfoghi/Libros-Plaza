using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace MvcApp.EFCore.Models
{
    public class ApplicationUser:IdentityUser
    {
        [Required]
        public string? Name { get; set; }
        [MaxLength(255)]

        public string? Address { get; set; }
        [MaxLength(255)]

        public string? PostalCode { get; set; }
        [MaxLength(255)]
        public string? City { get; set; }

    }
}
