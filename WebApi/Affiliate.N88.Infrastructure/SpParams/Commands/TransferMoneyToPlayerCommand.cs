using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.N88.SpParams.Commands
{
    public class TransferMoneyToPlayerCommand : NonQueryCommand
    {
        public override string GetSettingKey() => "Affiliate_TransferToPlayer";

        public override bool IsValid() => true;

        public int SiteId { get; set; }

        public string? RefTransId { get; set; }

        public int FromCustId { get; set; }

        public decimal FromAmount { get; set; }

        public int ToCustId { get; set; }

        public decimal ToAmount { get; set; }

        public decimal MaxTransferAmount { get; set; }

        public int ErrCode { get; set; }
    }
}
