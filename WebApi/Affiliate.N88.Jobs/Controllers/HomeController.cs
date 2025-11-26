using Affiliate.N88.Jobs.Apis;
using Affiliate.N88.Jobs.RecurringTasks;
using Affiliate.N88.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        private readonly ILicenseeApi _licenseeApi;
        private readonly IFetchBetRecordDataTask _fetchBetRecordService;

        public HomeController(ILicenseeApi lincenseeApi, IFetchBetRecordDataTask fetchBetRecordService)
        {
            _licenseeApi = lincenseeApi;
            _fetchBetRecordService = fetchBetRecordService;
        }

        [HttpGet]
        [HttpGet("Index")]
        public Task<string> Index()
        {
            return Task.FromResult("Site is running in api");
        }

        [HttpGet("CheckConnection")]
        public async Task<string> CheckConnection()
        {
            return await _licenseeApi.CheckConnection();
        }

        [HttpGet("GetCustomers")]
        public async Task<IEnumerable<AffCustomerModel>> GetAffCustomerAsync(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize)
        {
            return await _licenseeApi.GetAffCustomerAsync(fromDate, toDate, pageIndex, pageSize);
        }

        [HttpGet("GetBetRecord")]
        public async Task<IEnumerable<TransBetRecordInfo>> GetBetRecord(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize)
        {
            return await _licenseeApi.GetBetRecordAsync(fromDate, toDate, pageIndex, pageSize);
        }

        [HttpGet("PullBetRecord")]
        public async Task<string> PullBetRecord(DateTime fromDate, DateTime toDate)
        {
            await _fetchBetRecordService.FetchDataAsync(fromDate, toDate);
            return "Finished Pull";
        }
    }
}