using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Affiliate.N88.Comparer
{
    public class StringToIntComparer : IComparer<string>
    {
        public int Compare(string? x, string? y)
        {
            var xInt = Convert.ToInt32(x);
            var yInt = Convert.ToInt32(y);
            return xInt - yInt;
        }
    }
}
