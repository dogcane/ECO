using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.UnitTests.Utils
{
    public class EntityFoo : Entity<int>
    {
        public EntityFoo()
        {
        }

        public EntityFoo(int identity) : base(identity)
        {
        }
    }

}
