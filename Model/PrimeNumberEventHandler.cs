using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AsyncMvvm.Model
{
    public delegate void PrimeNumberEventHandler(object sender, PrimeNumberEventArgs e);

    public class PrimeNumberEventArgs
    {
        public PrimeNumberEventArgs(long number) 
        {
            this.Number = number;
        }

        public long Number { get; }
    }
}
