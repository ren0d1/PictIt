namespace PictIt.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using Microsoft.EntityFrameworkCore;

    using PictIt.Models;

    public abstract class BaseRepository<T, S> : IDisposable, IRepository<T, S> where T : class, IModel<S>
    {
        private readonly AppDbContext context;
        private bool disposed;

        protected BaseRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await context.Set<T>().ToListAsync();
        }

        public async Task<T> GetById(S id)
        {
            return await context.Set<T>().FirstOrDefaultAsync(o => o.Id.Equals(id));
        }

        public async Task<IEnumerable<T>> SearchFor(Expression<Func<T, bool>> predicate)
        {
            return await context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<bool> Insert(T entity)
        {
            try
            {
                await context.Set<T>().AddAsync(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(GetType() + " has seen the following error occur during the insert method: " + e.Message);
                return false;
            }

            return true;
        }

        public async Task Update(T entity)
        {
            await Task.FromResult(context.Set<T>().Update(entity));
        }

        public async Task Delete(S id)
        {
            try
            {
                var entity = await context.Set<T>().FindAsync(id);

                context.Set<T>().Remove(entity);
            }
            catch (Exception e)
            {
                Console.WriteLine(GetType() + " has seen the following error occur during the insert method: " + e.Message);
            }
        }

        public async Task<int> Save()
        {
            return await context.SaveChangesAsync();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            if (disposing)
            {
                context.Dispose();
            }

            disposed = true;
        }
    }
}
