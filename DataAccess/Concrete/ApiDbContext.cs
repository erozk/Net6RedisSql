using Microsoft.EntityFrameworkCore;
using Net6RedisSql.DataAccess.Abstract;
using Net6RedisSql.Entities;

namespace Net6RedisSql.DataAccess
{
    public class ApiDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlite("Data Source=SqLiteDatabase.db");
        }

        public DbSet<Trade> Trades { get; set; }

        public DbSet<Symbol> Symbols { get; set; }

        public DbSet<Rate> Rates { get; set; }
        
    }
    
}