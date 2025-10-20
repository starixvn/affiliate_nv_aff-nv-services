using Affiliate.N88.Models;
using Affiliate.N88.WebApi.Models.Fake;
using Microsoft.AspNetCore.Mvc;

namespace Affiliate.N88.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FakeController : ControllerBase
    {

        [HttpPost]
        public ActionResult<SetAccountResult> SetCustActStatus(AccountParam param)
        {
            return Ok(new SetAccountResult
            {
                Data = false,
                ErrorCode = 99
            });
        }
      
    }
}
