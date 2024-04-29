using Microsoft.AspNetCore.Mvc.Rendering;
using MvcApp.EFCore.Models;

namespace MvcApp.EFCore.ViewModel
{
    public class OrderViewModel
    {
        public IEnumerable<ShoppingCart>? carts { get; set; }
        public Order order { get; set; }
        public string? CartUserId { get; set; }
        public IEnumerable<SelectListItem>? paymentOption { get; set; }
        public decimal PaymentAmount { get; set; }
    }
}
