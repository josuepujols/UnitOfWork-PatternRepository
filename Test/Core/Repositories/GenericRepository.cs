using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Test.Core.IRepository;
using Test.Data;

namespace Test.Core.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        protected ApplicationDbContext _context;
        protected DbSet<T> _dbSet;
        protected readonly ILogger _logger;

        protected readonly DbContext Context;
        protected readonly DbSet<T> DbSet;

        public GenericRepository(ApplicationDbContext context)
        {

            Context = context;
            DbSet = Context.Set<T>();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await DbSet.ToListAsync();
        }

        public async Task<T> GetById(Guid Id)
        {
            return await DbSet.FindAsync(Id);
        }

        public async Task<bool> Add(T entity)
        {
            await DbSet.AddAsync(entity);
            return await CommitChanges();
        }

        public async Task<bool> Delete(Guid Id)
        {
            var objectToDelete = await DbSet.FindAsync(Id);
            DbSet.Remove(objectToDelete);
            return await CommitChanges();
        }

        public async virtual Task<bool> Update(T entity)
        {
            if (Context.Entry(entity).State == EntityState.Detached)
            {
                Context.Attach(entity);
            }
            Context.Entry(entity).State = EntityState.Modified;
            return await CommitChanges();
        }

        public async Task<T> FindWhere(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        public async Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).ToListAsync();
        }

        public async Task<bool> Exists(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }

        public async Task<bool> CommitChanges()
        {
            if (await Context.SaveChangesAsync() >= 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
