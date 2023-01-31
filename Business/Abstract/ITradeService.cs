using System.Linq.Expressions;
using Net6RedisSql.Entities;
using Net6RedisSql.Entities.Dto;

namespace Net6RedisSql.Business.Abstract
{
    public interface ITradeService
    {
        List<Trade> GetList(Expression<Func<Trade, bool>>? filter = null);

        List<TradeDto> GetListWithDetails(Expression<Func<Trade, bool>> filter = null);

        void Add(Trade entity);
    }
}