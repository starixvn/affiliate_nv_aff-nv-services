using Affiliate.N88.Models;
using Affiliate.N88.Params;
using Refit;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.N88.Jobs.Apis
{
    public interface ILicenseeApi
    {
        [Get("/Home/Index")]
        Task<string> CheckConnection();

        [Get("/customer/GetAffCustomers")]
        Task<IEnumerable<AffCustomerModel>> GetAffCustomerAsync(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize);

        [Get("/customer/GetCustomers")]
        Task<IEnumerable<CustomerInfo>> GetCustomersAsync(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize);

        [Get("/customer/GetFixedCustomers")]
        Task<IEnumerable<CustomerInfo>> GetFixedCustomers();

        [Get("/BetReport/GetBetRecord")]
        Task<IEnumerable<TransBetRecordInfo>> GetBetRecordAsync(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize);

        [Get("/setting/GetServerInfo")]
        Task<IEnumerable<ServerInfo>> GetServerInfoAsync();

        [Get("/setting/GetTpGameType")]
        Task<IEnumerable<TpGameTypeInfo>> GetGameTypeAsync();

        [Get("/customer/GetCustWallets")]
        Task<IEnumerable<CustWalletInfo>> GetCustomerWalletAsync(DateTime fromDate);

        [Post("/customer/InsertAffiliateInfo")]
        Task<ResultInfo> InsertAffiliateInfo(AffiliateInfoParam affiliateInfo);

        [Get("/BetReport/GetTpTransferInfo")]
        Task<IEnumerable<TpTransferInfo>> GetTpTransferInfo(int id, int totalRow);

        [Get("/betreport/GetDepositInfo")]
        Task<IEnumerable<DepositInfo>> GetDepositInfo(DateTime fromDate, DateTime toDate, int pageIndex, int pageSize);

        [Get("/Customer/GetCustomerRegInfo")]
        Task<IEnumerable<CustRegModel>> GetCustomerRegInfo(long unixTime);

        [Get("/BetReport/GetBonusInfo")]
        Task<IEnumerable<BonusDTO>> GetBonusInfo(int id, int batchCount);

        [Get("/BetReport/GetBonusInfoByDate")]
        Task<IEnumerable<BonusDTO>> GetBonusInfoByDate(DateTime calledTime, int pageIndex, int pageSize);

        [Get("/BetReport/GetWithdrawalInfo")]
        Task<IEnumerable<Withdrawal>> GetWithdrawalInfo(DateTime updateTime, int batchCount);

        [Get("/Customer/GetCustBonusRollovers")]
        Task<IEnumerable<CustBonusRolloverModel>> GetCustBonusRolloversAsync(int id, int batchCount);

        [Get("/LogReport/GetRebateMLogs")]
        Task<IEnumerable<RebateMLogModel>> GetRebateMLogs(DateTime fromDate, DateTime toDate);

        [Get("/customer/GetCustClosingBalance")]
        Task<IEnumerable<CustClosingBalanceModel>> GetCustClosingBalance(DateTime lastGetTime, int pageIndex, int pageSize);

        [Get("/customer/GetCustVerificationRewards")]
        Task<IEnumerable<CustVerificationReward>> GetCustVerificationRewards(DateTime lastGetTime, string conditionField, int batchCount);

        [Get("/BetReport/GetAdjustmentInfos")]
        Task<IEnumerable<AdjustmentInfo>> GetAdjustmentInfos(int id, int batchCount);

        [Get("/LogReport/GetDrawLogs")]
        Task<IEnumerable<DrawLogInfo>> GetDrawLogs(int id, int batchCount);

        [Get("/LogReport/GetEventDraws")]
        Task<IEnumerable<EventDrawInfo>> GetEventDraws(int id, int batchCount);

        [Get("/Customer/GetVipUpgradeReviews")]
        Task<IEnumerable<VipUpgradeReviewInfo>> GetVipUpgradeReviews(DateTime monthDate, int pageIndex, int pageSize);

        [Get("/LogReport/GetLogAdminAdjustWallet")]
        Task<IEnumerable<LogAdminAdjustWalletInfo>> GetLogAdminAdjustWallet(int id, int batchCount);

        [Get("/LogReport/GetReferralMLogs")]
        Task<IEnumerable<ReferralMLogModel>> GetReferralMLogs(int id, int batchCount);

        [Get("/LogReport/GetReferralLogs")]
        Task<IEnumerable<ReferralLogModel>> GetReferralLogs(int id, int batchCount);
    }
}