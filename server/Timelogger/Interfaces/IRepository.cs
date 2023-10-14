using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Threading;

namespace Timelogger.Core.Interfaces
{
    public interface IRepository<TEntity> where TEntity : class
    {
        Task<TEntity> GetByIdAsync(int id);
        Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken cancellationToken);
        Task AddAsync(TEntity entity);
        void Update(TEntity entity);
        void Remove(TEntity entity);
        Task<bool> SaveChangesAsync(CancellationToken cancellationToken);
    }
}