using Affiliate.N88.Core;
using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public abstract class IdenticalSurfixCriteriaBase : CriteriaBase
    {
        protected readonly IPrefixProvider? _prefixProvider;

        protected IdenticalSurfixCriteriaBase(IPrefixProvider? prefixProvider = null)
            => _prefixProvider = prefixProvider;
        
        protected abstract string SuffixSettingKey { get; }

        public override string GetSettingKey()
            => _prefixProvider == null || string.IsNullOrWhiteSpace(_prefixProvider.PrefixCode)
                ? SuffixSettingKey
                : $"{_prefixProvider.PrefixCode}_{SuffixSettingKey}";
    }
}
