namespace Fanex.Athena.Controllers.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Web.Mvc;
    using Model.BetList.MetaModel;
    using Model.BetList.ViewModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    public class CorrectScoreBetListControllerTest : BetListBaseControllerHelperTest
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
        public void Index_WhenGet_ReturnCorrectView()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index() as ViewResult;

                // Assert
                Assert.That(result.ViewName, Is.EqualTo("~/Views/BetList/CorrectScoreBetList/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = controller.Index() as ViewResult;
                var model = (BetListBaseViewModel)result.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    AssertDefaultBetListViewModel(model.Param, new BetListMetaModel());
                    Assert.That(model.ShowEmptyContent, Is.True);
                });
            }
        }

        private CorrectScoreBetListController CreateController()
        {
            var controller = Substitute.ForPartsOf<CorrectScoreBetListController>(
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