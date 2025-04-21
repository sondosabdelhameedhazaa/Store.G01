using Domain.Contracts;
using Domain.Models;
using Persistence.Data;
//using Persistence.Data.Repositories;
using Persistence.Repositories;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _context;
        //private readonly Dictionary<string, object> _repositories;
        private readonly ConcurrentDictionary<string, object> _repositories;

        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
            _repositories = new ConcurrentDictionary<string, object>();
        }
        public IGenericRepository<TEntity, Tkey> GetRepository<TEntity, Tkey>() where TEntity : BaseEntity<Tkey>
        {
            //var type=typeof(TEntity).Name;
            //if(!_repositories.ContainsKey(type))
            //{
            //    var repository=new GenericRepository<TEntity,Tkey>(_context);
            //    _repositories.Add(type, repository);
            //}
            //return (IGenericRepository<TEntity, Tkey>) _repositories[type];
            return (IGenericRepository<TEntity, Tkey>)_repositories.GetOrAdd(typeof(TEntity).Name, new GenericRepository<TEntity, Tkey>(_context));
        }

        public async Task<int> SaveChangesAsync()
        {
            return await SaveChangesAsync();
        }
    }
}