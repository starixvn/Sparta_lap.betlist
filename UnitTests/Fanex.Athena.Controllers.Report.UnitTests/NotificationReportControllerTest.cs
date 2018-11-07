namespace Fanex.Athena.Controllers.Report.UnitTests
{
    using System;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;

    public class NotificationReportControllerTest : BaseControllerHelperTest
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
                Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/Notification/Notification.cshtml"));
            }
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewModel()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var model = (NotificationViewModel)viewResult.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.FromDate.DayOfWeek, Is.EqualTo(DayOfWeek.Sunday));
                    Assert.That(model.ToDate.Subtract(model.FromDate).TotalDays, Is.LessThanOrEqualTo(7));
                });
            }
        }

        private NotificationReportController CreateController()
        {
            var productService = Substitute.For<IProductService>();

            var controller = Substitute.ForPartsOf<NotificationReportController>(
                LangService,
                SessionService,
                AppSettings,
                productService);
            return controller;
        }
    }
}