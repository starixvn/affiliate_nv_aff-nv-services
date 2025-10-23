namespace Affiliate.N88.Models
{
    public class DrawLogInfo
    {
        public int Id { get; set; }

        public int EventId { get; set; }

        public int CustId { get; set; }

        public int PrizeId { get; set; }

        public string? PrizeName { get; set; }

        public Decimal Amount { get; set; }

        public DateTime DrawEventDate { get; set; }

        public DateTime DrawTime { get; set; }

        public int SiteId { get; set; }

        public int CurrencyId { get; set; }
    }
}
