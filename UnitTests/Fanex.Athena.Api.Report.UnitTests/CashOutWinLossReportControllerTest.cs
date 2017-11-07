namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;

    public class CashOutWinLossReportControllerTest : BaseApiReportControllerHelperTest
    {
        private ICashOutWinLossService _service;
        private CashOutWinLossReportController _controller;
        private JavaScriptSerializer _serializer;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            MockSessionData();

            _service = Substitute.For<ICashOutWinLossService>();
            _controller = Substitute.ForPartsOf<CashOutWinLossReportController>(_service, SessionService, ProductService);
            _serializer = new JavaScriptSerializer();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _service = null;
            _controller = null;
        }

        [Test]
        public void Index_InvalidRequest_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = new ThirdPartyWinLossMetaModel { CurrencyId = -4 };

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.Empty);
                Assert.That(result.Total, Is.Empty);
                Assert.That(result.SummaryTotal, Is.Empty);
            });
        }

        [Test]
        public void Index_Level0_CallGetSummaryMethodInService()
        {
            // Arrange
            const int CurrencyId = -2, Level = 0, CustId = 1;
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = CurrencyId,
                IsExcludedTestData = true,
                DrillDownFromDate = new DateTime(2017, 4, 1),
                DrillDownToDate = new DateTime(2017, 4, 18),
                DrillDownLevel = 0,
                DrillDownCurrencyId = CurrencyId,
                DrillDownCustomerId = CustId,
                AgentListXml = "XmlData"
            };
            SessionService.GenerateAgentListXml(CurrencyId, metaModel.IsExcludedTestData).Returns("XmlData");
            var dataListResult = new SortableList<CashOutWinLoss>
            {
                new CashOutWinLoss()
            };
            var totalDataResult = dataListResult.ToList();
            var summaryTotalResult = new Dictionary<string, CashOutWinLoss> { { "EUR", new CashOutWinLoss() } };
            _service.GetSummary(Arg.Is<ThirdPartyWinLossMetaModel>(x => IsValidMetaModelForLevel0(x, Level))).Returns(
                new SortableReportResultModel<CashOutWinLoss>
                {
                    SummaryTotal = summaryTotalResult,
                    DataList = dataListResult
                });
            _service.TotalCalc(dataListResult).Returns(totalDataResult);
            var expectedResult = CreateResultData(dataListResult, totalDataResult, summaryTotalResult);

            // Action
            var result = _serializer.Serialize(_controller.Index(metaModel, null, null, Level, CurrencyId, CustId));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_Level1_CallGetDetailMethodInService()
        {
            // Arrange
            const int CurrencyId = -2, Level = 1, CustId = 1;
            var fromDate = new DateTime(2017, 4, 1);
            var toDate = new DateTime(2017, 4, 18);
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = CurrencyId,
                IsExcludedTestData = true,
                DrillDownLevel = Level,
                DrillDownCurrencyId = CurrencyId,
                DrillDownCustomerId = CustId,
                AgentListXml = "XmlData"
            };
            SessionService.GenerateAgentListXml(CurrencyId, metaModel.IsExcludedTestData).Returns("XmlData");
            var dataListResult = new SortableList<CashOutWinLoss>
            {
                new CashOutWinLoss()
            };
            var totalDataResult = dataListResult.ToList();
            var summaryTotalResult = new Dictionary<string, CashOutWinLoss> { { "EUR", new CashOutWinLoss() } };
            _service.GetDetail(Arg.Is<ThirdPartyWinLossMetaModel>(x => IsValidMetaModelForLevel0(x, Level))).Returns(
                new SortableReportResultModel<CashOutWinLoss>
                {
                    SummaryTotal = summaryTotalResult,
                    DataList = dataListResult
                });
            _service.TotalCalc(dataListResult).Returns(totalDataResult);
            var expectedResult = CreateResultData(dataListResult, totalDataResult, summaryTotalResult);

            // Action
            var result = _serializer.Serialize(_controller.Index(metaModel, fromDate, toDate, Level, CurrencyId, CustId));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_DataListParamReturnEmpty_ReturnEmptyResult()
        {
            // Arrange
            const int CurrencyId = -2, Level = 1, CustId = 1;
            var fromDate = new DateTime(2017, 4, 1);
            var toDate = new DateTime(2017, 4, 18);
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = -1,
                IsExcludedTestData = true,
                DrillDownLevel = Level,
                DrillDownCurrencyId = CurrencyId,
                DrillDownCustomerId = CustId,
                AgentListXml = "XmlData"
            };
            SessionService.GenerateAgentListXml(CurrencyId, metaModel.IsExcludedTestData).Returns("XmlData");

            _service.GetDetail(Arg.Any<ThirdPartyWinLossMetaModel>()).ReturnsForAnyArgs(new SortableReportResultModel<CashOutWinLoss>());

            var expectedResult = CreateResultData(new List<CashOutWinLoss>(), new List<CashOutWinLoss>(), new Dictionary<string, CashOutWinLoss>());

            // Action
            var result = _serializer.Serialize(_controller.Index(metaModel, fromDate, toDate, Level, CurrencyId, CustId));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_IsExportExcelTrue_CallExcelStream()
        {
            // Arrange
            const int CurrencyId = -2, Level = 1, CustId = 1;
            var fromDate = new DateTime(2017, 4, 1);
            var toDate = new DateTime(2017, 4, 18);
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = -1,
                IsExcludedTestData = true,
                DrillDownLevel = Level,
                DrillDownCurrencyId = CurrencyId,
                DrillDownCustomerId = CustId,
                IsExportExcel = true,
                DrillDownFromDate = fromDate,
                DrillDownToDate = toDate,
                FromDate = fromDate
            };
            _service.GetSummary(Arg.Any<ThirdPartyWinLossMetaModel>()).ReturnsForAnyArgs(new SortableReportResultModel<CashOutWinLoss>());
            var excel = Substitute.ForPartsOf<CashOutWinLossExcel>(
                string.Empty, 
                new ConvertingOption(), 
                metaModel, 
                new CashOutWinLossViewModel
                {
                    MainResult = new CashOutWinLoss[] { }
                });
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<ThirdPartyWinLossMetaModel>(),
                Arg.Any<CashOutWinLossViewModel>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            // Action
            _controller.Index(metaModel);

            // Assert
            _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>());
        }

        [TestCase(0, -2)]
        [TestCase(1, -2)]
        [TestCase(1, 1)]
        public void Index_IsExportExcelTrueWithDifferentLevelAndCurrency_UpdateMetanData(int level, int currencyId)
        {
            // Arrange
            const int CustId = 1, DrilldownCurrency = 2;
            var fromDate = new DateTime(2017, 4, 1);
            var toDate = new DateTime(2017, 4, 18);
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = currencyId,
                IsExcludedTestData = true,
                IsExportExcel = true,
                FromDate = fromDate.AddDays(1),
                ToDate = toDate.AddDays(1)
            };

            _service.GetSummary(Arg.Any<ThirdPartyWinLossMetaModel>()).ReturnsForAnyArgs(new SortableReportResultModel<CashOutWinLoss>());
            var excel = Substitute.ForPartsOf<CashOutWinLossExcel>(
                string.Empty,
                new ConvertingOption(),
                metaModel, 
                new CashOutWinLossViewModel
                {
                    MainResult = new CashOutWinLoss[] { }
                });
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<ThirdPartyWinLossMetaModel>(),
                Arg.Any<CashOutWinLossViewModel>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            SessionService.UserSession.CurrencyList = new Dictionary<int, string> { { currencyId, "Currency" } };

            var expectedFromDate = level > 0 && currencyId <= 0 ? fromDate : metaModel.FromDate;
            var expectedToDate = level > 0 && currencyId <= 0 ? toDate : metaModel.ToDate;
            var expectedCurrency = level > 0 && currencyId > 0 ? DrilldownCurrency : metaModel.CurrencyId;

            // Action
            _controller.Index(metaModel, fromDate, toDate, level, DrilldownCurrency, CustId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(metaModel.FromDate, Is.EqualTo(expectedFromDate));
                Assert.That(metaModel.ToDate, Is.EqualTo(expectedToDate));
                Assert.That(metaModel.CurrencyId, Is.EqualTo(expectedCurrency));
            });
        }

        private static bool IsValidMetaModelForLevel0(ThirdPartyWinLossMetaModel metaModel, int level)
        {
            return metaModel.DrillDownCurrencyId == -2 && metaModel.DrillDownLevel == level &&
                metaModel.DrillDownCustomerId == 1 && metaModel.DrillDownFromDate == new DateTime(2017, 4, 1) &&
                metaModel.DrillDownToDate == new DateTime(2017, 4, 18) && metaModel.AgentListXml == "XmlData";
        }

        private string CreateResultData(List<CashOutWinLoss> data, List<CashOutWinLoss> total, Dictionary<string, CashOutWinLoss> summaryTotal)
        {
            return _serializer.Serialize(
                new
                {
                    Data = data,
                    Total = total,
                    SummaryTotal = summaryTotal
                });
        }
    }
}