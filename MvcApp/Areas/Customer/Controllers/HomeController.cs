using Microsoft.AspNetCore.Mvc;
using MvcApp.EFCore.Models;
using System.Diagnostics;
using MvcApp.Services;
using Microsoft.EntityFrameworkCore.Internal;
using System.Text;
namespace MvcApp.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork = null)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public async Task<IActionResult> Index()
        {
            
            return View(await _unitOfWork.BookRepository.GetAllAsync("Category"));
        }
        
        public async Task<IActionResult> Details(int id)
        {
            return View(await _unitOfWork.BookRepository.GetAsync(x=>x.Id==id,includes:"Category"));
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
