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
        public IDynamicRepository Repo { get; }

        public LogReportController(IDynamicRepository repo)
        {
            Repo = repo;
        }

        [HttpGet("GetRebateMLogs")]
        public async Task<ActionResult<IEnumerable<RebateMLogModel>>> GetRebateMLogs(DateTime fromDate, DateTime toDate)
            => Ok(await Repo.FetchAsync<RebateMLogModel>(new GetRebateMLogCriteria(fromDate, toDate)));

        [HttpGet("GetDrawLogs")]
        public async Task<ActionResult<IEnumerable<DrawLogInfo>>> GetDrawLogs(int id, int batchCount = 2000)
            => Ok(await Repo.FetchAsync<DrawLogInfo>(new GetDrawLogCritearia(id, batchCount)));

        [HttpGet("GetEventDraws")]
        public async Task<ActionResult<IEnumerable<EventDrawInfo>>> GetEventDraws(int id, int batchCount = 2000)
            => Ok(await Repo.FetchAsync<EventDrawInfo>(new GetEventDrawCritearia(id, batchCount)));

        [HttpGet("GetLogAdminAdjustWallet")]
        public async Task<ActionResult<IEnumerable<LogAdminAjustWalletInfo>>> GetLogAdminAdjustWallet(int id, int batchCount = 2000)
            => Ok(await Repo.FetchAsync<LogAdminAjustWalletInfo>(new GetLogAdminAjustWalletCriteria(id, batchCount)));

        [HttpGet("GetReferralMLogs")]
        public async Task<ActionResult<IEnumerable<ReferralMLogModel>>> GetReferralMLogs(int id, int batchCount = 2000)
            => Ok(await Repo.FetchAsync<ReferralMLogModel>(new GetReferralMLogCriteria(id, batchCount)));

        [HttpGet("GetReferralLogs")]
        public async Task<ActionResult<IEnumerable<ReferralLogModel>>> GetReferralLogs(int id, int batchCount = 2000)
            => Ok(await Repo.FetchAsync<ReferralLogModel>(new GetReferralLogCriteria(id, batchCount)));
    }
}
