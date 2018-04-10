namespace Fanex.Athena.Controllers.Report.UnitTests
{
    using System;
    using System.Web.Mvc;
    using Base.UnitTests;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration.Interfaces;

    public class ExchangeWinLossReportControllerTest : BaseControllerHelperTest
    {
        private readonly DateTime _minDate = new DateTime(2017, 1, 1);
        private readonly DateTime _maxDate = new DateTime(2018, 1, 3);

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
                // Action
                var viewResult = controller.Index() as ViewResult;
                var model = (ExchangeWinLossViewModel)viewResult.Model;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(model.MinDate, Is.EqualTo(_minDate));
                    Assert.That(model.MaxDate, Is.EqualTo(_maxDate));
                });
            }
        }

        private ExchangeWinLossReportController CreateController()
        {
            var reportSettingService = Substitute.For<IReportSettingService>();
            reportSettingService.GetSetting(PermissionKeys.ReportLicenseeWinLoss).Returns(new ReportSetting
            {
                MinDate = _minDate,
                MaxDate = _maxDate
            });
            var controller = Substitute.ForPartsOf<ExchangeWinLossReportController>(
                LangService,
                SessionService,
                AppSettings,
                ClientViewHelper,
                Substitute.For<IProductService>(),
                reportSettingService);
            return controller;
        }
    }
}