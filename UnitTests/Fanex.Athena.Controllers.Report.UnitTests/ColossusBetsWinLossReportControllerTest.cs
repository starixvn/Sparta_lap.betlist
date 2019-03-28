namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Constant.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;

    [TestFixture]
    public class ColossusBetsWinLossReportControllerTest : CultureBaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            SetUpDefaultFakeData();
        }

        [Test]
        public async Task Index_ProductListNotContainColossusBets_RedirectToAccessDenied()
        {
            using (var controller = CreateController())
            {
                // Action
                var result = await controller.Index() as RedirectResult;

                // Assert
                Assert.That(result.Url, Is.EqualTo("~/Utility/AccessDenied"));
            }
        }

        [Test]
        public async Task Index_ProductListContainColossusBets_ReturnCorrectViewResult()
        {
            using (var controller = CreateController(new[] { ProductTypes.ColossusBets }))
            {
                // Action
                var viewResult = await controller.Index() as ViewResult;
                var viewModel = viewResult.Model as ThirdPartyWinLossViewModel;

                // Assert
                StringAssert.Contains("Report/ColossusBetsWinLoss/Main.cshtml", viewResult.ViewName);
            }
        }

        private ColossusBetsWinLossReportController CreateController(IEnumerable<int> productIdList = null)
        {
            var productList = productIdList == null ?
                new List<Product>() :
                productIdList.Select(x => new Product { ProductID = x });
            var productService = Substitute.For<IProductService>();
            productService.GetProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language).Returns(new ProductAccess(productList.ToList()));
            return new ColossusBetsWinLossReportController(
                LangService,
                SessionService,
                AppSettings,
                productService);
        }
    }
}