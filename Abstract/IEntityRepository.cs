using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Net6RedisSql.Abstract;

namespace Net6RedisSql.Core
{

    public interface IEntityRepository<T> 
        where T : class, IEntity, new()
    {
        T Get(Expression<Func<T,bool>> filter = null); 

        List<T> GetList(Expression<Func<T,bool>> filter = null);

        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}