using ECO.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECO.UnitTests.Utils.Mocks
{
    public class PersistenceUnitMock : Mock<IPersistenceUnit>
    {
        public PersistenceContextMock PersistenceContextMock { get; init; } = new PersistenceContextMock();

        public PersistenceUnitMock()
        {
            Setup(m => m.Name).Returns("Persistence Unit Mock");
            Setup(m => m.CreateContext()).Returns(PersistenceContextMock.Object);
        }
    }
}
