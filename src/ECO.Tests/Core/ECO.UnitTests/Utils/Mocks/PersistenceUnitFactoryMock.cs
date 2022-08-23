using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ECO.Data;
using Moq;

namespace ECO.UnitTests.Utils.Mocks
{
    public class PersistenceUnitFactoryMock : Mock<IPersistenceUnitFactory>
    {
        public PersistenceUnitMock PersistenceUnitMock { get; init; } = new PersistenceUnitMock();

        public PersistenceUnitFactoryMock()
        {
            Setup(m => m.GetPersistenceUnit(It.IsAny<Type>())).Returns(PersistenceUnitMock.Object);
        }
    }
}
