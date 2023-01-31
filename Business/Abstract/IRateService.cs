using System.Linq.Expressions;
using Net6RedisSql.Entities;


namespace Net6RedisSql.Business.Abstract
{
    public interface IRateService
    {
        List<Rate> GetList(Expression<Func<Rate, bool>>? filter = null);
        Rate Get(Expression<Func<Rate, bool>>? filter = null);
    }
}