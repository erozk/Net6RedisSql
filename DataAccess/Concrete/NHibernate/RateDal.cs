using Net6RedisSql.Abstract.Orm;
using Net6RedisSql.DataAccess.Abstract;
using Net6RedisSql.Entities;

namespace Net6RedisSql.DataAccess.Concrete.NHibernate
{
    public class RateDal : NHibernateEntityRepositoryBase<Rate, ApiDbContext>, IRateDal
    {
        
    }
}