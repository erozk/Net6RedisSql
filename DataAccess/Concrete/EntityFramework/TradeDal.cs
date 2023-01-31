using System.Linq.Expressions;
using Net6RedisSql.Abstract.Orm;
using Net6RedisSql.DataAccess.Abstract;
using Net6RedisSql.Entities;
using Net6RedisSql.Entities.Dto;

namespace Net6RedisSql.DataAccess.Concrete.EntityFramework
{
    public class TradeDal : EntityFrameworkEntityRepositoryBase<Trade, ApiDbContext>, ITradeDal
    {
        public List<TradeDto> GetListWithDetails(Expression<Func<Trade, bool>> filter = null)
        {
            using (ApiDbContext context = new ApiDbContext())
            {
                var result = from c in filter == null ? context.Trades : context.Trades.Where(filter)
                             join ra in context.Rates
                             on c.UsedRateId equals ra.Id
                             join co in context.Symbols
                             on ra.BaseCurrency equals co.SymbolName
                             join cor in context.Symbols
                             on ra.Currency equals cor.SymbolName
                             
                             select new TradeDto
                             {
                                 Id = c.Id,
                                 ClientId = c.ClientId,
                                 BaseCurrencyId = co.Id,
                                 BaseCurrencyName = co.SymbolName,
                                 CurrencyId = cor.Id,
                                 CurrencyName = cor.SymbolName,
                                 Amount = c.Amount,
                                 NewAmount=c.NewAmount,
                                 UsedRateId = c.UsedRateId,
                                 UsedCurrencyRate = ra.CurrencyRate,
                                 TimeStamp =c.TimeStamp
                             };
                return result.ToList();
            }
        }
    }
}