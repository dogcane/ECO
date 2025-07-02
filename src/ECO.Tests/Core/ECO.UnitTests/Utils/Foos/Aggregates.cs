using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.UnitTests.Utils.Foos
{
    public class AggregateRootFooOfInt : AggregateRoot<int>
    {
        public AggregateRootFooOfInt()
        {
        }

        public AggregateRootFooOfInt(int identity) : base(identity)
        {
        }
    }

    public class VersionableAggregateRootFooOfInt : VersionableAggregateRoot<int>
    {
        public VersionableAggregateRootFooOfInt() : base()
        {
        }

        public VersionableAggregateRootFooOfInt(int identity, int version) : base(identity, version)
        {
        }
    }
}
