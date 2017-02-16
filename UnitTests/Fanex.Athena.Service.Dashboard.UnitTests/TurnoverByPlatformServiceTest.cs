namespace Fanex.Athena.Service.Dashboard.UnitTests
{
    using System.Collections.Generic;
    using DataAccess.Dashboard.Interfaces;
    using Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class TurnoverByPlatformServiceTest
    {
        private IWidgetByPlatformDataAccess _turnoverByPlatformDataAccess;
        private IWidgetByPlatformService _turnoverByPlatformService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _turnoverByPlatformDataAccess = Substitute.For<IWidgetByPlatformDataAccess>();
            _turnoverByPlatformService = new TurnoverByPlatformService(_turnoverByPlatformDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _turnoverByPlatformDataAccess = null;
            _turnoverByPlatformService = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new WidgetByPlatformViewModel
            {
                Data = new List<KeyValuePair<string, decimal>>
                {
                    new KeyValuePair<string, decimal>("Asia", 100M),
                    new KeyValuePair<string, decimal>("China", 300M),
                    new KeyValuePair<string, decimal>("WAP", 800M),
                    new KeyValuePair<string, decimal>("Smartsite", 700M),
                    new KeyValuePair<string, decimal>("Others", 1700M)
                }
            };

            var metaModel = new DashboardBaseModel();
            _turnoverByPlatformDataAccess.GetData(metaModel).Returns(expected);

            // Action
            var result = _turnoverByPlatformService.GetData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(5));
                Assert.That(result.Data[0].Key, Is.EqualTo(expected.Data[0].Key));
                Assert.That(result.Data[0].Value, Is.EqualTo(expected.Data[0].Value));
                Assert.That(result.Data[2].Key, Is.EqualTo(expected.Data[2].Key));
                Assert.That(result.Data[2].Value, Is.EqualTo(expected.Data[2].Value));
                Assert.That(result.Data[4].Key, Is.EqualTo(expected.Data[4].Key));
                Assert.That(result.Data[4].Value, Is.EqualTo(expected.Data[4].Value));
            });
        }
    }
}