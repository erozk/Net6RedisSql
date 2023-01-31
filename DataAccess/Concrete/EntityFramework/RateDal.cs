using Net6RedisSql.Abstract.Orm;
using Net6RedisSql.DataAccess.Abstract;
using Net6RedisSql.DataAccess;
using Net6RedisSql.Entities;

namespace Net6RedisSql.DataAccess.Concrete.EntityFramework
{
    public class RateDal : EntityFrameworkEntityRepositoryBase<Rate, ApiDbContext>, IRateDal 
    {
        
    }
}