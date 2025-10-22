namespace Affiliate.N88.WebApi.Models
{
    public class TransferMoneyParam
    {
        public int SiteId { get; set; }

        public string? RefTransId { get; set; }

        public int FromCustId { get; set; }

        public decimal FromAmount { get; set; }

        public int ToCustId { get; set; }

        public decimal ToAmount { get; set; }

        public decimal MaxTransferAmount { get; set; }
    }
}