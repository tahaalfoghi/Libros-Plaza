using Microsoft.EntityFrameworkCore;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using System.Linq.Expressions;

namespace MvcApp.Services
{
    public class OrderDetailsRepository:IRepository<OrderDetails>
    {
        private readonly AppDbContext _context;

        public OrderDetailsRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(OrderDetails entity)
        {
            await _context.Set<OrderDetails>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.Set<OrderDetails>().FindAsync(id);
            _context.Set<OrderDetails>().Remove(record);
        }

        public async Task<IEnumerable<OrderDetails>> GetAllAsync(string? includes = null)
        {
            IQueryable<OrderDetails> query = _context.Set<OrderDetails>();
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

        public async Task<OrderDetails> GetAsync(Expression<Func<OrderDetails, bool>> predicate, string? includes = null)
        {
            IQueryable<OrderDetails> query = _context.Set<OrderDetails>().Where(predicate);
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

        public async Task<OrderDetails> GetByIdAsync(int id, string? includes = null)
        {
            IQueryable<OrderDetails> query = _context.Set<OrderDetails>();
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

        public async Task UpdateAsync(int id, OrderDetails entity)
        {
            var record = await _context.Set<OrderDetails>().FindAsync(id);
            _context.Entry(record).CurrentValues.SetValues(entity);
        }
    }
}
