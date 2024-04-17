using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace MvcApp.Services
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<IEnumerable<TEntity>> GetAllAsync(string? includes = null);
        Task<TEntity> GetByIdAsync(int id,string? includes = null);
        Task<TEntity> GetAsync(Expression<Func<TEntity,bool>> predicate, string? includes = null);
        Task CreateAsync(TEntity entity);
        Task UpdateAsync(int id,TEntity entity);
        Task DeleteAsync(int id);
    }
}
