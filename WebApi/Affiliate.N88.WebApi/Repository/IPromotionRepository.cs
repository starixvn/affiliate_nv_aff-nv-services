using Affiliate.N88.WebApi.DTO;

namespace Affiliate.N88.WebApi.Repository
{
    public interface IPromotionRepository
    {
        Task<IEnumerable<Promotion>> GetPromotionsAsync(DateTime fromDate, DateTime toDate);
    }
}