using System.ComponentModel.DataAnnotations;
using Net6RedisSql.Abstract;

namespace Net6RedisSql.Entities
{
    public class Trade : IEntity
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public int ClientId { get; set; }
        [Required]
        [Range(0.1, float.MaxValue, ErrorMessage = "Please enter a value bigger than {0.1}")]
        public float Amount { get; set; }
        public int BaseCurrencyId { get; set; }
        public int CurrencyId { get; set; }
        public float NewAmount { get; set; }
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value bigger than {0}")]
        public int UsedRateId { get; set; }
        public Int64 TimeStamp { get; set; }
    }
}