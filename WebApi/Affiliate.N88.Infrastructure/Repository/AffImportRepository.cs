using Affiliate.N88.DTO;
using Affiliate.N88.Exceptions;
using Affiliate.N88.SpParams.Commands;
using Fanex.Data.Repository;
using System;
using System.Threading.Tasks;

namespace Affiliate.N88.Repository
{
    public class AffImportRepository : IAffMemberRepository
    {
        private readonly IDynamicRepository _dynamicRepository;

        public AffImportRepository(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }

        public async Task<DateTime> GetLastedMemberImportedTimeAsync(MemberImportedTimeInfo settingInfo)
            => await _dynamicRepository.GetAsync<DateTime>(settingInfo);

        public async Task<string> GetSiteNameAsync(SiteNameInfo settingInfo)
            => await _dynamicRepository.GetAsync<string>(settingInfo);

        public async Task ImportMemberAsync(InsertMemberInfoCommand memberInfo)
        {
            await _dynamicRepository.ExecuteAsync(memberInfo);
            if (memberInfo.ErrCode != 0)
            {
                throw new ImporterException(memberInfo.ErrCode.ToString(), memberInfo.ErrMsg ?? string.Empty, memberInfo.XmlData ?? string.Empty);
            }
        }

        public async Task InsertAffMemberTimeTrackerAsync(InsertAffMemberTimeTrackerCommand command)
        {
            await _dynamicRepository.ExecuteAsync(command);

            if (command.ErrCode != 0)
            {
                throw new ImporterException(command.ErrCode, command.ErrMsg ?? string.Empty);
            }
        }
    }
}