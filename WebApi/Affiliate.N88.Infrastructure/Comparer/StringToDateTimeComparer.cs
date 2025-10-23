namespace Affiliate.N88.Comparer
{
    public class StringToDateTimeComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            var xDateTime = Convert.ToDateTime(x);
            var yDateTime = Convert.ToDateTime(y);
            var subVal = (xDateTime - yDateTime).Ticks;

            if (subVal > 0)
            {
                return 1;
            }    
            else
            {
                return subVal == 0 ? 0 : -1;
            }    
        }
    }
}
