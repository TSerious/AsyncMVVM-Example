using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMvvm.Model
{
    internal static class Tabs
    {
        public static string Add(int i)
        {
            StringBuilder sb = new (i);
            for(int j = 0; j < i; j++)
            {
                sb.Append("\t");
            }

            return sb.ToString();
        }

    }
}
