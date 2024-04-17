using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApp.EFCore.Models
{
    public class ShoppingCart
    {
        public ShoppingCart()
        {
            Quantity = 1;
        }
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }

        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }
        [Range(1,100,ErrorMessage ="Please enter a value from 1 between 100")]
        public int Quantity { get; set; }
        [NotMapped]
        public decimal Price { get; set; }  
    }
}
