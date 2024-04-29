using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MvcApp.EFCore.Models
{
    public class Order
    {
        [Key]
        public int Id { get; set; }
        
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public ApplicationUser User { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public DateTime RequireDate { get; set; }

        public decimal Total {  get; set; }
        public string? Status { get; set; }
        public string? PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public int? TransactionId { get; set; }
        [MaxLength(255)]

        public string? City { get; set; }
        [MaxLength(255)]

        public string? State { get; set; }
        [MaxLength(255)]

        public string? PhoneNumber { get; set; }
        [MaxLength(255)]

        public string? PostalCode { get; set;}
        [MaxLength(255)]
        public string? Name {  get; set; }

    }
}
