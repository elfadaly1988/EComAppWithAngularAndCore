using ECom.Core.Interfaces;
using ECom.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECom.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly AppDbContext _context;

        public GenericRepository(AppDbContext context)
        {
            this._context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _context.Set<T>().AddAsync(entity);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            if (entity == null)
            {
                throw new InvalidOperationException($"Entity with id {id} not found.");
            }
            _context.Set<T>().Remove(entity);
            await _context.SaveChangesAsync();
        }
        public async Task<IReadOnlyList<T>> GetAllAsync() => await _context.Set<T>().AsNoTracking().ToListAsync();

        public async Task<IReadOnlyList<T>> GetAllAsync(params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            return await query.ToListAsync();
        }

        public async Task<T> GetByIdAsync(int id)
        {
            var entity = await _context.Set<T>().FindAsync(id);
            return entity; //?? throw new InvalidOperationException();// ($"Entity with id {id} not found.");
        }

        public async Task<T> GetByIdAsync(int id, params Expression<Func<T, object>>[] includes)
        {
            var query = _context.Set<T>().AsQueryable();
            foreach (var item in includes)
            {
                query = query.Include(item);
            }
            var entity =await query.FirstOrDefaultAsync(x => EF.Property<int>(x, "Id") == id);
            return entity;
        }

        public async Task UpdateAsync(T entity)
        {
            var item = _context.Entry<T>(entity).State=EntityState.Modified;
            await _context.SaveChangesAsync();
        }
    }
}
