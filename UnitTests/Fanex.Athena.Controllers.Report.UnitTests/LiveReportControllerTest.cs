namespace Fanex.Athena.Controllers.TotalBets.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Base.UnitTests;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.DTO.LiveReports;
    using Model.Report.MetaModel;
    using Model.Report.ResultModel;
    using Model.Report.ViewModel.LiveReports;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Service.Report.Interfaces;
    using Web.ResultExtension;

    [TestFixture]
    public class LiveReportControllerTest : CultureBaseControllerHelperTest
    {
        private const int CurrencyId = 999;

        private const string AgentListXml = "melody";

        private static readonly DateTime BalanceUpDate = new DateTime(2017, 3, 4);

        private IBalanceUpDataAccess _balanceUpDataAccess;

        private ILiveReportService _liveReportService;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _liveReportService = Substitute.For<ILiveReportService>();

            _balanceUpDataAccess.GetBalanceUp().Returns(new BalanceUp
            {
                BalanceUpDay = BalanceUpDate
            });

            base.FixtureSetUp();

            SetUpDefaultFakeData();
            SessionService.GenerateAgentListXml(CurrencyId).Returns(AgentListXml);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Index_WhenGet_ReturnCorrectViewResult(bool isViewedHistory)
        {
            using (var controller = CreateController())
            {
                // Action
                var viewResult = controller.Index(isViewedHistory) as ViewResult;
                var viewModel = viewResult.Model as LiveReportViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/LiveReport/Main.cshtml"));
                    Assert.That(viewModel.CurrencyList, Is.EqualTo(SessionService.UserSession.CurrencyList));
                    Assert.That(viewModel.BalanceUpDate, Is.EqualTo(BalanceUpDate));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                    Assert.That(viewModel.Param.UserId, Is.EqualTo(SessionService.UserSession.UserId));
                    Assert.That(viewModel.Param.IsViewedHistory, Is.EqualTo(isViewedHistory));
                    Assert.That(viewModel.Param.CurrencyId, Is.EqualTo(0));
                    Assert.That(viewModel.Param.LiveIndicator, Is.EqualTo(1));
                });
            }
        }

        [Test]
        public async Task IndexPost_NotExportExcel_ReturnCorrectViewResult()
        {
            using (var controller = CreateController("AB,BC,CD"))
            {
                // Arrange
                var metaModel = new LiveReportMetaModel
                {
                    CurrencyId = CurrencyId,
                    IsExportExcel = false
                };

                var expectedData = new List<LiveReport>
                {
                    new LiveReport()
                };
                _liveReportService.GetLiveReportSummary(metaModel).Returns(expectedData);

                // Action
                var viewResult = await controller.Index(metaModel) as ViewResult;
                var viewModel = viewResult.Model as LiveReportViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/LiveReport/Main.cshtml"));
                    Assert.That(viewModel.Param, Is.EqualTo(metaModel));
                    Assert.That(viewModel.BalanceUpDate, Is.EqualTo(BalanceUpDate));
                    Assert.That(viewModel.Param.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(viewModel.Param.LicenseeId, Is.EqualTo(SessionService.LicenseeSession.LicenseeId));
                    Assert.That(viewModel.Param.ProductCodeList, Is.EqualTo("AB,BC,CD"));
                    Assert.That(viewModel.MainResult, Is.EqualTo(expectedData));
                });
            }
        }

        [TestCase("BA,AB", true, true)]
        [TestCase("AB", false, true)]
        [TestCase("BA,AB", true, false)]
        [TestCase("AB", false, false)]
        public async Task IndexPost_ExportExcel_PassParamsToCreateExcelCorrectly(string productCodeList, bool expectedBaVisible, bool expectedRbVisible)
        {
            using (var controller = CreateController(productCodeList))
            {
                // Arrange
                controller.ControllerContext = ControllerContext;
                controller.Request.Form.Returns(new NameValueCollection());
                MockCreatingExcel(controller);
                var metaModel = new LiveReportMetaModel
                {
                    BaseCurrency = "UUS",
                    CurrencyName = "SSU",
                    CurrencyId = CurrencyId,
                    IsExportExcel = true
                };
                var expectedData = new List<LiveReport>
                {
                    new LiveReport()
                };
                _liveReportService.GetLiveReportSummary(metaModel).Returns(expectedData);

                // Action
                var result = await controller.Index(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).CreateExcel(
                        Arg.Any<string>(),
                        Arg.Is<LiveReportViewModel>(x => x.MainResult == expectedData),
                        Arg.Is<ShowingOption>(x => x.BAVisible == expectedBaVisible),
                        Arg.Is<ConvertingOption>(x =>
                            x.IsConvert == metaModel.IsConvert &&
                            x.BaseCurrency == metaModel.BaseCurrency &&
                            x.CurrencyName == metaModel.CurrencyName),
                        Arg.Any<LiveReportDetailViewModel>());
                    Assert.That(result, Is.InstanceOf<ExcelResult>());
                });
            }
        }

        [Test]
        public async Task MatchList_WhenGet_ReturnCorrectViewResult()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = new LiveReportMatchListMetalModel
                {
                    CurrencyId = CurrencyId,
                    CurrencyName = "abc"
                };
                var dataList = new List<LiveReportWinLossByMatch>();
                _liveReportService.GetLiveReportWinLossByMatch(metaModel).Returns(dataList);

                // Action
                var viewResult = await controller.MatchList(metaModel) as ViewResult;
                var viewModel = viewResult.Model as LiveReportMatchListViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/LiveReport/MatchList/Content.cshtml"));
                    Assert.That(viewModel.MainResult, Is.EqualTo(dataList));
                    Assert.That(viewModel.Param, Is.EqualTo(metaModel));
                    Assert.That(viewModel.CurrencyName, Is.EqualTo(metaModel.CurrencyName));
                    Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                });
            }
        }

        [TestCase("ALL", "AB,BC,CD")]
        [TestCase("CD", "CD")]
        public async Task Detail_WhenGet_ReturnCorrectViewResult(string productCode, string expectedProductCodeList)
        {
            using (var controller = CreateController("AB,BC,CD"))
            {
                // Arrange
                var metaModel = new LiveReportMetaModel
                {
                    CurrencyId = CurrencyId,
                    CurrencyName = "abc"
                };
                var resultModel = new LiveReportDetailResultModel
                {
                    DataByCurrency = new List<LiveReport>() { new LiveReport() },
                    DataBySport = new List<LiveReport>() { new LiveReport() },
                    DataByBetType = new List<LiveReportByBetType>()
                };
                _liveReportService.GetLiveReportSumaryDetail(metaModel, UserCurrencyList, Arg.Any<IEnumerable<Product>>()).Returns(resultModel);

                // Action
                var viewResult = await controller.Detail(metaModel, productCode) as ViewResult;
                var viewModel = viewResult.Model as LiveReportDetailViewModel;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(viewResult.ViewName, Is.EqualTo("~/Views/Desktop/Report/LiveReport/AjaxTemplate/LiveReportDetail.cshtml"));
                    Assert.That(viewModel.BySports, Is.EqualTo(resultModel.DataBySport));
                    Assert.That(viewModel.ByCurrencies, Is.EqualTo(resultModel.DataByCurrency));
                    Assert.That(viewModel.ByBetType, Is.EqualTo(resultModel.DataByBetType));
                    Assert.That(metaModel.AgentListXml, Is.EqualTo(AgentListXml));
                    Assert.That(metaModel.ProductCodeList, Is.EqualTo(expectedProductCodeList));
                });
            }
        }

        private static void MockCreatingExcel(LiveReportController controller)
        {
            var excel = Substitute.ForPartsOf<LiveReportExcel>();
            excel.When(x => x.Export()).DoNotCallBase();
            controller.CreateExcel(string.Empty, null, null, null, null).ReturnsForAnyArgs(excel);
        }

        private LiveReportController CreateController(string productCodeList = null)
        {
            var productList = string.IsNullOrEmpty(productCodeList) ?
                new List<Product>() :
                productCodeList.Split(',').Select(x => new Product
                {
                    ProductCode = x
                });
            var productService = Substitute.For<IProductService>();
            productService.GetProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                FunctionNames.LiveReport).Returns(new ProductAccess(productList.ToList()));
            var controller = Substitute.ForPartsOf<LiveReportController>(
                LangService,
                SessionService,
                AppSettings,
                productService,
                _balanceUpDataAccess,
                _liveReportService);
            controller.When(x => x.CreateExcel(string.Empty, null, null, null, null)).DoNotCallBase();
            return controller;
        }
    }
}