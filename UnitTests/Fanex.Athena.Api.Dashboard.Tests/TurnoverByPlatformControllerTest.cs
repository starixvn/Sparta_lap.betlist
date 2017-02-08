namespace Fanex.Athena.Api.Dashboard.Tests
{
    using System.Collections.Generic;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Configuration.Interfaces;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class TurnoverByPlatformControllerTest
    {
        private IWidgetByPlatformService _turnoverByPlatformService;

        private IBalanceUpService _balanceUpService;

        private TurnoverByPlatformController _turnoverByPlatformController;

        [OneTimeSetUp]
        public void SetUp()
        {
            _balanceUpService = Substitute.For<IBalanceUpService>();
            _turnoverByPlatformService = Substitute.For<IWidgetByPlatformService>();
            _turnoverByPlatformController = new TurnoverByPlatformController(_turnoverByPlatformService, _balanceUpService);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _turnoverByPlatformService = null;
            _turnoverByPlatformController = null;
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
            _turnoverByPlatformService.GetData(metaModel).Returns(expected);

            // Action
            var result = _turnoverByPlatformController.GetData(metaModel);

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