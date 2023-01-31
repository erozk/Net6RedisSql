namespace Net6RedisSql.Entities.Dto
{
    public class TradeDto
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public float Amount { get; set; }
        public int BaseCurrencyId { get; set; }
        public string BaseCurrencyName { get; set; }
        public int CurrencyId { get; set; }
        public string CurrencyName { get; set; }
        public float NewAmount { get; set; }
        public int UsedRateId { get; set; }
        public float UsedCurrencyRate { get; set; }
        public Int64 TimeStamp { get; set; }
    }
}