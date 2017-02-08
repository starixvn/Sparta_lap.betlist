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
    public class LicenseeWinLossByPlatformServiceTest
    {
        private IWidgetByPlatformDataAccess _licenseeWinLossByPlatformDataAccess;
        private IWidgetByPlatformService _licenseeWinLossByPlatformService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _licenseeWinLossByPlatformDataAccess = Substitute.For<IWidgetByPlatformDataAccess>();
            _licenseeWinLossByPlatformService = new LicenseeWinLossByPlatformService(_licenseeWinLossByPlatformDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _licenseeWinLossByPlatformDataAccess = null;
            _licenseeWinLossByPlatformService = null;
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
            _licenseeWinLossByPlatformDataAccess.GetData(metaModel).Returns(expected);

            // Action
            var result = _licenseeWinLossByPlatformService.GetData(metaModel);

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