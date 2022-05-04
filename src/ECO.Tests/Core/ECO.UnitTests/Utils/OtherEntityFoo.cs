using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.UnitTests.Utils
{
    public class OtherEntityFoo : Entity<string>
    {
        public OtherEntityFoo()
        {
        }

        public OtherEntityFoo(string identity) : base(identity)
        {
        }
    }
}
