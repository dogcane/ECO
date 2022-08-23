using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.UnitTests.Utils.Foos
{
    public class VersionableAggregateRootFoo : VersionableAggregateRoot<int>
    {
        public VersionableAggregateRootFoo() : base()
        {
        }

        public VersionableAggregateRootFoo(int identity, int version) : base(identity, version)
        {
        }
    }
}
