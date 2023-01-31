using Net6RedisSql.Core;
using Net6RedisSql.Entities;

namespace Net6RedisSql.DataAccess.Abstract
{
    public interface IRateDal : IEntityRepository<Rate>
    {
             // sp or view operations
    }
}