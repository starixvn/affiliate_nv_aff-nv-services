using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Commands
{
    public class UpdateCustomerWalletCommand : NonQueryCommand
    {
        public override string GetSettingKey()
            => "Affiliate_Wallet_Set";

        public override bool IsValid()
            => true;

        public int CustId { get; set; }

        public string? UserName { get; set; }

        public int SiteId { get; set; }

        public decimal Amount { get; set; }

        public int Type { get; set; }

        public string? Memo { get; set; }

        public string? RefTransId { get; set; }

        public decimal AfterAmount { get; set; }
    }
}
