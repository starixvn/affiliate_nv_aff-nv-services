using Affiliate.N88.Constants;
using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetTpTransferInfoCriteria : CriteriaBase
    {
        public GetTpTransferInfoCriteria(int transferId, int batchCount = PagingConstants.DefaultBatchCount)
        {
            Id = transferId;
            BatchCount = batchCount;
        }

        public override string GetSettingKey() => "N88_Get_TpTransferInfo";

        public override bool IsValid() => true;

        public int Id { get; set; }

        public int BatchCount { get; set; }

    }
}
