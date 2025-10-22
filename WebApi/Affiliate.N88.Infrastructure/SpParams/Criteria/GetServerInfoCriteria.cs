using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Criteria
{
    public class GetServerInfoCriteria : CriteriaBase
    {
        private const string SP_NAME = "N88_Get_ServerInfo";

        private string? Prefix { get; }

        public GetServerInfoCriteria(string? prefix = null)
        {
            Prefix = prefix;
        }

        public override string GetSettingKey() => string.IsNullOrWhiteSpace(Prefix) ? SP_NAME : $"{Prefix}_{SP_NAME}";

        public override bool IsValid() => true;
    }
}
