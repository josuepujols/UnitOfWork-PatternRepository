using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Test.Core.IRepository;
using Test.Data;

namespace Test.Core.Repositories
{
    public class UnitOfWork : IDisposable
    {
        private Dictionary<string, object> repositories;
        public DbContext Db { get; }

        public UnitOfWork(DbContextOptions<ApplicationDbContext> opt)
        {
            Db = new ApplicationDbContext(opt);
        }

        public GenericRepository<T> Repository<T>() where T : class
        {
            if (Singleton.Instance.repositories == null)
            {
                Singleton.Instance.repositories = new Dictionary<string, object>();
            }

            var type = typeof(T).Name;

            if (!Singleton.Instance.repositories.ContainsKey(type))
            {
                var repositoryType = typeof(GenericRepository<>);
                object repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), Db);
                Singleton.Instance.repositories.Add(type, repositoryInstance);
            }

            return (GenericRepository<T>)Singleton.Instance.repositories[type];
        }

        public void Dispose()
        {
            
        }
    }
}
