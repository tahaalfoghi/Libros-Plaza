using Microsoft.AspNetCore.Mvc;
using MvcApp.EFCore.Models;
using System.Diagnostics;
using MvcApp.Services;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using MvcApp.Utility;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using MvcApp.EFCore.Data;
namespace MvcApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly AppDbContext _context;
        public HomeController(ILogger<HomeController> logger,
            IUnitOfWork unitOfWork, SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, AppDbContext context)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var claim = _signInManager.IsSignedIn(User);
            if (claim)
            {
                var userId = _userManager.GetUserId(User);
                var count = _context.Set<ShoppingCart>().Where(x => x.UserId == userId).Count();
                HttpContext.Session.SetInt32(CartCount.sessionCart,count);

            }
            
            return View(await _unitOfWork.BookRepository.GetAllAsync("Category"));
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var record = await _unitOfWork.BookRepository.GetAsync(x=>x.Id==id, includes:"Category");
            
            return View(record);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
