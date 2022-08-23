using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.UnitTests.Utils.Foos
{
    public class VersionableEntityFoo : VersionableEntity<int>
    {
        public VersionableEntityFoo() : base()
        {
        }

        public VersionableEntityFoo(int identity, int version) : base(identity, version)
        {
        }
    }
}
