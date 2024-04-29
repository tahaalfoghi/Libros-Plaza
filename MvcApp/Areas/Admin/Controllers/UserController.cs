using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using MvcApp.Utility;

namespace MvcApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Roles.Role_Admin)]
    public class UserController:Controller
    {
        private readonly AppDbContext _context;
        
        public UserController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            IQueryable<MvcApp.EFCore.Models.UserView> records = from u in _context.Users
                                                                join ur in _context.UserRoles on u.Id equals ur.UserId
                                                                join r in _context.Roles on ur.RoleId equals r.Id
                                                                select new MvcApp.EFCore.Models.UserView
                                                                {
                                                                    UserName = u.UserName,
                                                                    Email = u.Email,
                                                                    Role = r.Name
                                                                };
            return View(records);

        }
    }
    public class UserView
    {
        public string? UserName { get; set;}
        public string? Email { get; set;}
        public string? Role { get; set;}
    }
}
