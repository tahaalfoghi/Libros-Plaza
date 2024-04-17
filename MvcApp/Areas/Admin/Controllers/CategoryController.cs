using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using MvcApp.Services;
using MvcApp.Utility;

namespace MvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class CategoryController : Controller
    {
        private readonly IRepository<Category> _repo;
        private readonly IUnitOfWork _uow;

        public CategoryController(IRepository<Category> repo, IUnitOfWork uow)
        {
            _repo = repo;
            _uow = uow;
        }

        public async Task<IActionResult> Index()
        {
            var records = await _uow.CategoryRepository.GetAllAsync();
            return View(records);
        }
        public async Task<IActionResult> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category Category, CancellationToken token)
        {

            if (ModelState.IsValid)
            {
                await _uow.CategoryRepository.CreateAsync(Category);
                await _uow.CommitAsync(token);
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index");
            }
            return View();
        }

        public async Task<IActionResult> Update(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var record = await _uow.CategoryRepository.GetByIdAsync(id);
            if (record is null)
                return NotFound();

            return View(record);
        }
        [HttpPost]
        public async Task<IActionResult> Update(Category Category, CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                await _uow.CategoryRepository.UpdateAsync(Category.Id, Category);
                await _uow.CommitAsync(token);
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            var record = await _uow.CategoryRepository.GetByIdAsync(id);
            if (record is null)
                return NotFound();

            return View(record);
        }
        

        #region APICALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _uow.CategoryRepository.GetAllAsync();

            return Json(new { data = records });
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id,CancellationToken token)
        {
            if(id<=0)
            {
                return Json(new { succes = false, message = "category id is invalid" });
            }
            var record = await _uow.CategoryRepository.GetByIdAsync(id);
            if(record is null)
            {
                return Json(new { succes = false, message = "category not found invalid" });
            }
            await _uow.CategoryRepository.DeleteAsync(id);
            await _uow.CommitAsync(token);
            return Json(new { succes = false, message = "category deleted successfully" });
        }
        #endregion
    }
}
