using System.ComponentModel.DataAnnotations;

namespace MvcApp.EFCore.Models
{
    public class Category
    {
        public required int Id { get; set; }
        [Required(ErrorMessage ="The name cannot be null or white space")]
        public required string? Name { get; set; }
        [Required(ErrorMessage = "the order must be in the rnage from 1 to 100")]
        [Range(1,100)]
        public required int DisplayOrder { get; set; }
    }
}
