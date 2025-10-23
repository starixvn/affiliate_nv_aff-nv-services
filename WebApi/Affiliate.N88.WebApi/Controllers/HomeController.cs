using Affiliate.N88.Models;
using Affiliate.N88.SpParams.Criteria;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Affiliate.N88.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IDynamicRepository _dynamicRepository;
        private readonly IConfiguration _configuration;

        public HomeController(IDynamicRepository dynamicRepository, IConfiguration configuration)
        {
            _dynamicRepository = dynamicRepository;
            _configuration = configuration;
        }

        [HttpGet("Index")]
        public async Task<ActionResult> Index()
        {
            var connectedMessage = "Database connects successfully";
            try
            {
                await _dynamicRepository.FetchAsync<ServerInfo>(new GetServerInfoCriteria("Slave"));
            }
            catch (Exception ex)
            {
                connectedMessage = ex.ToString();
            }

            var env = _configuration["Enviroment"];

            return Content($"Site is running {env} <br/> {connectedMessage}", "text/html");
        }
    }
}