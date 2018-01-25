namespace Fanex.Athena.Controllers.Report.UnitTests
{
    using System;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    public class ExchangeWinLossReportControllerTest : BaseControllerHelperTest
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
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/ExchangeWinLoss/Main.cshtml"));
            }
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var minDate = new DateTime(2017, 1, 1);
                var maxDate = new DateTime(2017, 1, 10);

                AppSettings.HistoryDate.Returns(minDate);
                AppSettings.MaxDate.Returns(maxDate);

                // Action
                var viewResult = controller.Index() as ViewResult;
                var model = (ExchangeWinLossViewModel)viewResult.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.MinDate, Is.EqualTo(minDate));
                    Assert.That(model.MaxDate, Is.EqualTo(maxDate));
                });
            }
        }

        private ExchangeWinLossReportController CreateController()
        {
            var controller = Substitute.ForPartsOf<ExchangeWinLossReportController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                ProductService);
            return controller;
        }
    }
}