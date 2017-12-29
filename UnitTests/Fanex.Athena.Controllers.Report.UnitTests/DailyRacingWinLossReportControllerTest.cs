namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Service.Report.Interfaces;
    using Web.ResultExtension;

    [TestFixture]
    public class DailyRacingWinLossReportControllerTest : BaseControllerHelperTest
    {
        private const int CurrencyId = 999;

        private const string AgentListXml = "<abc>";

        private static readonly DateTime BalanceUpDate = new DateTime(2017, 3, 4);

        private IBalanceUpDataAccess _balanceUpDataAccess;

        private IDailyRacingWinLossService _dailyRacingWinLossService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _dailyRacingWinLossService = Substitute.For<IDailyRacingWinLossService>();

            _balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp
            {
                BalanceUpDay = BalanceUpDate
            });

            base.FixtureSetUp();

            SetUpDefaultFakeData();

            SessionService.GenerateAgentListXml(CurrencyId).Returns(AgentListXml);
        }

        [Test]
        public void Index_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index() as ViewResult;
                var viewModel = viewResult.Model as DailyRacingWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/DailyRacingWinLoss/Main.cshtml"));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                    Assert.That(viewModel.Param.UserId, Is.EqualTo(SessionService.UserSession.UserId));
                    Assert.That(viewModel.Param.BalanceUpDay, Is.EqualTo(BalanceUpDate.ToShortDateString()));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                });
            }
        }

        [Test]
        public void IndexPost_NotExportExcel_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new DailyRacingWinLossMetaModel
                {
                    CurrencyId = CurrencyId,
                    IsExportExcel = false
                };
               
                var expectedData = new DailyRacingWinLossData();
                _dailyRacingWinLossService.GetDailyRacingWinLossData(metaModel).Returns(expectedData);

                // Action
                var viewResult = controller.Index(metaModel) as ViewResult;
                var viewModel = viewResult.Model as DailyRacingWinLossViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Report/DailyRacingWinLoss/Main.cshtml"));
                    Assert.That(viewModel.Param, Is.EqualTo(metaModel));
                    Assert.That(viewModel.Param.BalanceUpDay, Is.EqualTo(BalanceUpDate.ToShortDateString()));
                    Assert.That(viewModel.Param.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(viewModel.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                    Assert.That(viewModel.DailyRacingWinLossData, Is.EqualTo(expectedData));
                });
            }
        }

        [Test]
        public void IndexPost_ExportExcel_ReturnCorrectRedirectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                MockCreatingExcel(controller);
                var metaModel = new DailyRacingWinLossMetaModel
                {
                    BaseCurrency = "UUS",
                    CurrencyName = "SSU",
                    CurrencyId = CurrencyId,
                    IsExportExcel = true
                };
                var expectedData = new DailyRacingWinLossData();
                _dailyRacingWinLossService.GetDailyRacingWinLossData(metaModel).Returns(expectedData);

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).CreateExcel(
                        Arg.Any<string>(),
                        Arg.Is<DailyRacingWinLossViewModel>(x => x.DailyRacingWinLossData == expectedData),
                        Arg.Is<ConvertingOption>(x =>
                            x.IsConvert == metaModel.IsConvert &&
                            x.BaseCurrency == metaModel.BaseCurrency &&
                            x.CurrencyName == metaModel.CurrencyName),
                        Arg.Any<ShowingOption>());
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        [Test]
        public void IndexPost_ExportExcelLicensee12Bet_ReturnCorrectRedirectResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                SessionService.LicenseeSession.LicenseeId = LicenseeIdentity.OneTwoBet;
                MockCreatingExcel12Bet(controller);
                var metaModel = new DailyRacingWinLossMetaModel
                {
                    BaseCurrency = "ABC",
                    CurrencyName = "CBA",
                    CurrencyId = CurrencyId,
                    IsExportExcel = true
                };
                var expectedData = new DailyRacingWinLossData();
                _dailyRacingWinLossService.GetDailyRacingWinLossData(metaModel).Returns(expectedData);

                // Action
                var result = controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).Create12BetExcel(
                        Arg.Any<string>(),
                        Arg.Is<DailyRacingWinLossViewModel>(x => x.DailyRacingWinLossData == expectedData),
                        Arg.Is<ConvertingOption>(x =>
                            x.IsConvert == metaModel.IsConvert &&
                            x.BaseCurrency == metaModel.BaseCurrency &&
                            x.CurrencyName == metaModel.CurrencyName),
                        Arg.Any<ShowingOption>());
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        private static void MockCreatingExcel(DailyRacingWinLossReportController controller)
        {
            var excel = Substitute.ForPartsOf<DailyRacingWinLossExcel>();
            excel.When(x => x.Export()).DoNotCallBase();
            controller.CreateExcel(string.Empty, null, null, null).ReturnsForAnyArgs(excel);
        }

        private static void MockCreatingExcel12Bet(DailyRacingWinLossReportController controller)
        {
            var excel = Substitute.ForPartsOf<DailyRacingWinLoss12BetExcel>();
            excel.When(x => x.Export()).DoNotCallBase();
            controller.Create12BetExcel(string.Empty, null, null, null).ReturnsForAnyArgs(excel);
        }

        private DailyRacingWinLossReportController CreateController()
        {
            var controller = Substitute.ForPartsOf<DailyRacingWinLossReportController>(
                LangService,
                SessionService,
                AppSettings,
                Substitute.For<IProductService>(),
                _balanceUpDataAccess,
                _dailyRacingWinLossService);
            controller.When(x => x.CreateExcel(string.Empty, null, null, null)).DoNotCallBase();
            controller.When(x => x.Create12BetExcel(string.Empty, null, null, null)).DoNotCallBase();
            return controller;
        }
    }
}