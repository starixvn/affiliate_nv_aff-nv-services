using Affiliate.N88.Models;
using Affiliate.N88.SpParams.Criteria;
using Fanex.Data.Repository;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Affiliate.N88.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IDynamicRepository dynamicRepository;
        private readonly IConfiguration configuration;

        public HomeController(IDynamicRepository dynamicRepository, IConfiguration configuration)
        {
            this.dynamicRepository = dynamicRepository;
            this.configuration = configuration;
        }

        [HttpGet("Index")]
        public async Task<ActionResult> Index()
        {
            var connectedMessage = "Database connects successfully";
            try
            {
                await dynamicRepository.FetchAsync<ServerInfo>(new GetServerInfoCriteria("Slave"));
            }
            catch (Exception ex)
            {
                connectedMessage = ex.ToString();
            }

            var x = 0;
            var y = 1 / x;
            var env = configuration["Enviroment"];

            return Content($"Site is running {env} <br/> {connectedMessage}", "text/html");
        }
    }
}