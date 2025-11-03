namespace Affiliate.N88.Models
{
    public class PaymentRequestInfo
    {
        public int CustId { get; set; }

        public string? UserName { get; set; }

        public int SiteId { get; set; }

        public decimal Amount { get; set; }

        public int Type { get; set; }

        public string? Memo { get; set; }

        public string? RefTransId { get; set; }
    }
}
