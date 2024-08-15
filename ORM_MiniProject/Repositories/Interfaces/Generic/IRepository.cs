using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ORM_MiniProject.Repositories.Interfaces.Generic
{
    public interface IRepository<T>
    {
        Task<List<T>> GetAllAsync(params string[] includes);
        Task<T?> GetAsync(Expression<Func<T,bool>> expression,params string[] includes);
        Task<bool> IsExistAsync(Expression<Func<T, bool>> expression);
        Task CreateAsync(T entity);
        void Update(T entity); 
        void Delete(T entity);
        Task<int> SaveChangesAsync();
        Task<List<T>> GetFilterAsync(Expression<Func<T,bool> > expression);
    }
}
