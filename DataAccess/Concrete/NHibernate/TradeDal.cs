using System.Linq.Expressions;
using Net6RedisSql.Abstract.Orm;
using Net6RedisSql.DataAccess.Abstract;
using Net6RedisSql.Entities;
using Net6RedisSql.Entities.Dto;

namespace Net6RedisSql.DataAccess.Concrete.NHibernate
{
    public class TradeDal : NHibernateEntityRepositoryBase<Trade, ApiDbContext>, ITradeDal
    {
        public List<TradeDto> GetListWithDetails(Expression<Func<Trade, bool>> filter = null)
        {
            throw new NotImplementedException();
        }
    }
}