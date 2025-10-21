using Affiliate.N88.WebApi.DTO;
using Affiliate.N88.SpParams.Criteria;
using Fanex.Data.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Affiliate.N88.WebApi.Repository
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly IDynamicRepository _dynamicRepository;

        public PromotionRepository(IDynamicRepository dynamicRepository)
        {
            _dynamicRepository = dynamicRepository ?? throw new ArgumentNullException(nameof(dynamicRepository));
        }

        public async Task<IEnumerable<Promotion>> GetPromotionsAsync(DateTime fromDate, DateTime toDate)
        {
            var criteria = new GetPromotionCriteria(fromDate, toDate);
            return await _dynamicRepository.FetchAsync<Promotion>(criteria);
        }
    }
}