using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Test.Core.IRepository
{
    interface IGenericRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(Guid Id);
        Task<T> FindWhere(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetList(Expression<Func<T, bool>> predicate);
        Task<bool> Add(T entity);
        Task<bool> Delete(Guid Id);
        Task<bool> Update(T entity);
        Task<bool> Exists(Expression<Func<T, bool>> predicate);
        Task<bool> CommitChanges();
    }
}
