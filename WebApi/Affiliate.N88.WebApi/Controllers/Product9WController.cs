using Affiliate.N88.Models;
using Affiliate.N88.Params;
using Affiliate.N88.SpParams.Criteria;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Affiliate.N88.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Product9WController : ControllerBase
    {

        private readonly IDynamicRepository dynamicRepository;

        public Product9WController(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        [HttpGet("GetTpReport")]
        public async Task<ActionResult<IEnumerable<TpReport9W>>> GetTpReport([FromQuery] BatchCountParam param)
            => Ok(await dynamicRepository.FetchAsync<TpReport9W>(new GetTpReport9WCriteria
            {
                SQ = param.SQ,
                FromDate = param.FromDate,
                ToDate = param.ToDate,
                BatchCount = param.BatchCount
            }));

        [HttpGet("GetTpWager")]
        public async Task<ActionResult<IEnumerable<TpWager9W>>> GetTpWager([FromQuery] BatchCountParam param)
            => Ok(await dynamicRepository.FetchAsync<TpWager9W>(new GetTpWager9WCriteria
            {
                CreateTime = param.CreateTime,
                FromDate = param.FromDate,
                ToDate = param.ToDate,
                BatchCount = param.BatchCount
            }));
    }
}