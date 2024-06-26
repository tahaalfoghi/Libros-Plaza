﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using System.Linq.Expressions;
using System.Security.Claims;

namespace MvcApp.Services
{
    public class ShoppingCartRepository:IRepository<ShoppingCart>
    {
        private readonly AppDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;
        public ShoppingCartRepository(AppDbContext context, UserManager<IdentityUser> userManager ,
            IHttpContextAccessor contextAccessor = null)
        {
            _context = context;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }
        public async Task CreateAsync(ShoppingCart entity)
        {
            await _context.Set<ShoppingCart>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.Set<ShoppingCart>().FindAsync(id);
            _context.Set<ShoppingCart>().Remove(record);
        }

        public async Task<IEnumerable<ShoppingCart>> GetAllAsync(string? includes = null)
        {
            IQueryable<ShoppingCart> query = _context.Set<ShoppingCart>();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var i in includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<ShoppingCart> GetAsync(Expression<Func<ShoppingCart, bool>> predicate, string? includes = null)
        {
            IQueryable<ShoppingCart> query = _context.Set<ShoppingCart>().Where(predicate);
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var i in includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task<ShoppingCart> GetByIdAsync(int id, string? includes = null)
        {
            IQueryable<ShoppingCart> query = _context.Set<ShoppingCart>();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var item in includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, ShoppingCart entity)
        {
            var record = await _context.Set<ShoppingCart>().FindAsync(id);
            _context.Entry(record).CurrentValues.SetValues(entity);
        }
        public async Task<string?> GetUserId()
        {

            var principle = _contextAccessor.HttpContext.User;
            var userId = _userManager.GetUserId(principle);
            return userId;
        }
        public async Task<IEnumerable<ShoppingCart>> GetUserCart(Expression<Func<ShoppingCart, bool>> predicate)
        {
            return await _context.Set<ShoppingCart>().Where(predicate).ToListAsync();
        }
        public async Task<IEnumerable<ShoppingCart>> GetRangeAsync(
            Expression<Func<ShoppingCart, bool>> predicate, string? includes = null)
        {
            IQueryable<ShoppingCart> query = _context.Set<ShoppingCart>().Where(predicate);
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var item in includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.ToListAsync();

        }
    }
}
