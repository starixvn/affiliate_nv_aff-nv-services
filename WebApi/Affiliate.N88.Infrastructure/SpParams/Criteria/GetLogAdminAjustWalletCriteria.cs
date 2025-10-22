using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetLogAdminAjustWalletCriteria : CriteriaBase
    {
        public GetLogAdminAjustWalletCriteria(int id, int batchCount)
        {
            Id = id;
            BatchCount = batchCount;
        }

        public int Id { get; set; }

        public int BatchCount { get; set; }

        public override string GetSettingKey()
            => "API_LogAdminAdjustWallet_Get";

        public override bool IsValid() => true;

    }
}
