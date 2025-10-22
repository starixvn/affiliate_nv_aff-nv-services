using System.Text;
using Affiliate.N88.Models;
using Affiliate.N88.Params;
using Affiliate.N88.SpParams.Commands;
using Affiliate.N88.SpParams.Criteria;
using Affiliate.N88.WebApi.Models;
using Fanex.Data.Repository;
using Fanex.Logging;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Affiliate.N88.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private const int _Marketing_Cust_ID = -1;
        private readonly IDynamicRepository dynamicRepository;

        public CustomerController(IDynamicRepository dynamicRepository)
        {
            this.dynamicRepository = dynamicRepository;
        }

        [HttpGet("GetAffCustomers")]
        public async Task<ActionResult<IEnumerable<AffCustomerModel>>> GetAffCustomers(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize = 2000)
        {
            return Ok(await dynamicRepository.FetchAsync<AffCustomerModel>(new GetAffCustomersCriteria(fromDate, toDate, pageIndex, pageSize)));
        }

        [HttpGet("GetCustomers")]
        public async Task<ActionResult<IEnumerable<CustomerInfo>>> GetCustomers(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize = 2000)
            => Ok(await dynamicRepository.FetchAsync<CustomerInfo>(new GetCustomersCriteria(fromDate, toDate, pageIndex, pageSize)));

        [HttpGet("GetFixedCustomers")]
        public async Task<ActionResult<IEnumerable<CustomerInfo>>> GetFixedCustomers()
        {
            var result = await dynamicRepository.FetchAsync<CustomerInfo>(new GetFixedCustomerCriteria());
            return Ok(result);
        }

        [HttpGet("GetCustWallets")]
        public async Task<ActionResult<IEnumerable<CustWalletInfo>>> GetCustWallets(DateTime fromDate)
            => Ok(await dynamicRepository.FetchAsync<CustWalletInfo>(new GetCustWalletCriteria(fromDate)));

        [HttpGet("GetLoginCustomerInfo")]
        public async Task<ActionResult<CustomerInfo>> GetLoginCustomerInfo(string userName, int siteId)
        {
            CustomerInfo? cust = null;
            if (!HasNonASCIIChars(userName))
            {
                cust = await dynamicRepository.GetAsync<CustomerInfo>(new GetLoginCustInfoCriteria() { UserName = userName, SiteId = siteId });
            }
            return Ok(cust);
        }

        [HttpGet("GetCustomerRegInfo")]
        public async Task<ActionResult<IEnumerable<CustRegModel>>> GetCustomerRegInfo(long unixTime)
        {
            var infos = await dynamicRepository.FetchAsync<CustRegInfo>(new GetCustRegInfoCriteria(unixTime));
            return Ok(infos.Select(i => new CustRegModel
            {
                CustId = i.CustId,
                RegTime = DateTimeOffset.Parse(i.RegTime ?? string.Empty),
                SignUpUrl = i.SignUpUrl,
                SiteId = i.SiteId
            }));
        }

        [HttpPost]
        public async Task<ActionResult<decimal>> UpdateCustomerWallet(PaymentRequestInfo payment)
        {
            await LogService.InfoAsync($"Recevice update wallet request\n" +
                $"Param: {JsonConvert.SerializeObject(payment)}\n" +
                $"RequestIp: ${GetIpAddress()}");
            var criteria = new UpdateCustomerWalletCommand
            {
                Amount = payment.Amount,
                AfterAmount = 0,
                CustId = payment.CustId,
                Memo = payment.Memo ?? string.Empty,
                RefTransId = payment.RefTransId ?? string.Empty,
                SiteId = payment.SiteId,
                Type = payment.Type,
                UserName = payment.UserName ?? string.Empty
            };

            await dynamicRepository.ExecuteAsync(criteria);
            await LogService.InfoAsync($"AfterAmount: {criteria.AfterAmount}");
            return Ok(criteria.AfterAmount);
        }

        [HttpGet("GetCustBonusRollOvers")]
        public async Task<ActionResult<IEnumerable<CustBonusRolloverModel>>> GetCustBonusRollOvers(int id, int batchCount = 2000)
            => Ok(await dynamicRepository.FetchAsync<CustBonusRolloverModel>(new GetCustBonusRolloverCriteria(id, batchCount)));

        [HttpGet("GetCustClosingBalance")]
        public async Task<ActionResult<IEnumerable<CustClosingBalanceModel>>> GetCustClosingBalance(DateTime lastGetTime, int pageIndex, int pageSize = 2000)
            => Ok(await dynamicRepository.FetchAsync<CustClosingBalanceModel>(new GetCustClosingBalanceCriteria(lastGetTime, pageIndex, pageSize)));

        [HttpPost]
        public async Task<ActionResult<ResultInfo>> UnlockUser(int[] userIds)
        {
            try
            {
                var json = BuildJson(userIds);
                await dynamicRepository.ExecuteAsync(new UnlockUsersCommand { CustIdList = json });
                return Ok(new ResultInfo
                {
                    IsSuccess = true
                });
            }
            catch (Exception ex)
            {
                return Ok(new ResultInfo
                {
                    IsSuccess = false,
                    ErrMsg = ex.Message
                });
            }
        }

        [HttpPost]
        public async Task<ActionResult<ResultInfo>> InsertAffiliateInfo(AffiliateInfoParam affiliate)
        {
            try
            {
                await dynamicRepository.ExecuteAsync(new InsertAffiliateInfoCommand
                {
                    AffiliateId = affiliate.AffiliateId,
                    AffiliateCode = affiliate.AffiliateCode ?? string.Empty,
                    AffiliateName = affiliate.AffiliateName ?? string.Empty,
                    CustId = affiliate.CustId == 0 ? _Marketing_Cust_ID : affiliate.CustId,
                    ParentAffiliate = affiliate.ParentAffiliate ?? string.Empty,
                    SiteId = affiliate.SiteId,
                    UserName = affiliate.UserName ?? string.Empty,
                    FirstName = affiliate.FirstName ?? string.Empty,
                    LastName = affiliate.LastName ?? string.Empty,
                    ContactNumber = affiliate.ContactNumber ?? string.Empty,
                    Email = affiliate.Email ?? string.Empty,
                    Status = affiliate.Status ?? string.Empty
                });
                return Ok(new ResultInfo { IsSuccess = true });
            }
            catch (Exception ex)
            {
                return Ok(new ResultInfo
                {
                    IsSuccess = false,
                    ErrMsg = ex.Message
                });
            }
        }

        [HttpGet("GetCustBalance")]
        public async Task<ActionResult<IEnumerable<CustBalanceInfo>>> GetCustBalance([FromQuery]int[] ids)
        {
            var json = BuildJson(ids);
            return Ok(await dynamicRepository.FetchAsync<CustBalanceInfo>(new GetCustBalanceCriteria(json)));
        }

        [HttpGet("GetVipUpgradeReviews")]
        public async Task<ActionResult<IEnumerable<VipUpgradeReviewInfo>>> GetVipUpgradeReviews(DateTime monthDate, int pageIndex, int pageSize)
        {
            var txtMonthDate = monthDate.ToString("yyyyMM");
            return Ok(await dynamicRepository.FetchAsync<VipUpgradeReviewInfo>(new GetVipUpgradeReviewCriteria
            {
                MonthDate = txtMonthDate,
                PageIndex = pageIndex,
                PageSize = pageSize
            }));
        }

        [HttpPost]
        public async Task<ActionResult<ResultInfo>> TransferMoneyToPlayer(TransferMoneyParam param)
        {
            await LogService.InfoAsync($"Receive transfer money request\n" +
                $"Param: {JsonConvert.SerializeObject(param)}\n" +
                $"RequestIp: ${GetIpAddress()}");

            var command = new TransferMoneyToPlayerCommand
            {
                SiteId = param.SiteId,
                ErrCode = 0,
                FromAmount = param.FromAmount,
                FromCustId = param.FromCustId,
                MaxTransferAmount = param.MaxTransferAmount,
                RefTransId = param.RefTransId,
                ToAmount = param.ToAmount,
                ToCustId = param.ToCustId,
            };
            try
            {
                await dynamicRepository.ExecuteAsync(command);
            }
            catch (Exception ex)
            {
                await LogService.ErrorAsync($"{ex.Message}");
                throw;
            }

            await LogService.InfoAsync($"ErrCode: {command.ErrCode}");
            return Ok(new ResultInfo
            {
                ErrMsg = command.ErrCode.ToString(),
                IsSuccess = command.ErrCode == 0
            });
        }

        [HttpGet("GetCustVerificationRewards")]
        public async Task<ActionResult<IEnumerable<CustVerificationReward>>> GetCustVerificationRewards(DateTime lastGetTime, string conditionField, int batchCount = 2000)
            => Ok(await dynamicRepository.FetchAsync<CustVerificationReward>(new GetCustVerificationRewardCriteria(lastGetTime, conditionField, batchCount)));

        private static string BuildJson(int[] userIds)
            => $"[{string.Join(",", userIds)}]";

        private string GetIpAddress()
        {
            var httpContext = HttpContext;
            string? ipAddress = httpContext.Request.Headers["X-Forwarded-For"].FirstOrDefault();

            if (!string.IsNullOrEmpty(ipAddress))
            {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0)
                {
                    return addresses[0];
                }
            }

            return httpContext.Connection.RemoteIpAddress?.ToString() ?? "Unknown";
        }

        private static bool HasNonASCIIChars(string str)
            => Encoding.UTF8.GetByteCount(str) != str.Length;
    }
}