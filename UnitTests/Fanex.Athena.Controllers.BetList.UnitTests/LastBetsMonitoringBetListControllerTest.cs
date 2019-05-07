namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Common.Interfaces;
    using Model.BetList.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    public class LastBetsMonitoringBetListControllerTest : CultureBaseControllerHelperTest
    {
        private ISportTypeDataAccess _sportTypeDataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _sportTypeDataAccess = Substitute.For<ISportTypeDataAccess>();

            SetUpDefaultFakeData();

            StubGetValueAppSettings("BaseUsdLicensees", "usd");
        }

        [Test]
        public async Task Index_WhenCall_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Arrange
                _sportTypeDataAccess.GetAllSports().Returns(new Dictionary<int, string>());

                // Action
                var result = await controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/Desktop/BetList/LastBetsMonitoringNew/Main.cshtml"));
            }
        }

        [Test]
        public async Task Index_WhenCall_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string ParlayId = "999";

                var sports = new Dictionary<int, string> { { 1, "Test Sport" } };
                _sportTypeDataAccess.GetAllSports().Returns(sports);

                // Action
                var result = await controller.Index() as ViewResult;
                var model = (LastBetsMonitoringViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.Param.LicenseeId, Is.EqualTo(DefaultLicenseeSession.LicenseeId));
                    Assert.That(model.SportsList.Count, Is.EqualTo(3));
                    Assert.That(model.SportsList.ElementAt(0).Value, Is.EqualTo("0"));
                    Assert.That(model.SportsList.ElementAt(1).Value, Is.EqualTo(sports.ElementAt(0).Key.ToString()));
                    Assert.That(model.SportsList.ElementAt(2).Value, Is.EqualTo(ParlayId));
                });
            }
        }

        private LastBetsMonitoringBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<LastBetsMonitoringBetListController>(
                LangService,
                SessionService,
                AppSettings,
                _sportTypeDataAccess);
            return controller;
        }
    }
}