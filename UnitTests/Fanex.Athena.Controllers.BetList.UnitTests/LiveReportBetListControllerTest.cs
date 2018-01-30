namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class LiveReportBetListControllerTest : BetListBaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void Match_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateMetaModel();

                // Action
                var result = controller.Match(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/Match.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.Null);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void MixParlay_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateMetaModel();

                // Action
                var result = controller.MixParlay(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/MixParlay.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.Null);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void Outright_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateMetaModel();

                // Action
                var result = controller.Outright(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/Outright.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.Null);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void NumberGame_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = CreateMetaModel();

                // Action
                var result = controller.NumberGame(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/NumberGame.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.Null);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void Racing_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string SportName = "Racing";

                var metaModel = CreateMetaModel();
                CacheRefAdapter.GetSportName(metaModel.SportType).Returns(SportName);

                // Action
                var result = controller.Racing(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/Racing.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.EqualTo(SportName));
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        [Test]
        public void VirtualSports_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                const string SportName = "VirtualSports";

                var metaModel = CreateMetaModel();
                CacheRefAdapter.GetSportName(metaModel.SportType).Returns(SportName);

                // Action
                var result = controller.VirtualSports(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/LiveReport/VirtualSports.cshtml"));
                    Assert.That(result.ViewBag.SportName, Is.EqualTo(SportName));
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        private LiveReportBetListMetaModel CreateMetaModel()
        {
            return new LiveReportBetListMetaModel
            {
                FromDate = new DateTime(2018, 2, 1),
                ToDate = new DateTime(2018, 2, 10),
                Date = new DateTime(2018, 2, 5)
            };
        }

        private LiveReportBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<LiveReportBetListController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                AuthorizationService,
                ProductService,
                BetListService,
                BalanceUpDataAccess,
                CacheRefAdapter);
            return controller;
        }
    }
}