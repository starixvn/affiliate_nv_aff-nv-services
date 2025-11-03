using Affiliate.N88.Exceptions;
using Affiliate.N88.SpParams.Commands;
using Fanex.Data.Repository;
using System.Threading.Tasks;

namespace Affiliate.N88.Repository
{
    public class ThirdPartyImportRepository : IThirdPartyImportRepository
    {
        public readonly IDynamicRepository _dynamicRepository;

        public ThirdPartyImportRepository(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }

        public async Task ImportRecordAsync(InsertThirdPartyRecodInfoCommand betRecord)
        {
            await _dynamicRepository.ExecuteAsync(betRecord);
            if (betRecord.ErrCode != 0)
            {
                throw new ImporterException(betRecord.ErrCode.ToString(), betRecord.ErrMsg ?? string.Empty, betRecord.XmlData ?? string.Empty);
            }
        }
    }
}
