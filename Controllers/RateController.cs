using System.Linq.Expressions;
using System.Text;
using Medallion.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Net6RedisSql.Business.Abstract;
using Net6RedisSql.Entities;
using Newtonsoft.Json;

namespace Net6RedisSql.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RateController : ControllerBase
{

    private readonly IRateService _rateService;
    private readonly IDistributedCache _redisDistributedCache;

    public RateController(IRateService rateService, IDistributedCache distributedCache)
    {
        _rateService = rateService;
        _redisDistributedCache = distributedCache;
    }

    [HttpGet]
    public async Task<IActionResult> GetRates(string currency)
    {
        List<Rate> rates = new List<Rate>();
        try
        {
            string cacheJsonItem;
            var clientRatesFromCache = await _redisDistributedCache.GetAsync(currency +"_rates");
            if (clientRatesFromCache != null)
            {
                cacheJsonItem = Encoding.UTF8.GetString(clientRatesFromCache);
                rates = JsonConvert.DeserializeObject<List<Rate>>(cacheJsonItem);
            }
            else
            {
                Expression<Func<Rate, bool>> filter = m => m.BaseCurrency == currency;
                rates = await Task.Run(() => _rateService.GetList(filter));
                if (rates.Count > 0)
                {
                    cacheJsonItem = JsonConvert.SerializeObject(rates);
                    clientRatesFromCache = Encoding.UTF8.GetBytes(cacheJsonItem);
                    var options = new DistributedCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromMinutes(5))
                            .SetAbsoluteExpiration(DateTime.Now.AddMinutes(25));
                    await _redisDistributedCache.SetAsync(currency +"_rates", clientRatesFromCache, options);
                }
            }
            return Ok(rates);
        }
        catch (System.Exception)
        {
            return BadRequest(rates);
        }

    }
    
}
