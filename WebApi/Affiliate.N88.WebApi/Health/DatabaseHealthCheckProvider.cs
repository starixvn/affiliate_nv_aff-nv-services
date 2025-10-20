using Affiliate.N88.DTO;
using Affiliate.N88.SpParams.Criteria;
using Affiliate.N88.WebApi.Extensions;
using Affiliate.N88.WebApi.Infastructure.Enums;
using Affiliate.N88.WebApi.Models.HelthCheck;
using Fanex.Data.Repository;
using System;
using System.Threading.Tasks;

namespace Affiliate.N88.WebApi.Health
{
    public class DatabaseHealthCheckProvider : IHealthCheckProvider
    {
        private DatabaseServerType DatabaseServerType { get; }
        private IDynamicRepository Repo { get; }

        public int SortOrder { get; }


        public DatabaseHealthCheckProvider(IDynamicRepository repo, DatabaseServerType databaseServerType, int sortOrder)
        {
            Repo = repo;
            DatabaseServerType = databaseServerType;
            SortOrder = sortOrder;
        }

        public async Task<HealthCheckItemResult> GetHealthCheckAsync()
        {
            var dataBaseServerName = DatabaseServerType.ToString();
            var criteria = new GetServerInfoCriteria(dataBaseServerName);
            var result = new HealthCheckItemResult($"{nameof(DatabaseHealthCheckProvider)}"
                , SortOrder
                , "Checks the database"
                , $"Checks whether the {dataBaseServerName} database can be accessed.");
            try
            {
                await Repo.FetchAsync<AffCustomerInfo>(criteria);
                result.HealthState = HealthState.Healthy;
                result.Messages.Add($"Connect database {dataBaseServerName} success");
            }
            catch 
            {
                result.HealthState = HealthState.Unhealthy;
                result.Messages.Add($"Connect database {dataBaseServerName} fail");
            }
            return result;
        }
    }
}