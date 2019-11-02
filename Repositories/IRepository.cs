namespace PictIt.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using System.Threading.Tasks;

    using PictIt.Models;

    public interface IRepository<T, in S> where T : class, IModel<S>
    {
        Task<IEnumerable<T>> GetAll();

        Task<T> GetById(S id);

        Task<IEnumerable<T>> SearchFor(Expression<Func<T, bool>> predicate);

        Task<bool> Insert(T entity);

        Task Update(T entity);

        Task Delete(S id);

        Task<int> Save();
    }
}
