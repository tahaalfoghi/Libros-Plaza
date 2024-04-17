using Microsoft.EntityFrameworkCore;
using MvcApp.EFCore.Data;
using MvcApp.EFCore.Models;
using System;
using System.Linq.Expressions;

namespace MvcApp.Services
{
    public class BookRepository:IRepository<Book>
    {
        private readonly AppDbContext _context;

        public BookRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(Book entity)
        {
            await _context.Set<Book>().AddAsync(entity);
        }

        public async Task DeleteAsync(int id)
        {
            var record = await _context.Set<Book>().FindAsync(id);
            _context.Set<Book>().Remove(record);
        }

        public async Task<IEnumerable<Book>> GetAllAsync(string? includes = null)
        {
            IQueryable<Book> query = _context.Set<Book>();
            if (!string.IsNullOrEmpty(includes))
            {
                foreach(var i in includes
                    .Split(new char[] {','},StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);   
                }
            }
            return await query.ToListAsync();
        }

        public async Task<Book> GetAsync(Expression<Func<Book, bool>> predicate, string? includes = null)
        {
            IQueryable<Book> query = _context.Set<Book>().Where(predicate);
            if (!string.IsNullOrEmpty(includes))
            {
                foreach (var i in includes
                    .Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    query = query.Include(i);
                }
            }
            return query.FirstOrDefault();
            
        }

        public async Task<Book> GetByIdAsync(int id, string? includes = null)
        {
            IQueryable<Book> query = _context.Set<Book>().Where(x=>x.Id==id);
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

        public async Task UpdateAsync(int id, Book entity)
        {
            var record = await _context.Set<Book>().FindAsync(id);
            _context.Entry(record).CurrentValues.SetValues(entity);
        }
    }
}
