using Affiliate.N88.Constants;
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
        private readonly IDynamicRepository _dynamicRepository;

        public BetReportController(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }

        [HttpGet("GetBetRecord")]
        public async Task<ActionResult<IEnumerable<TransBetRecordInfo>>> GetBetRecord(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize = PagingConstants.DefaultPageSize)
        {
            var betlist = await _dynamicRepository.FetchAsync<TransBetRecordInfo>(new GetBetRecordCriteria(fromDate, toDate, pageIndex, pageSize));
            return Ok(betlist);
        }

        [HttpGet("GetTpTransferInfo")]
        public async Task<ActionResult<IEnumerable<TpTransferInfo>>> GetTpTransferInfo(int id, int totalRow)
            => Ok(await _dynamicRepository.FetchAsync<TpTransferInfo>(new GetTpTransferInfoCriteria(id, totalRow)));

        [HttpGet("GetDepositInfo")]
        public async Task<ActionResult<IEnumerable<DepositInfo>>> GetDepositInfo(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize = PagingConstants.DefaultPageSize)
            => Ok(await _dynamicRepository.FetchAsync<DepositInfo>(new GetDepositInfoCriteria(fromDate, toDate, pageIndex, pageSize)));

        [HttpGet("GetBonusInfo")]
        public async Task<ActionResult<IEnumerable<BonusDTO>>> GetBonusInfo(int id, int batchCount = PagingConstants.DefaultPageSize)
            => Ok(await _dynamicRepository.FetchAsync<BonusDTO>(new GetBonusInfoCriteria(id, batchCount)));

        [HttpGet("GetBonusInfoByDate")]
        public async Task<ActionResult<IEnumerable<BonusDTO>>> GetBonusInfoByDate(DateTime calledTime, int pageIndex, int pageSize = PagingConstants.DefaultPageSize)
            => Ok(await _dynamicRepository.FetchAsync<BonusDTO>(new GetBonusInfoByDateCriteria(calledTime, pageIndex, pageSize)));

        [HttpGet("GetWithdrawalInfo")]
        public async Task<ActionResult<IEnumerable<Withdrawal>>> GetWithdrawalInfo(DateTime updateTime, int batchCount = PagingConstants.DefaultPageSize)
            => Ok(await _dynamicRepository.FetchAsync<Withdrawal>(new GetWithdrawalInfoCriteria(updateTime, batchCount)));

        [HttpGet("GetAdjustmentInfos")]
        public async Task<ActionResult<IEnumerable<AdjustmentInfo>>> GetAdjustmentInfos(int id, int batchCount = PagingConstants.DefaultPageSize)
            => Ok(await _dynamicRepository.FetchAsync<AdjustmentInfo>(new GetAdjustmentInfoCriteria(id, batchCount)));
    }
}
