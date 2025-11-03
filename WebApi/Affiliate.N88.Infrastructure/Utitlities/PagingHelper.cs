namespace Affiliate.N88.Utitlities
{
    public static class PagingHelper
    {
        public static IEnumerable<IEnumerable<T>> PagingData<T>(IEnumerable<T> datas, int max)
        {
            var result = new List<IEnumerable<T>>();
            result.Add(datas.Take(max));
            var remain = datas.Skip(max);
            while (remain.Any())
            {
                result.Add(remain.Take(max));
                remain = remain.Skip(max);
            }
            return result;
        }
    }
}
