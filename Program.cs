using Medallion.Threading;
using Medallion.Threading.Redis;
using Microsoft.OpenApi.Models;
using Net6RedisSql.Abstract.Orm;
using Net6RedisSql.Business.Abstract;
using Net6RedisSql.Business.Managers;
using Net6RedisSql.Core;
using Net6RedisSql.DataAccess.Abstract;
using Net6RedisSql.DataAccess.Concrete.EntityFramework;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// builder.Services.AddDbContext<ApiDbContext>(option =>
//     option.UseSqlite(builder.Configuration.GetConnectionString("SqLiteWebApiDatabase")));

//builder.Services.AddTransient<IEntityRepository,EntityFrameworkEntityRepositoryBase>();
builder.Services.AddScoped<ITradeService,TradeManager>();
builder.Services.AddScoped<ITradeDal, TradeDal>();
builder.Services.AddScoped<IRateService,RateManager>();
builder.Services.AddScoped<IRateDal, RateDal>();
builder.Services.AddStackExchangeRedisCache(options => { options.Configuration = builder.Configuration["RedisCacheUrl"]; });

builder.Services.AddSingleton<IDistributedLockProvider>(sp =>
{
    var connection = ConnectionMultiplexer.Connect(builder.Configuration["Redis:Configuration"]);
    return new RedisDistributedSynchronizationProvider(connection.GetDatabase());
});

//builder.Services.AddSingleton<IDistributedLockProvider>(_ => new RedisDistributedSynchronizationProvider(""));
//services.AddTransienSomeService>();


// services.AddSingleton<IConnectionMultiplexer>(sp =>
//                  ConnectionMultiplexer.Connect(new ConfigurationOptions
//                  {
//                      EndPoints = { $"{Configuration.GetValue<string>("RedisCache:Host")}: 
//                   {Configuration.GetValue<int>("RedisCache:Port")}" },
//                   AbortOnConnectFail = false,
//                }));



builder.Services.AddControllers();



// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "SqliteRedisCacheAndLock",
        Version = "v1"
    });
});
// builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    //app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "SqliteRedisCacheAndLock v1"));
    app.UseSwaggerUI(c =>
     {
         c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
         c.RoutePrefix = "";
     });
}

//app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
