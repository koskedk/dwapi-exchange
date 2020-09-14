using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Dwapi.Exchange.SharedKernel.Interfaces;
using Dwapi.Exchange.SharedKernel.Model;
using Microsoft.EntityFrameworkCore;

namespace Dwapi.Exchange.SharedKernel.Infrastructure.Data
{
   public abstract class Repository<T, TId> : IRepository<T, TId> where T : Entity<TId>
    {
        protected internal DbContext Context;
        protected internal readonly DbSet<T> DbSet;
        
        protected Repository(DbContext context)
        {
            Context = context;
            DbSet = Context.Set<T>();
        }
    
        public async Task<T> GetAsync(TId id)
        {
            return await DbSet.FindAsync(id);
        }

        public async Task<IEnumerable<T>> SearchAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.Where(predicate).AsNoTracking().ToListAsync();
        }

        public async Task CreateAsync(T entity)
        {
            Context.Add(entity);
            await Context.SaveChangesAsync();
        }

        public async Task UpdateAsync(T entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
        }

        public async Task DeleteAsync(TId id)
        {
            var entity = await GetAsync(id);
            if (null != entity)
            {
                DbSet.Remove(entity);
                await Context.SaveChangesAsync();
            }
        }
    }
}