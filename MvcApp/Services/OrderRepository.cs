using Microsoft.EntityFrameworkCore;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using System.Linq.Expressions;

namespace MvcApp.Services
{
    public class OrderRepository : IRepository<Order>
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(Order entity)
        {
            await _context.Set<Order>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.Set<Order>().FindAsync(id);
            _context.Set<Order>().Remove(record);
        }

        public async Task<IEnumerable<Order>> GetAllAsync(string? includes = null)
        {
            IQueryable<Order> query = _context.Set<Order>();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach(var i in includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }
            return await query.ToListAsync();
        }

        public async Task<Order> GetAsync(Expression<Func<Order, bool>> predicate, string? includes = null)
        {
            IQueryable<Order> query = _context.Set<Order>().Where(predicate);
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

        public async Task<Order> GetByIdAsync(int id, string? includes = null)
        {
            IQueryable<Order> query = _context.Set<Order>();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var item in includes
                    .Split(new char[] {','}, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(item);
                }
            }
            return await query.FirstOrDefaultAsync();
        }

        public async Task UpdateAsync(int id, Order entity)
        {
            var record = await _context.Set<Order>().FindAsync(id);
            _context.Entry(record).CurrentValues.SetValues(entity);
        }
    }
}
