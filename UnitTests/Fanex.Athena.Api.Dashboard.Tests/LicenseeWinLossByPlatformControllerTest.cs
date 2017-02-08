namespace Fanex.Athena.Api.Dashboard.Tests
{
    using System.Collections.Generic;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class LicenseeWinLossByPlatformControllerTest
    {
        private IWidgetByPlatformService _licenseeWinLossByPlatformService;
        private LicenseeWinLossByPlatformController _licenseeWinLossByPlatformController;

        [OneTimeSetUp]
        public void SetUp()
        {
            _licenseeWinLossByPlatformService = Substitute.For<IWidgetByPlatformService>();
            _licenseeWinLossByPlatformController = new LicenseeWinLossByPlatformController(_licenseeWinLossByPlatformService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _licenseeWinLossByPlatformService = null;
            _licenseeWinLossByPlatformController = null;
        }

        [Test]
        public void Index_WhenCall_ReturnCorrectData()
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
            _licenseeWinLossByPlatformService.GetData(metaModel).Returns(expected);

            // Action
            var result = _licenseeWinLossByPlatformController.GetData(metaModel);

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