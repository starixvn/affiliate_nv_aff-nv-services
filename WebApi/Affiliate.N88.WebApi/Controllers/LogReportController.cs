using Affiliate.N88.Constants;
using Affiliate.N88.Models;
using Affiliate.N88.SpParams.Criteria;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Affiliate.N88.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogReportController : ControllerBase
    {
        private readonly IDynamicRepository _repo;

        public LogReportController(IDynamicRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("GetRebateMLogs")]
        public async Task<ActionResult<IEnumerable<RebateMLogModel>>> GetRebateMLogs(DateTime fromDate, DateTime toDate)
            => Ok(await _repo.FetchAsync<RebateMLogModel>(new GetRebateMLogCriteria(fromDate, toDate)));

        [HttpGet("GetDrawLogs")]
        public async Task<ActionResult<IEnumerable<DrawLogInfo>>> GetDrawLogs(int id, int batchCount = PagingConstants.DefaultBatchCount)
            => Ok(await _repo.FetchAsync<DrawLogInfo>(new GetDrawLogCritearia(id, batchCount)));

        [HttpGet("GetEventDraws")]
        public async Task<ActionResult<IEnumerable<EventDrawInfo>>> GetEventDraws(int id, int batchCount = PagingConstants.DefaultBatchCount)
            => Ok(await _repo.FetchAsync<EventDrawInfo>(new GetEventDrawCritearia(id, batchCount)));

        [HttpGet("GetLogAdminAdjustWallet")]
        public async Task<ActionResult<IEnumerable<LogAdminAdjustWalletInfo>>> GetLogAdminAdjustWallet(int id, int batchCount = PagingConstants.DefaultBatchCount)
            => Ok(await _repo.FetchAsync<LogAdminAdjustWalletInfo>(new GetLogAdminAdjustWalletCriteria(id, batchCount)));

        [HttpGet("GetReferralMLogs")]
        public async Task<ActionResult<IEnumerable<ReferralMLogModel>>> GetReferralMLogs(int id, int batchCount = PagingConstants.DefaultBatchCount)
            => Ok(await _repo.FetchAsync<ReferralMLogModel>(new GetReferralMLogCriteria(id, batchCount)));

        [HttpGet("GetReferralLogs")]
        public async Task<ActionResult<IEnumerable<ReferralLogModel>>> GetReferralLogs(int id, int batchCount = PagingConstants.DefaultBatchCount)
            => Ok(await _repo.FetchAsync<ReferralLogModel>(new GetReferralLogCriteria(id, batchCount)));
    }
}
