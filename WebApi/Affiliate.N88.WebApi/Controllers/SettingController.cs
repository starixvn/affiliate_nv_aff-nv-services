using Affiliate.N88.Models;
using Affiliate.N88.SpParams.Criteria;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Affiliate.N88.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingController : ControllerBase
    {
        private readonly IDynamicRepository _dynamicRepository;

        public SettingController(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository;
        }

        [HttpGet("GetServerInfo")]
        public async Task<ActionResult<IEnumerable<ServerInfo>>> GetServerInfo()
            => Ok(await _dynamicRepository.FetchAsync<ServerInfo>(new GetServerInfoCriteria()));

        [HttpGet("GetTpGameType")]
        public async Task<ActionResult<IEnumerable<TpGameTypeInfo>>> GetTpGameType()
            => Ok(await _dynamicRepository.FetchAsync<TpGameTypeInfo>(new GetTpGameTypeCriteria()));
    }
}
