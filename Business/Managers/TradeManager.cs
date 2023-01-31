using System.Linq.Expressions;
using Net6RedisSql.Business.Abstract;
using Net6RedisSql.DataAccess.Abstract;
using Net6RedisSql.Entities;
using Net6RedisSql.Entities.Dto;

namespace Net6RedisSql.Business.Managers
{
    public class TradeManager : ITradeService
    {
        private readonly ITradeDal _tradedal;

        public TradeManager(ITradeDal tradedal)
        {
            _tradedal = tradedal;
        }

        public void Add(Trade entity)
        {
            _tradedal.Add(entity);
        }

        public List<Trade> GetList(Expression<Func<Trade, bool>>? filter = null)
        {
            return _tradedal.GetList(filter);
        }

        public List<TradeDto> GetListWithDetails(Expression<Func<Trade, bool>> filter = null)
        {
            return new List<TradeDto>(_tradedal.GetListWithDetails(filter));
        }

    }
}