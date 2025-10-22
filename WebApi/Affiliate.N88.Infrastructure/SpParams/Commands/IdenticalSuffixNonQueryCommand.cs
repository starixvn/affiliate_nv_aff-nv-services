using Affiliate.N88.Core;
using Affiliate.N88.SpParams.Criteria;
using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Commands
{
    public abstract class IdenticalSuffixNonQueryCommand : IdenticalSurfixCriteriaBase, INonQueryCommand
    {
        protected IdenticalSuffixNonQueryCommand(IPrefixProvider? prefixProvider = null) : base(prefixProvider)
        {
        }
    }
}
