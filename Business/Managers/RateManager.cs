using System.Linq.Expressions;
using Net6RedisSql.Business.Abstract;
using Net6RedisSql.DataAccess.Abstract;
using Net6RedisSql.Entities;

namespace Net6RedisSql.Business.Managers
{
    public class RateManager : IRateService
    {
        private readonly IRateDal _ratedal;

        public RateManager(IRateDal ratedal)
        {
            _ratedal = ratedal;
        }

        public Rate Get(Expression<Func<Rate, bool>>? filter) => _ratedal.Get(filter);

        public List<Rate> GetList(Expression<Func<Rate, bool>>? filter) => _ratedal.GetList(filter);

    }
}