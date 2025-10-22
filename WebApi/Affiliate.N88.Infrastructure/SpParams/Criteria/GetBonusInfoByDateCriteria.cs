using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetBonusInfoByDateCriteria : CriteriaBase
    {
        public DateTime CalledTime { get; }

        public int PageIndex { get; }

        public int PageSize { get; }

        public GetBonusInfoByDateCriteria(DateTime calledTime, int pageIndex, int pageSize)
        {
            CalledTime = calledTime;
            PageIndex= pageIndex;
            PageSize = pageSize;
        }

        public override string GetSettingKey() => "API_BonusInfoByDate_Get";

        public override bool IsValid() => true;
    }
}
