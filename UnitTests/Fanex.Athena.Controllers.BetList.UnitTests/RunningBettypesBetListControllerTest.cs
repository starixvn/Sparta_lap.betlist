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

    public class RunningBettypesBetListControllerTest : BetListBaseControllerHelperTest
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
        public void MixParlay_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    MultipleBettype = ((int)BetType.CombinationMixParlay).ToString()
                };

                // Action
                var result = controller.MixParlay(metaModel) as ViewResult;

                // Assert
                AssertViewResult(Menu.Menu_12, "TotalBetsMixParlay", metaModel, result);
            }
        }

        [Test]
        public void DoubleChange_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    MultipleBettype = "24,151,186,410,431"
                };

                // Action
                var result = controller.DoubleChange(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.DoubleChance, "TotalBetsDoubleChance", metaModel, result);
            }
        }

        [Test]
        public void BONTeamToScore_WhenCall_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new BetListMetaModel
                {
                    MultipleBettype = "26,145,146,170,188,427,433"
                };

                // Action
                var result = controller.BONTeamToScore(metaModel) as ViewResult;

                // Assert
                AssertViewResult(ViewResources.BONTeamToScore, "TotalBetsBON", metaModel, result);
            }
        }

        private void AssertViewResult(string reportTitle, string userGuide, BetListMetaModel metaModel, ViewResult viewResult)
        {
            var viewModel = (BetListBaseViewModel)viewResult.Model;

            Assert.Multiple(() =>
            {
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/BetList/RunningBetListMultipleBettypes/Main.cshtml"));
                Assert.That(viewModel.ShowEmptyContent, Is.True);
                Assert.That(viewModel.Param.BetType, Is.EqualTo(metaModel.BetType));
                Assert.That(viewModel.Param.MultipleBettype, Is.EqualTo(metaModel.MultipleBettype));
                Assert.That(viewResult.ViewBag.ReportTitle, Is.EqualTo(reportTitle));
                Assert.That(viewResult.ViewBag.UserGuideName, Is.EqualTo(userGuide));
                AssertDefaultBetListViewModel(viewModel.Param, metaModel);
            });
        }

        private RunningBettypesBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<RunningBettypesBetListController>(
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