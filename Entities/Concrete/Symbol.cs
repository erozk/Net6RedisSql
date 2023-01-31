using System.ComponentModel.DataAnnotations;
using Net6RedisSql.Abstract;

namespace Net6RedisSql.Entities
{
    public class Symbol : IEntity
    {
        [Key]
        public int Id { get; set; }
        public string SymbolName { get; set; }
        public string LongName { get; set; }

    }
}