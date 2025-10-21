using Affiliate.N88.Models;
using Affiliate.N88.SpParams.Criteria;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Affiliate.N88.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BetReportController : ControllerBase
    {
        private readonly IDynamicRepository dynamicRepository;

        public BetReportController(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        [HttpGet("GetBetRecord")]
        public async Task<ActionResult<IEnumerable<TransBetRecordInfo>>> GetBetRecord(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize = 2000)
        {
            var betlist = await dynamicRepository.FetchAsync<TransBetRecordInfo>(new GetBetRecordCriteria(fromDate, toDate, pageIndex, pageSize));
            return Ok(betlist);
        }

        [HttpGet("GetTpTransferInfo")]
        public async Task<ActionResult<IEnumerable<TpTransferInfo>>> GetTpTransferInfo(int id, int totalRow)
            => Ok(await dynamicRepository.FetchAsync<TpTransferInfo>(new GetTpTransferInfoCriteria(id, totalRow)));

        [HttpGet("GetDepositInfo")]
        public async Task<ActionResult<IEnumerable<DepositInfo>>> GetDepositInfo(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize = 2000)
            => Ok(await dynamicRepository.FetchAsync<DepositInfo>(new GetDepositInfoCriteria(fromDate, toDate, pageIndex, pageSize)));

        [HttpGet("GetBonusInfo")]
        public async Task<ActionResult<IEnumerable<BonusDTO>>> GetBonusInfo(int id, int batchCount = 2000)
            => Ok(await dynamicRepository.FetchAsync<BonusDTO>(new GetBonusInfoCriteria(id, batchCount)));

        [HttpGet("GetBonusInfoByDate")]
        public async Task<ActionResult<IEnumerable<BonusDTO>>> GetBonusInfoByDate(DateTime calledTime, int pageIndex, int pageSize = 2000)
            => Ok(await dynamicRepository.FetchAsync<BonusDTO>(new GetBonusInfoByDateCriteria(calledTime, pageIndex, pageSize)));

        [HttpGet("GetWithdrawalInfo")]
        public async Task<ActionResult<IEnumerable<Withdrawal>>> GetWithdrawalInfo(DateTime updateTime, int batchCount = 2000)
            => Ok(await dynamicRepository.FetchAsync<Withdrawal>(new GetWithdrawalInfoCriteria(updateTime, batchCount)));

        [HttpGet("GetAdjustmentInfos")]
        public async Task<ActionResult<IEnumerable<AdjustmentInfo>>> GetAdjustmentInfos(int id, int batchCount = 2000)
            => Ok(await dynamicRepository.FetchAsync<AdjustmentInfo>(new GetAdjustmentInfoCriteria(id, batchCount)));
    }
}