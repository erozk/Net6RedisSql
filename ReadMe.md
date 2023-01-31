.NET 6 Web API for Redis Cache, Distributed Lock, Sqlite

 with Dependency Injection 


* ORMs and Databases can be immplemented optionally 

    - In the project > Entity Framework Core, Redis and Sqlite were used




1- Sqlite DB Migration

# Project Reference
    dotnet add package Microsoft.EntityFrameworkCore

    dotnet add package Microsoft.EntityFrameworkCore.Sqlite

    dotnet add package Microsoft.EntityFrameworkCore.Design

    dotnet ef migrations add InitialCreate --context ApiDbContext

    dotnet ef database update --context ApiDbContext  

    <!-- for fresh start on db  -->
    -- dotnet ef database update 0 --context ApiDbContext

2- Redis 

# Setup

    To run redis server on windows

    Download and run redis-server.exe
    https://github.com/microsoftarchive/redis/releases/download/win-3.0.504/Redis-x64-3.0.504.zip

    then you can run redis-cli.exe

    Redis will be ready for test instance

# Project Reference

    dotnet add package Microsoft.Extensions.Caching.StackExchangeRedis

    in appsettings add Redis url
      "RedisCacheUrl": "127.0.0.1:6379"