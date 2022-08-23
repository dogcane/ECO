using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.UnitTests.Utils.Foos
{
    public class AggregateRootFoo : AggregateRoot<int>
    {
        public AggregateRootFoo()
        {
        }

        public AggregateRootFoo(int identity) : base(identity)
        {
        }
    }
}
