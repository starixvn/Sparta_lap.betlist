namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Constant.DataDomain;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;

    public class RunningBetListControllerTest : BetListBaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            ProductService.GetProductAccess(Arg.Any<int>(), Arg.Any<string>())
                .Returns(new ProductAccess(new List<Product> { new Product { ProductCode = Product.AB } }));

            SetUpDefaultFakeData();

            StubGetValueAppSettings("BaseUsdLicensees", "usd");
        }

        [Test]
        public void CleanSheet_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    BetType = (int)BetType.CleanSheet
                };

                // Action
                var result = controller.CleanSheet(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.CleanSheet, "TotalBetsCleanSheet", metaModel, result);
            }
        }

        [Test]
        public void ToWinToNil_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    BetType = (int)BetType.ToWinToNil
                };

                // Action
                var result = controller.ToWinToNil(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.ToWinToNil, "TotalBetsToWinToNil", metaModel, result);
            }
        }

        [Test]
        public void ThreeWayHandicap_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    BetType = (int)BetType.ThreeWayHandicap,
                    MultipleBettype = $"{(int)BetType.ThreeWayHandicap},{(int)BetType.FirstHalfThreeWayHandicap}"
                };

                // Action
                var result = controller.ThreeWayHandicap(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.lbl3WayHandicap, "TotalBets3WayHandicap", metaModel, result);
            }
        }

        private void AssertViewResult(string reportTitle, string userGuide, BetListMetaModel metaModel, ViewResult viewResult)
        {
            var viewModel = (BetListBaseViewModel)viewResult.Model;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/BetList/RunningBetList/Main.cshtml"));
                Assert.That(viewModel.ShowEmptyContent, Is.True);
                Assert.That(viewModel.Param.BetType, Is.EqualTo(metaModel.BetType));
                Assert.That(viewModel.Param.MultipleBettype, Is.EqualTo(metaModel.MultipleBettype));
                Assert.That(viewResult.ViewBag.ReportTitle, Is.EqualTo(reportTitle));
                Assert.That(viewResult.ViewBag.UserGuideName, Is.EqualTo(userGuide));
                AssertDefaultBetListViewModel(viewModel.Param, metaModel);
            });
        }

        private RunningBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<RunningBetListController>(
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