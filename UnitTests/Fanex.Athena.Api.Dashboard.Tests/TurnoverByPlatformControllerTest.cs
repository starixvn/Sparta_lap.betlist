namespace Fanex.Athena.Api.Dashboard.Tests
{
    using System.Collections.Generic;
    using Model.Dashboard.DTO;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Dashboard.Interfaces;

    [TestFixture]
    public class TurnoverByPlatformControllerTest
    {
        private ITurnoverByPlatformService _turnoverByPlatformService;
        private TurnoverByPlatformController _turnoverByPlatformController;

        [OneTimeSetUp]
        public void SetUp()
        {
            _turnoverByPlatformService = Substitute.For<ITurnoverByPlatformService>();
            _turnoverByPlatformController = new TurnoverByPlatformController(_turnoverByPlatformService);
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
            var expected = new TurnoverByPlatformViewModel
            {
                Data = new List<Platform>
                {
                    new Platform { Name = "Asia", Value = 100M },
                    new Platform { Name = "China", Value = 300M },
                    new Platform { Name = "WAP", Value = 800M },
                    new Platform { Name = "Smartsite", Value = 700M },
                    new Platform { Name = "Others", Value = 1700M }
                }
            };
            var metaModel = new DashboardBaseModel();
            _turnoverByPlatformService.GetData(metaModel).Returns(expected);

            // Action
            var result = _turnoverByPlatformController.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(5));
                Assert.That(result.Data[0].Name, Is.EqualTo(expected.Data[0].Name));
                Assert.That(result.Data[0].Value, Is.EqualTo(expected.Data[0].Value));
                Assert.That(result.Data[2].Name, Is.EqualTo(expected.Data[2].Name));
                Assert.That(result.Data[2].Value, Is.EqualTo(expected.Data[2].Value));
                Assert.That(result.Data[4].Name, Is.EqualTo(expected.Data[4].Name));
                Assert.That(result.Data[4].Value, Is.EqualTo(expected.Data[4].Value));
            });
        }
    }
}