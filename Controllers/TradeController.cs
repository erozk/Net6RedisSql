using System.Linq.Expressions;
using System.Text;
using Medallion.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Net6RedisSql.Business.Abstract;
using Net6RedisSql.Entities;
using Net6RedisSql.Entities.Dto;
using Newtonsoft.Json;

namespace Net6RedisSql.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TradeController : ControllerBase
{

    private readonly ITradeService _tradeService;

    private readonly IRateService _rateService;
    private readonly IDistributedCache _redisDistributedCache;
    private readonly IDistributedLockProvider _synchronizationProvider;

    public TradeController(ITradeService tradeService, IRateService rateService, IDistributedCache distributedCache, IDistributedLockProvider synchronizationProvider)
    {
        _tradeService = tradeService;
        _redisDistributedCache = distributedCache;
        _synchronizationProvider = synchronizationProvider;
        _rateService = rateService;
    }


    [HttpGet]
    public async Task<IActionResult> GetExchangeTradesAsync(string clientId)
    {
        List<TradeDto> clientTrades = new List<TradeDto>();
        string cacheJsonItem;
        try
        {
            var @lock = this._synchronizationProvider.CreateLock($"ClientAccount{clientId}");
            await using (var handle = await @lock.TryAcquireAsync())
            {
                if (handle != null)
                {
                    /* received lock */
                    var clientTradesFromCache = await _redisDistributedCache.GetAsync("Client" + clientId);
                    if (clientTradesFromCache != null)
                    {
                        cacheJsonItem = Encoding.UTF8.GetString(clientTradesFromCache);
                        clientTrades = JsonConvert.DeserializeObject<List<TradeDto>>(cacheJsonItem);
                    }
                    else
                    {
                        Expression<Func<Trade, bool>> filter = m => m.ClientId == int.Parse(clientId);
                        clientTrades = await Task.Run(() => _tradeService.GetListWithDetails(filter));
                        if (clientTrades.Count > 0)
                        {
                            cacheJsonItem = JsonConvert.SerializeObject(clientTrades);
                            clientTradesFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                            var options = new DistributedCacheEntryOptions()
                                    .SetSlidingExpiration(TimeSpan.FromMinutes(10))
                                    .SetAbsoluteExpiration(DateTime.Now.AddHours(1));
                            await _redisDistributedCache.SetAsync("Client" + clientId, clientTradesFromCache, options);
                        }
                    }
                    return Ok(clientTrades);
                }
                else
                    return BadRequest(clientTrades);
            }


        }
        catch (System.Exception)
        {
            return BadRequest(clientTrades);
        }

        //return clientTrades;
    }



    [HttpPost]
    public async Task<IActionResult> Create(Trade trade)
    {
        if (!ModelState.IsValid)
            return BadRequest();

        try
        {
            // To search latest 10 in 1 hour
            string cacheJsonItem;
            List<TradeDto> clientTrades = new List<TradeDto>();
            var clientTradesFromCache = await _redisDistributedCache.GetAsync("Client" + trade.ClientId);
            if (clientTradesFromCache != null)
            {
                cacheJsonItem = Encoding.UTF8.GetString(clientTradesFromCache);
                clientTrades = JsonConvert.DeserializeObject<List<TradeDto>>(cacheJsonItem);
            }
            if (clientTrades.Count > 10)
                return BadRequest("Client has more than 10 trades in an hour");



            // To check 30 minutes of usedRate
            Rate usedRate = new Rate();
            Expression<Func<Rate, bool>> filter = m => m.Id == trade.UsedRateId;
            usedRate = await Task.Run(() => _rateService.Get(filter));

            //if (DateTimeOffset.Now.AddMinutes(-30).ToUnixTimeMilliseconds() > usedRate.TimeStamp * 1000)
              //  return BadRequest("Rate value is overdated");


            var @lock = this._synchronizationProvider.CreateLock($"ClientNewTrade{trade.ClientId}");
            await using (var handle = await @lock.TryAcquireAsync())
            {
                if (handle != null)
                {
                    /* received lock */
                    trade.BaseCurrencyId = 0; // no need to use since we have usedRateId
                    trade.CurrencyId = 0; // no need to use since we have usedRateId..
                    trade.NewAmount = usedRate.CurrencyRate * trade.Amount;
                    trade.TimeStamp = DateTimeOffset.Now.ToUnixTimeSeconds();
                    await Task.Run(() => _tradeService.Add(trade));

                    //remove cache 
                    await _redisDistributedCache.RemoveAsync("Client" + trade.ClientId);
                    return Ok();
                }
                else
                    return BadRequest("Locked process");

            }

        }
        catch (System.Exception)
        {
            return BadRequest("Exception");
        }
    }
}
