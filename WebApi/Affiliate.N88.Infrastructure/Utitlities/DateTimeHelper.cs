namespace Affiliate.N88.Utitlities
{
    public static class DateTimeHelper
    {
        public static IEnumerable<(DateTime FromDate, DateTime ToDate)> GetProportionTimeRange(DateTime startTime, DateTime endTime, TimeSpan span, bool isExcludeNotFullRangeSpan = true)
        {
            if (span == TimeSpan.Zero)
            {
                throw new ArgumentException("Zero time span, this make a infinite repeation to get timerange.");
            }
            var startRequestTime = startTime;
            var endRequestTime = GetSmallerTime(startRequestTime.Add(span), endTime);
            while (startRequestTime < endRequestTime)
            {
                if (!isExcludeNotFullRangeSpan || (endRequestTime - startRequestTime) == span)
                {
                    yield return (startRequestTime, endRequestTime);
                }
                startRequestTime = endRequestTime;
                endRequestTime = GetSmallerTime(endRequestTime.Add(span), endTime);
            }
        }

        public static DateTime GetSmallerTime(DateTime compareTime, DateTime endTime)
        {
            return DateTime.Compare(compareTime, endTime) > 0 ? endTime : compareTime;
        }

        public static DateTime GetApiServerDateTime()
            => DateTime.UtcNow;
    }
}
