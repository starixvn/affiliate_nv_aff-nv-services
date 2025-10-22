using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetTpTransferInfoCriteria : CriteriaBase
    {
        public GetTpTransferInfoCriteria(int transferId, int batchCount = 2000)
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
