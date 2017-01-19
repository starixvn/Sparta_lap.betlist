namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    using System.Collections.Generic;
    using Interfaces;
    using Model.Dashboard.DTO;
    using Model.Dashboard.MetaModel;
    using NUnit.Framework;

    [TestFixture]
    public class TurnoverByPlatformDataAccessTest
    {
        private ITurnoverByPlatformDataAccess _turnoverByPlatformDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _turnoverByPlatformDataAccess = new TurnoverByPlatformDataAccess();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _turnoverByPlatformDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new List<Platform>
            {
                new Platform { Name = "Asia", Value = 100M },
                new Platform { Name = "China", Value = 300M },
                new Platform { Name = "WAP", Value = 800M },
                new Platform { Name = "Smartsite", Value = 700M },
                new Platform { Name = "Others", Value = 1700M }
            };
            var metaModel = new DashboardBaseModel();

            // Action
            var result = _turnoverByPlatformDataAccess.GetData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(5));
                Assert.That(result[0].Name, Is.EqualTo(expected[0].Name));
                Assert.That(result[0].Value, Is.EqualTo(expected[0].Value));
                Assert.That(result[2].Name, Is.EqualTo(expected[2].Name));
                Assert.That(result[2].Value, Is.EqualTo(expected[2].Value));
                Assert.That(result[4].Name, Is.EqualTo(expected[4].Name));
                Assert.That(result[4].Value, Is.EqualTo(expected[4].Value));
            });
        }
    }
}