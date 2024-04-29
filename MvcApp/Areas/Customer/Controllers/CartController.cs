using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using MvcApp.EFCore.ViewModel;
using MvcApp.Services;
using MvcApp.Utility;
using System.Linq;

namespace MvcApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController:Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly IUnitOfWork _uow;
        public CartController(AppDbContext context,
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IUnitOfWork uow)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
            _uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            var carts = await _uow.ShoppingCartRepository.GetAllAsync(includes:"Book");
            return View(carts);
        }
        [Authorize]
        public async Task<IActionResult> AddToCart(int bookId,CancellationToken token)
        {
            var record = await _context.Set<Book>().FirstOrDefaultAsync(x=>x.Id==bookId);
            if (_signInManager.IsSignedIn(User))
            {
                var user =  _userManager.GetUserId(User);
                if(user is not null)
                {
                    var getCartIfExistInUser = await _context.ShoppingCarts.Where(x => x.UserId.Equals(user)).ToListAsync();
                    if (getCartIfExistInUser.Count()>0)
                    {
                        // User has a cart & updatig the item quantity
                        var getTheQuantity = getCartIfExistInUser.FirstOrDefault(x => x.BookId == bookId);
                        if(getTheQuantity is not null)
                        {
                            getTheQuantity.Quantity += 1;
                            await _uow.ShoppingCartRepository.UpdateAsync(getTheQuantity.Id,getTheQuantity);
                        }
                        else
                        {
                            // User has a cart & adding a new item
                            ShoppingCart cart = new ShoppingCart
                            {
                                BookId = bookId,
                                UserId = user,
                                Quantity =1
                            };
                            await _uow.ShoppingCartRepository.CreateAsync(cart);
                        }
                    }
                    else
                    {
                        // user has no cart & adding a new item
                        ShoppingCart cart = new ShoppingCart
                        {
                            BookId =bookId,
                            UserId=user,
                            Quantity =1 
                        };
                        await _uow.ShoppingCartRepository.CreateAsync(cart);
                    }
                    await _uow.CommitAsync(token);
                }
            }
            return RedirectToAction("Index","Home");
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            
            return Ok(await _uow.ShoppingCartRepository.GetAllAsync(includes:"Book"));
        }
       


        #region APICALL

        public async Task<IActionResult> Delete(int cartId,CancellationToken token)
        {
            var cart = await _uow.ShoppingCartRepository.GetAsync(x => x.Id == cartId, includes: "Book");
            var count = _context.ShoppingCarts.Where(x => x.UserId == cart.UserId).ToList().Count();
            await _uow.ShoppingCartRepository.DeleteAsync(cart.Id);
            await _uow.CommitAsync(token);
            HttpContext.Session.SetInt32(CartCount.sessionCart, count - 1);

            return RedirectToAction("Index");
        }
        #endregion
    }
}
