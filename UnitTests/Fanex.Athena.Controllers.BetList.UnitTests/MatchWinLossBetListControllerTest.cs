namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class MatchWinLossBetListControllerTest : BetListBaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public void MatchBetType_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = controller.MatchBetType(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/MatchWl/MatchBetType.cshtml"));
                    Assert.That(model.IsServerPaging, Is.True);
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
                var metaModel = new BetListMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = controller.MixParlay(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/MatchWl/MixParlay.cshtml"));
                    Assert.That(model.IsServerPaging, Is.True);
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
                var metaModel = new BetListMetaModel
                {
                    FromDate = new DateTime(2018, 2, 1),
                    ToDate = new DateTime(2018, 2, 10),
                    Date = new DateTime(2018, 2, 5)
                };

                // Action
                var result = controller.Outright(metaModel) as ViewResult;
                var model = (BetListMetaModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/MatchWl/Outright.cshtml"));
                    Assert.That(model.IsServerPaging, Is.True);
                    AssertDefaultBetListViewModel(model, metaModel);
                });
            }
        }

        private MatchWinLossBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<MatchWinLossBetListController>(
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