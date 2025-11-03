using Fanex.Data.Repository;

namespace Affiliate.N88.SpParams.Commands
{
    public class UnlockUsersCommand : NonQueryCommand
    {
        public string? CustIdList { get; set; }

        public override string GetSettingKey() => "API_Customer_Unlock";

        public override bool IsValid() => true;
    }
}
