using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using MvcApp.EFCore.ViewModel;
using MvcApp.Services;
using MvcApp.Utility;

namespace MvcApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class OrderController:Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;
        private readonly IUnitOfWork _uow;

        public OrderController(UserManager<IdentityUser> userManager, 
            SignInManager<IdentityUser> signInManager, 
            AppDbContext context, IUnitOfWork uow)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
            _uow = uow;
        }
        public async Task<IActionResult> Checkout()
        {
            var claim = _signInManager.IsSignedIn(User);
            if (claim)
            {
                var userId = _userManager.GetUserId(User);
                var currentUser = await _context.Set<ApplicationUser>().FindAsync(userId);
                OrderViewModel orderSummery = new OrderViewModel
                {
                    carts = await _uow.ShoppingCartRepository.GetRangeAsync(x => x.UserId.Equals(userId)),
                    order = new Order(),
                    CartUserId = userId
                };
                if (currentUser is not null)
                {
                    orderSummery.order.City = currentUser.City;
                    orderSummery.order.PostalCode = currentUser.PostalCode;
                }
                var count = _context.ShoppingCarts
                    .Where(x => x.UserId.Equals(_userManager.GetUserId(User))).ToList().Count();
                HttpContext.Session.SetInt32(CartCount.sessionCart, count);
                return View(orderSummery);
            }
            return NotFound();
        }
    }
}
