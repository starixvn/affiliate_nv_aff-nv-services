using Affiliate.N88.Jobs.Configurations;
using Affiliate.N88.Jobs.Constants;
using Affiliate.N88.Jobs.RecurringTasks;
using Affiliate.N88.Jobs.RecurringTasks.Log;
using Affiliate.N88.Jobs.RecurringTasks.Product9W;
using Affiliate.N88.Jobs.RecurringTasks.Promotion;
using Affiliate.N88.Jobs.Services;
using Affiliate.N88.Repository;
using Affiliate.N88.Services;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Affiliate.N88.Jobs.Filter;
using Affiliate.N88.Jobs.RecurringTasks.Bonus;
using Affiliate.N88.Jobs.RecurringTasks.RetryJob;
using Affiliate.N88.Jobs.RecurringTasks.AffCustomer;
using Affiliate.N88.Jobs.RecurringTasks.Customer;
using Affiliate.N88.Jobs.RecurringTasks.Deposit;
using Hangfire.Redis.StackExchange;
using StackExchange.Redis;
using System.Net;
using Fanex.Logging;

namespace Affiliate.N88.Jobs.Middlewares
{
    public static class HangfireMiddleware
    {
        private const string AppSettingsRoot = "AppSettings";

        public static void RegisterHangfire(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddOptions();
            services.AddSingleton<GlobalHangfireJobExceptionFilterAttribute>();
            services.AddSingleton<UMHangfireFilterAttribute>();

            var redisConfig = configuration.GetSection($"Redis").Get<RedisSetting>();
            var configString = $"{redisConfig.Server}:{redisConfig.Port},password={redisConfig.Pass},keepAlive={180},abortConnect={false},defaultDatabase={redisConfig.Database}";
            var redis = ConnectionMultiplexer.Connect(ConfigurationOptions.Parse(configString));

            services.AddHangfire((serviceProvider, config) =>
            {
                config.UseRedisStorage(redis, new RedisStorageOptions
                {
                    Prefix = redisConfig.AppName,
                    Db = redisConfig.Database
                }).WithJobExpirationTimeout(TimeSpan.FromHours(2));
                config.UseFilter(serviceProvider.GetService<GlobalHangfireJobExceptionFilterAttribute>());
                config.UseFilter(serviceProvider.GetService<UMHangfireFilterAttribute>());
            });

            const int SchedulePollingIntervalSeconds = 1;
            services.AddHangfireServer(options =>
            {
                options.SchedulePollingInterval = TimeSpan.FromSeconds(SchedulePollingIntervalSeconds);
            });

            var appSettings = new AppSettings();
            configuration.GetSection(AppSettingsRoot).Bind(appSettings);

            //FetchAffCustomerDataTask
            services.AddScoped<IAffMemberRepository, AffImportRepository>();
            services.Configure<LicenseeSiteSetting>(configuration.GetSection($"{AppSettingsRoot}:LicenseeSetting"));

            services.AddScoped<IFetchAffCustomerDataTask, FetchAffCustomerDataTask>().Configure<FetchServiceSetting>(
                ServiceConfigName.PULL_AFF_CUSTOMER_SETTING,
                configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_AFF_CUSTOMER_SETTING}"));

            services.AddScoped<IFetchRealAffCustomerDataTask, FetchRealAffCustomerDataTask>().Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_REAL_AFF_CUSTOMER_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_REAL_AFF_CUSTOMER_SETTING}"));

            //TimeTrackerService
            services.AddScoped<ITrackerRepository, TrackerRespository>();
            services.AddScoped<ITrackerService, TimeTrackerService>();
            services.Configure<TimeTrackerSetting>(configuration.GetSection($"{AppSettingsRoot}:TimeTrackerSetting"));

            //DataTrackerService
            services.AddScoped<IDataTrackerRepository, DataTrackerRepository>();
            services.AddScoped<IDataTrackerService, DataTrackerService>();

            //FetchBetRecordDataTask
            services.AddScoped<IThirdPartyImportRepository, ThirdPartyImportRepository>();
            services.AddScoped<IFetchBetRecordDataTask, FetchBetRecordDataTask>();
            services.Configure<FetchServiceSetting>(
                ServiceConfigName.PULL_BET_RECORD_SETTING,
                configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_BET_RECORD_SETTING}"));

            //FetchCustomerDataTask
            services.AddScoped<IFetchCustomerDataTask, FetchCustomerDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_CUSTOMER_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_CUSTOMER_SETTING}"));

            services.AddScoped<IFetchRealCustomerDataTask, FetchRealCustomerDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_REAL_CUSTOMER_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_REAL_CUSTOMER_SETTING}"));

            //FetchServerInfoDataTask
            services.AddScoped<IFetchServerInfoDataTask, FetchServerInfoDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_SERVER_INFO_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_SERVER_INFO_SETTING}"));

            //FetchTpGameTypeDataTask
            services.AddScoped<IFetchGameTypeDataTask, FetchGameTypeDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_GAME_TYPE_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_GAME_TYPE_SETTING}"));

            //FetchTpTransferInfoDataTask
            services.AddScoped<IFetchTpTransferInfoDataTask, FetchTpTransferInfoDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_TPTRANSFER_INFO_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_TPTRANSFER_INFO_SETTING}"));

            //FetchDepositInfoDataTask
            services.AddScoped<IFetchDepositInfoDataTask, FetchDepositInfoDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_DEPOSIT_INFO_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_DEPOSIT_INFO_SETTING}"));

            services.AddScoped<IFetchRealDepositInfoDataTask, FetchRealDepositInfoDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_REAL_DEPOSIT_INFO_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_REAL_DEPOSIT_INFO_SETTING}"));

            //FetchBonusInfoDataTask
            services.AddScoped<IFetchBonusInfoDataTask, FetchBonusInfoDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_BONUS_INFO_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_BONUS_INFO_SETTING}"));

            services.AddScoped<IFetchWeeklyBonusInfoDataTask, FetchWeeklyBonusInfoDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_WEEKLY_BONUS_INFO_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_WEEKLY_BONUS_INFO_SETTING}"));

            //FetchCustRegInfoDataTask
            services.AddScoped<IFetchCustRegInfoDataTask, FetchCustRegInfoDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_CUST_REG_INFO_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_CUST_REG_INFO_SETTING}"));

            //FetchWithdrawalInfoDataTask
            services.AddScoped<IFetchWithdrawalInfoDataTask, FetchWithdrawalInfoDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_WITHDRAWAL_INFO,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_WITHDRAWAL_INFO}")
                );

            //FetchPromotionDataTask
            services.AddScoped<IFetchPromotionDataTask, FetchPromotionDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_PROMOTION_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_PROMOTION_SETTING}")
                );

            //FetchPromotionDataTask
            services.AddScoped<IFetchPromotionSpecificDataTask, FetchPromotionSpecificDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_PROMOTIONSPECIFIC_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_PROMOTIONSPECIFIC_SETTING}")
                );

            //FetchRebateMLogDataTask
            services.AddScoped<IFetchRebateMLogDataTask, FetchRebateMLogDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_REBATEMLOG_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_REBATEMLOG_SETTING}")
                );

            //FetchCustBonusRollOver
            services.AddScoped<IFetchCustBonusRolloverDataTask, FetchCustBonusRolloverDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_CUST_BONUS_ROLLOVER_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_CUST_BONUS_ROLLOVER_SETTING}")
                );

            //FetchCustClosingBalance
            services.AddScoped<IFetchCustClosingBalanceDataTask, FetchCustClosingBalanceDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_CUST_CLOSING_BALANCE_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_CUST_CLOSING_BALANCE_SETTING}")
                );

            services.AddScoped<IFetchAdjustmentInfoDataTask, FetchAdjustmentInfoDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_ADJUSTMENT_INFO_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_ADJUSTMENT_INFO_SETTING}")
                );

            services.AddScoped<IFetchDrawLogDataTask, FetchDrawLogDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_DRAW_LOG_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_DRAW_LOG_SETTING}")
                );

            services.AddScoped<IFetchEventDrawDataTask, FetchEventDrawDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_EVENT_DRAW_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_EVENT_DRAW_SETTING}")
                );

            services.AddScoped<IFetchVipUpgradeReviewDataTask, FetchVipUpgradeReviewDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_VIP_UPGRADE_REVIEW_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_VIP_UPGRADE_REVIEW_SETTING}")
                );

            services.AddScoped<IFetchLogAdminAdjustWalletDataTask, FetchLogAdminAdjustWalletDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_LOG_ADMIN_ADJUST_WALLET_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_LOG_ADMIN_ADJUST_WALLET_SETTING}")
                );

            services.AddScoped<IFetchCustVerificationRewardDataTask, FetchCustVerificationRewardDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_CUST_VERIFICATION_REWARD_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_CUST_VERIFICATION_REWARD_SETTING}")
                );

            services.AddScoped<IFetchReferralMLogDataTask, FetchReferralMLogDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_REFERRALMLOG_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_REFERRALMLOG_SETTING}")
                );

            services.AddScoped<IFetchReferralLogDataTask, FetchReferralLogDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_REFERRALLOG_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_REFERRALLOG_SETTING}")
                );

            services.AddScoped<IFetchTpReport9WDataTask, FetchTpReport9WDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_TPREPORT_9W_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_TPREPORT_9W_SETTING}")
                );

            services.AddScoped<IFetchTpWager9WDataTask, FetchTpWager9WDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.PULL_TPWAGER_9W_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.PULL_TPWAGER_9W_SETTING}")
                );

            services.AddScoped<IReInsertAffiliateInfoDataTask, ReInsertAffiliateInfoDataTask>()
                .Configure<FetchServiceSetting>(
                    ServiceConfigName.INSERT_AFFILIATE_SETTING,
                    configuration.GetSection($"{AppSettingsRoot}:{ServiceConfigName.INSERT_AFFILIATE_SETTING}")
                );
        }

        public static void UseHangfire(this IApplicationBuilder app, AppSettings appSettings)
        {
            app.UseHangfireDashboard("/jobs", new DashboardOptions
            {
                DisplayStorageConnectionString = false,
                IgnoreAntiforgeryToken = true,
                Authorization = new[] { new AllowedAnonymousAuthorizationFilter() }
            });
            Logger.Log.Info($"AFF NV CronJobs on {Dns.GetHostName()}");

            if (appSettings.IsRunFetchTask)
            {
                Logger.Log.Info($"AFF NV CronJobs are registered on {Dns.GetHostName()}");

                RegisterFetchDataTask(appSettings);
                RegisterRetryDataTask(appSettings);
            }
        }

        private static void RegisterFetchDataTask(AppSettings appSettings)
        {
            RegisterTask<IFetchRealAffCustomerDataTask>("Fetch Aff Customer Data-Real", appSettings.PullRealAffCustomerSetting.PullInfoCron, job => job.FetchDataAsync());
            RegisterTask<IFetchAffCustomerDataTask>("Fetch Aff Customer Data-Normal", appSettings.PullAffCustomerSetting.PullInfoCron, job => job.FetchDataAsync());
            RegisterTask<IFetchAffCustomerDataTask>("Fetch Aff Customer Data-DATE_RANGE", Cron.Never(), job => job.FetchDataByDateRangeTask());

            RegisterTask<IFetchBetRecordDataTask>($"{typeof(IFetchBetRecordDataTask).Name}", appSettings.PullBetRecordSetting.PullInfoCron, job => job.FetchDataAsync());
            RegisterTask<IFetchBetRecordDataTask>($"{typeof(IFetchBetRecordDataTask).Name}-2HourInterval", "30 0/2 ? * *", job => job.FetchAllDay(2));
            RegisterTask<IFetchBetRecordDataTask>($"{typeof(IFetchBetRecordDataTask).Name}-Daily", Cron.Daily(7), job => job.FetchFromAdjusmentDayAsync(3));
            RegisterTask<IFetchBetRecordDataTask>($"{typeof(IFetchBetRecordDataTask).Name}-Monthly", Cron.Monthly(4, 12, 0), job => job.FetchLastMonthAsync());

            RegisterTask<IFetchRealCustomerDataTask>("Fetch Customer Data-Real", appSettings.PullRealCustomerSetting.PullInfoCron, job => job.FetchDataAsync());
            RegisterTask<IFetchCustomerDataTask>("Fetch Customer Data-Normal", appSettings.PullCustomerSetting.PullInfoCron, job => job.FetchDataAsync());
            RegisterTask<IFetchCustomerDataTask>("Fetch Customer Data-Daily", Cron.Daily(5), job => job.FetchDataDailyTask());

            RegisterTask<IFetchServerInfoDataTask>($"{typeof(IFetchServerInfoDataTask).Name}", appSettings.PullServerInfoSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchGameTypeDataTask>($"{typeof(IFetchGameTypeDataTask).Name}", appSettings.PullGameTypeSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchTpTransferInfoDataTask>($"{typeof(IFetchTpTransferInfoDataTask).Name}", appSettings.PullTpTransferInfoSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchRealDepositInfoDataTask>("Fetch Deposit Data-Real", appSettings.PullRealDepositInfoSetting.PullInfoCron, job => job.FetchDataAsync());
            RegisterTask<IFetchDepositInfoDataTask>("Fetch Deposit Data-Normal", appSettings.PullDepositInfoSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchCustRegInfoDataTask>($"{typeof(IFetchCustRegInfoDataTask).Name}", appSettings.PullCustRegInfoSetting.PullInfoCron, job => job.FetchDataAsync());

            //Bonus Info
            RegisterTask<IFetchBonusInfoDataTask>($"{typeof(IFetchBonusInfoDataTask).Name}", appSettings.PullBonusInfoSetting.PullInfoCron, job => job.FetchDataAsync());
            RegisterTask<IFetchWeeklyBonusInfoDataTask>($"{typeof(IFetchWeeklyBonusInfoDataTask).Name}", appSettings.PullWeeklyBonusInfoSetting.PullInfoCron, job => job.FetchDataAsync());


            RegisterTask<IFetchWithdrawalInfoDataTask>($"{typeof(IFetchWithdrawalInfoDataTask).Name}", appSettings.PullWithdrawalInfoSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchPromotionDataTask>($"{typeof(IFetchPromotionDataTask).Name}", appSettings.PullPromotionSetting.PullInfoCron, job => job.FetchDataAsync());
            RegisterTask<IFetchPromotionDataTask>($"{typeof(IFetchPromotionDataTask).Name}-Daily", Cron.Daily(6), job => job.FetchDataDailyTask());

            RegisterTask<IFetchPromotionSpecificDataTask>($"{typeof(IFetchPromotionSpecificDataTask).Name}", appSettings.PullPromotionSpecificSetting.PullInfoCron, job => job.FetchDataAsync());
            RegisterTask<IFetchPromotionSpecificDataTask>($"{typeof(IFetchPromotionSpecificDataTask).Name}-Daily", Cron.Daily(6), job => job.FetchDataDailyTask());

            RegisterTask<IFetchRebateMLogDataTask>($"{typeof(IFetchRebateMLogDataTask).Name}", appSettings.PullRebateMLogSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchCustBonusRolloverDataTask>($"{typeof(IFetchCustBonusRolloverDataTask).Name}", appSettings.PullCustBonusRolloverSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchCustClosingBalanceDataTask>($"{typeof(IFetchCustClosingBalanceDataTask).Name}", appSettings.PullCustClosingBalanceSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchAdjustmentInfoDataTask>($"{typeof(IFetchAdjustmentInfoDataTask).Name}", appSettings.PullAdjustmentInfoSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchDrawLogDataTask>($"{typeof(IFetchDrawLogDataTask).Name}", appSettings.PullDrawLogSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchEventDrawDataTask>($"{typeof(IFetchEventDrawDataTask).Name}", appSettings.PullEventDrawSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchVipUpgradeReviewDataTask>($"{typeof(IFetchVipUpgradeReviewDataTask).Name}", appSettings.PullVipUpgradeReviewSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchVipUpgradeReviewDataTask>($"{typeof(IFetchVipUpgradeReviewDataTask).Name}-Monthly", Cron.Monthly(7, 2, 0), job => job.FetchLastMonthDataAsync());

            RegisterTask<IFetchLogAdminAdjustWalletDataTask>($"{typeof(IFetchLogAdminAdjustWalletDataTask).Name}", 
                appSettings.PullLogAdminAdjustWalletSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchCustVerificationRewardDataTask>($"{typeof(IFetchCustVerificationRewardDataTask).Name}", 
                appSettings.PullCustVerificationRewardSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchReferralMLogDataTask>($"{typeof(IFetchReferralMLogDataTask).Name}", appSettings.PullReferralMLogSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchReferralLogDataTask>($"{typeof(IFetchReferralLogDataTask).Name}", appSettings.PullReferralLogSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchTpReport9WDataTask>($"{typeof(IFetchTpReport9WDataTask).Name}", appSettings.PullTpReport9WSetting.PullInfoCron, job => job.FetchDataAsync());

            RegisterTask<IFetchTpWager9WDataTask>($"{typeof(IFetchTpWager9WDataTask).Name}", appSettings.PullTpWager9WSetting.PullInfoCron, job => job.FetchDataAsync());
        }

        private static void RegisterRetryDataTask(AppSettings appSettings)
        {
            if (appSettings.IsApiDataModifiable)
            {
                RegisterTask<IReInsertAffiliateInfoDataTask>($"{typeof(IReInsertAffiliateInfoDataTask).Name}", appSettings.InsertAffiliateSetting.PullInfoCron, job => job.HandleAsync());
            }
        }

        private static void RegisterTask<T>(string serviceId, string cronExpression, Expression<Func<T, Task>> methodCall)
        {
            if (!string.IsNullOrWhiteSpace(cronExpression))
            {
                RecurringJob.AddOrUpdate<T>(
                            serviceId,
                            methodCall,
                            cronExpression);
            }
        }
    }

    public class AllowedAnonymousAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            return true;
        }
    }
}