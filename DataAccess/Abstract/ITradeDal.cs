using System.Linq.Expressions;
using Net6RedisSql.Core;
using Net6RedisSql.Entities;
using Net6RedisSql.Entities.Dto;

namespace Net6RedisSql.DataAccess.Abstract
{
    public interface ITradeDal : IEntityRepository<Trade>
    {
            List<TradeDto> GetListWithDetails(Expression<Func<Trade, bool>> filter = null);
    }
}