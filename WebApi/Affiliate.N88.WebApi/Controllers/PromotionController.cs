using Affiliate.N88.Models;
using Affiliate.N88.SpParams.Criteria;
using Affiliate.N88.WebApi.Repository;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Affiliate.N88.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PromotionController : ControllerBase
    {
        private IDynamicRepository DynamicRepo { get; }
        
        public PromotionController(IDynamicRepository dynamicRepository)
        {
            DynamicRepo = dynamicRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromotionModel>>> GetPromotions(DateTime fromDate, DateTime toDate)
            => Ok(await DynamicRepo.FetchAsync<PromotionModel>(new GetPromotionCriteria(fromDate, toDate)));

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PromotionSpecificModel>>> GetPromotionSpecifics(DateTime fromDate, DateTime toDate)
            => Ok(await DynamicRepo.FetchAsync<PromotionSpecificModel>(new GetPromotionSpecificCriteria(fromDate, toDate)));
    }
}
