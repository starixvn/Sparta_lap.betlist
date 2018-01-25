namespace Fanex.Athena.Controllers.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DTO.Common;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    public class LicenseeWinLossByTimeZoneReportControllerTest : BaseControllerHelperTest
    {
        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            StubUserSessionData(DefaultUserSession);
            StubLicenseeSessionData(DefaultLicenseeSession);
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewName()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;

                // Assert
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/LicenseeWinLossByTimeZone/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var productList = new List<Product>
                {
                    new Product
                    {
                       ProductCode = Product.AB
                    }
                };

                ProductService.GetSingleWalletProductAccess(Arg.Any<int>(), Arg.Any<string>(), Arg.Any<string>())
                    .Returns(new ProductAccess(productList));

                // Action
                var viewResult = controller.Index() as ViewResult;
                var model = (LicenseeWinLossByTimeZoneViewModel)viewResult.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.ProductOptionList.Count, Is.EqualTo(productList.Count));
                    Assert.That(model.ProductOptionList.Select(x => x.Code).ToArray(), Is.EqualTo(productList.Select(x => x.ProductCode).ToArray()));
                });
            }
        }

        private LicenseeWinLossByTimeZoneReportController CreateController()
        {
            var controller = Substitute.ForPartsOf<LicenseeWinLossByTimeZoneReportController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                ProductService);
            return controller;
        }
    }
}