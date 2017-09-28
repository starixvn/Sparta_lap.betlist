namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using System.Web.Http.Results;
    using DataAccess.Core.Configuration.Interfaces;
    using DataAccess.Report.Interfaces;
    using Excel.Report;
    using Model.Core.DTO.Configuration;
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Resources;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class WinLossByLeagueReportControllerTest
    {
        private ISessionService _sessionService;
        private IWinLossByLeagueDataAccess _dataAccess;
        private WinLossByLeagueReportController _controller;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            var productService = Substitute.For<IProductService>();
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _dataAccess = Substitute.For<IWinLossByLeagueDataAccess>();
            _controller = Substitute.ForPartsOf<WinLossByLeagueReportController>(_sessionService, productService, balanceUpDataAccess, _dataAccess);
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp());
            CreateSession();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _dataAccess = null;
            _controller = null;
            _sessionService = null;
        }

        [Test]
        public void Index_WhenCallWithInvalidModel_ReturnBadRequestWithCorrectMessage()
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel { CurrencyId = 1 };

            // Action
            var result = _controller.Index(metaModel) as BadRequestErrorMessageResult;

            // Assert
            Assert.That(result.Message, Is.EqualTo(ViewResources.InvalidParams));
        }

        [Test]
        public void Index_WhenCallWithLevel0_CallAndGetCorrectDataFromService()
        {
            // Arrange
            var dataListResult = new List<WinLossByLeague>
            {
                new WinLossByLeague
                {
                    BetCount = 100
                },
                new WinLossByLeague
                {
                    BetCount = 200
                }
            };

            _dataAccess.GetSummary(Arg.Any<WinLossByLeagueMetaModel>()).Returns(dataListResult);

            // Action
            var result = _controller.Index(new WinLossByLeagueMetaModel());

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received().GetSummary(Arg.Any<WinLossByLeagueMetaModel>());
                Assert.That(result.Data.Count, Is.EqualTo(2));
                Assert.That(result.Total.BetCount, Is.EqualTo(300));
            });
        }

        [Test]
        public void Index_WhenCallWithLevel1_CallAndGetCorrectDataFromService()
        {
            // Arrange
            const decimal ExpectedAmountDue = 999;
            const int ExpectedTotalRows = 1000;
            var dataListResult = new List<WinLossByLeague>
            {
                new WinLossByLeague()
            };
            WinLossByLeague grandTotal;
            int totalrows = 0;
            _dataAccess.GetCustomers(Arg.Any<WinLossByLeagueMetaModel>(), out grandTotal, ref totalrows)
                .ReturnsForAnyArgs(x =>
            {
                x[1] = new WinLossByLeague
                {
                    AmountDue = ExpectedAmountDue
                };
                x[2] = ExpectedTotalRows;
                return dataListResult;
            });

            // Action
            var result = _controller.Index(new WinLossByLeagueMetaModel
            {
                Level = 1
            });

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.ReceivedWithAnyArgs().GetCustomers(Arg.Any<WinLossByLeagueMetaModel>(), out grandTotal, ref totalrows);
                Assert.That(result.Total.AmountDue, Is.EqualTo(ExpectedAmountDue));
                Assert.That(result.TotalRows, Is.EqualTo(ExpectedTotalRows));
                Assert.That(result.Data.Count, Is.EqualTo(1));
            });
        }

        [Test]
        public void Excel_WhenCallWithInvalidModel_ReturnBadRequestWithCorrectMessage()
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel { CurrencyId = 1 };

            // Action
            var result = _controller.Excel(metaModel);

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(HttpStatusCode.Forbidden));
        }

        [Test]
        public void Excel_ValidModel_CallCorrectMethodAndAssignCorrectData()
        {
            // Arrange
            var metaModel = new WinLossByLeagueMetaModel
            {
                CurrencyId = -1,
                BaseCurrency = "ABC"
            };
            var excel = Substitute.ForPartsOf<WinlossByLeagueExcel>(
                string.Empty,
                metaModel,
                new WinLossByLeagueViewModel
                {
                    MainResult = new List<WinLossByLeague>()
                },
                new WinLossByLeague());
            excel.When(x => x.Export()).DoNotCallBase();

            WinLossByLeagueViewModel viewModel = null;
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<WinLossByLeague>(),
                Arg.Any<WinLossByLeagueMetaModel>(),
                Arg.Do<WinLossByLeagueViewModel>(x => viewModel = x))
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();
            _dataAccess.GetSummary(metaModel).Returns(new List<WinLossByLeague>
            {
                new WinLossByLeague()
            });

            // Action
            _controller.Excel(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                excel.Received().Export();
                _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>());
                Assert.That(viewModel.MainResult.Count(), Is.EqualTo(1));
                Assert.That(metaModel.PageIndex, Is.EqualTo(1));
                Assert.That(metaModel.PageSize, Is.EqualTo(int.MaxValue - 1));
            });
        }

        private void CreateSession()
        {
            var userSession = new UserSession
            {
                CurrencyList = new Dictionary<int, string>
            {
                { -1, "EUR" }, { -2, "USD" }
            }
            };

            var licenseeSession = new LicenseeSession
            {
                LicenseeId = 1
            };

            _sessionService.UserSession.Returns(userSession);
            _sessionService.LicenseeSession.Returns(licenseeSession);
        }
    }
}