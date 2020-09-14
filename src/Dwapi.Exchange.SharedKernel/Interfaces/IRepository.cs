using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dwapi.Exchange.SharedKernel.Model;

namespace Dwapi.Exchange.SharedKernel.Interfaces
{
    public interface IRepository<T, in TId>  where T : Entity<TId>
    {
        Task<T> GetAsync(TId id);
        Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate);
        Task CreateAsync(T entity);
        Task UpdateAsync(T entity);
        Task DeleteAsync(TId id);
    }
}
