using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Routing.Constraints;
using MvcApp.EFCore.Models;
using MvcApp.Services;
using MvcApp.Utility;
using System.Collections.Generic;
using System.Linq;
namespace MvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    
    public class BookController:Controller
    {
        private readonly IUnitOfWork _uow;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BookController(IUnitOfWork uow, IWebHostEnvironment webHostEnvironment)
        {
            _uow = uow;
            _webHostEnvironment = webHostEnvironment;
        }
        [Authorize(Roles = Roles.Role_Admin)]
        public async Task<IActionResult> Index()
        {
            return View(await _uow.BookRepository.GetAllAsync(includes:"Category"));
        }
        [Authorize(Roles = Roles.Role_Admin)]
        public async Task<IActionResult> Upsert(int? id)
        {
            IEnumerable<SelectListItem> list = (await _uow.CategoryRepository.GetAllAsync())
            .Select(x => new SelectListItem
            {
                Text = x.Name,
                Value = x.Id.ToString()
            });
            ViewBag.CategoryList = list;
            BookViewModel BookVM = new()
            {
                CategoryList = list,
                book= new Book()
            };
            if(id == null || id == 0)
            {
                // create
                return View(BookVM);
            }
            else
            {
                // update
                BookVM.book = await _uow.BookRepository.GetAsync(x=>x.Id == id);
                return View(BookVM);
            }
        }
        [HttpPost]
        [Authorize(Roles = Roles.Role_Admin)]
        public async Task<IActionResult> Upsert(BookViewModel obj,IFormFile? file,CancellationToken token)
        {
            if (ModelState.IsValid)
            {
                string? wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file is not null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string bookPath = Path.Combine(wwwRootPath, @"Images\Books");

                    if (!string.IsNullOrEmpty(obj.book.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(wwwRootPath, obj.book.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    using (var fileStream = new FileStream(Path.Combine(bookPath, fileName), FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    obj.book.ImageUrl =@"\Images\Books\"+fileName;
                }
                if (obj.book.Id == 0)
                {
                    await _uow.BookRepository.CreateAsync(obj.book);
                    await _uow.CommitAsync(token);
                    TempData["success"] = "Book created successfully";
                }
                else
                {
                    await _uow.BookRepository.UpdateAsync(obj.book.Id,obj.book);
                    await _uow.CommitAsync(token);
                    TempData["success"] = "Book updated successfully";
                }
                return RedirectToAction("Index");
            }
            else
            {
                obj.CategoryList = (await _uow.BookRepository.GetAllAsync())
                .Select(x => new SelectListItem
                {
                    Text =  x.Title,
                    Value = x.Id.ToString()
                });
                return View(obj);
            }

           
        }
        
        public async Task<IActionResult> ProgrammingBooks()
        {
            var records = await _uow.BookRepository.GetAllAsync(includes:"Category");
            records = records.Where(x => x.Category.Name.Equals("Programming"));
            return View(records);
        }
        public async Task<IActionResult> MangaBooks()
        {
            var records = await _uow.BookRepository.GetAllAsync(includes: "Category");
            records = records.Where(x => x.Category.Name.Equals("Manga"));
            return View(records);
        }

        #region APICALL
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var records = await _uow.BookRepository.GetAllAsync(includes: "Category");

            return Json(new {data = records});
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id, CancellationToken token)
        {
            if (!ModelState.IsValid || id <= 0)
                return Json(new { success = false, message = "error while deleting" });

            var record = await _uow.BookRepository.GetByIdAsync(id);
            if (record is null)
                return Json(new { success = false, message = "error while deleting" });

            var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, 
                record.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }
            await _uow.BookRepository.DeleteAsync(id);
            await _uow.CommitAsync(token);
            TempData["success"] = "book delete successfully";
            return Json(new { success = true, message = "book deleted successfull" });
        }
        #endregion
    }
}
