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
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    public class CashOutWinLossReportControllerTest
    {
        private ISessionService _sessionService;
        private IProductService _productService;
        private ICashOutWinLossService _service;
        private CashOutWinLossReportController _controller;
        private JavaScriptSerializer _serializer;

        [SetUp]
        public void SetUp()
        {
            _sessionService = Substitute.For<ISessionService>();
            _productService = Substitute.For<IProductService>();
            _service = Substitute.For<ICashOutWinLossService>();
            _controller = Substitute.ForPartsOf<CashOutWinLossReportController>(_service, _sessionService, _productService);
            _serializer = new JavaScriptSerializer();
            CreateSession();
        }

        [TearDown]
        public void TearDown()
        {
            _service = null;
            _controller = null;
            _sessionService = null;
        }

        [Test]
        public void Index_WhenCallWithInvalidModel_ReturnEmptyResult()
        {
            // Arrange
            var metaModel = new CashOutWinLossMetaModel { CurrencyId = 1 };
            var expectedResult = CreateResultData(new List<CashOutWinLoss>(), new List<CashOutWinLoss>(), new Dictionary<string, CashOutWinLoss>());

            // Action
            var result = _serializer.Serialize(_controller.Index(metaModel));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_WhenCallWithLevel0_CallGetSummaryMethodInService()
        {
            // Arrange
            const int CurrencyId = -2, Level = 0, CustId = 1;
            var metaModel = new CashOutWinLossMetaModel
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
            _sessionService.GenerateAgentListXml(CurrencyId, metaModel.IsExcludedTestData).Returns("XmlData");
            IDictionary<string, CashOutWinLoss> summaryData;
            var dataListResult = new SortableList<CashOutWinLoss>
            {
                new CashOutWinLoss()
            };
            var totalDataResult = dataListResult.ToList();
            var summaryTotalResult = new Dictionary<string, CashOutWinLoss> { { "EUR", new CashOutWinLoss() } };
            _service.GetSummary(Arg.Is<CashOutWinLossMetaModel>(x => IsValidMetaModelForLevel0(x, Level)), out summaryData).Returns(x =>
            {
                x[1] = summaryTotalResult;
                return dataListResult;
            });
            _service.TotalCalc(dataListResult).Returns(totalDataResult);
            var expectedResult = CreateResultData(dataListResult, totalDataResult, summaryTotalResult);

            // Action
            var result = _serializer.Serialize(_controller.Index(metaModel, null, null, Level, CurrencyId, CustId));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_WhenCallWithLevel1_CallGetDetailMethodInService()
        {
            // Arrange
            const int CurrencyId = -2, Level = 1, CustId = 1;
            DateTime fromDate = new DateTime(2017, 4, 1), toDate = new DateTime(2017, 4, 18);
            var metaModel = new CashOutWinLossMetaModel
            {
                CurrencyId = CurrencyId,
                IsExcludedTestData = true,
                DrillDownLevel = Level,
                DrillDownCurrencyId = CurrencyId,
                DrillDownCustomerId = CustId,
                AgentListXml = "XmlData"
            };
            _sessionService.GenerateAgentListXml(CurrencyId, metaModel.IsExcludedTestData).Returns("XmlData");
            IDictionary<string, CashOutWinLoss> summaryData;
            var dataListResult = new SortableList<CashOutWinLoss>
            {
                new CashOutWinLoss()
            };
            var totalDataResult = dataListResult.ToList();
            var summaryTotalResult = new Dictionary<string, CashOutWinLoss> { { "EUR", new CashOutWinLoss() } };
            _service.GetDetail(Arg.Is<CashOutWinLossMetaModel>(x => IsValidMetaModelForLevel0(x, Level)), out summaryData).Returns(x =>
            {
                x[1] = summaryTotalResult;
                return dataListResult;
            });
            _service.TotalCalc(dataListResult).Returns(totalDataResult);
            var expectedResult = CreateResultData(dataListResult, totalDataResult, summaryTotalResult);

            // Action
            var result = _serializer.Serialize(_controller.Index(metaModel, fromDate, toDate, Level, CurrencyId, CustId));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void Index_WhenDataListParamReturnEmpty_ReturnEmptyResult()
        {
            // Arrange
            const int CurrencyId = -2, Level = 1, CustId = 1;
            DateTime fromDate = new DateTime(2017, 4, 1), toDate = new DateTime(2017, 4, 18);
            var metaModel = new CashOutWinLossMetaModel
            {
                CurrencyId = -1,
                IsExcludedTestData = true,
                DrillDownLevel = Level,
                DrillDownCurrencyId = CurrencyId,
                DrillDownCustomerId = CustId,
                AgentListXml = "XmlData"
            };
            _sessionService.GenerateAgentListXml(CurrencyId, metaModel.IsExcludedTestData).Returns("XmlData");
            IDictionary<string, CashOutWinLoss> summaryData;

            _service.GetDetail(Arg.Any<CashOutWinLossMetaModel>(), out summaryData).ReturnsForAnyArgs(x =>
            {
                return null;
            });

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
            DateTime fromDate = new DateTime(2017, 4, 1), toDate = new DateTime(2017, 4, 18);
            var metaModel = new CashOutWinLossMetaModel
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

            var excel = Substitute.ForPartsOf<CashOutWinLossExcel>(string.Empty, new ConvertingOption(), metaModel, new CashOutWinLossViewModel());
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<CashOutWinLossMetaModel>(),
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
        public void Index_IsExportExcelTrueWithDifferentLevelAndCurrency_CallExcelStream(int level, int currencyId)
        {
            // Arrange
            const int CustId = 1, DrillDownCurrencyId = -2;
            DateTime fromDate = new DateTime(2017, 4, 1), toDate = new DateTime(2017, 4, 18);
            var metaModel = new CashOutWinLossMetaModel
            {
                CurrencyId = currencyId,
                IsExcludedTestData = true,
                DrillDownLevel = level,
                DrillDownCurrencyId = DrillDownCurrencyId,
                DrillDownCustomerId = CustId,
                IsExportExcel = true,
                DrillDownFromDate = fromDate,
                DrillDownToDate = toDate,
                FromDate = fromDate
            };

            var excel = Substitute.ForPartsOf<CashOutWinLossExcel>(string.Empty, new ConvertingOption(), metaModel, new CashOutWinLossViewModel());
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<CashOutWinLossMetaModel>(),
                Arg.Any<CashOutWinLossViewModel>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            _sessionService.UserSession.CurrencyList = new Dictionary<int, string>() { { currencyId, "Currency" } };

            // Action
            _controller.Index(metaModel, fromDate, toDate, level, currencyId, CustId);

            // Assert
            _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>());
        }

        [Test]
        public void Index_WhenCallWithMetaModelFromFormBody_AddDataForDrillDownPropertiesOfMetaModel()
        {
            // Arrange
            const int CustId = 1, DrillDownCurrencyId = -2, CurrencyId = -2, Level = 1;
            var fromDate = new DateTime(2017, 4, 1);
            var toDate = new DateTime(2017, 4, 18);
            var inputedMetaModel = new CashOutWinLossMetaModel
            {
                CurrencyId = CurrencyId,
                IsExcludedTestData = true,
                DrillDownLevel = Level,
                DrillDownCurrencyId = DrillDownCurrencyId,
                DrillDownCustomerId = CustId,
                IsExportExcel = false,
                DrillDownFromDate = fromDate,
                DrillDownToDate = toDate,
                FromDate = fromDate
            };
            var expectedDrillDownData = new
            {
                DrillDownCurrencyId = 0,
                DrillDownLevel = 0,
                DrillDownCustomerId = 0,
                DrillDownFromDate = inputedMetaModel.DrillDownFromDate,
                DrillDownToDate = inputedMetaModel.DrillDownToDate
            };

            var excel = Substitute.ForPartsOf<CashOutWinLossExcel>(string.Empty, new ConvertingOption(), inputedMetaModel, new CashOutWinLossViewModel());
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<CashOutWinLossMetaModel>(),
                Arg.Any<CashOutWinLossViewModel>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            // Action
            _controller.Index(inputedMetaModel, null, null);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedDrillDownData.DrillDownCurrencyId, inputedMetaModel.DrillDownCurrencyId);
                Assert.AreEqual(expectedDrillDownData.DrillDownLevel, inputedMetaModel.DrillDownLevel);
                Assert.AreEqual(expectedDrillDownData.DrillDownCustomerId, inputedMetaModel.DrillDownCustomerId);
                Assert.AreEqual(expectedDrillDownData.DrillDownFromDate, inputedMetaModel.DrillDownFromDate);
                Assert.AreEqual(expectedDrillDownData.DrillDownToDate, inputedMetaModel.DrillDownToDate);
            });
        }

        [Test]
        public void Index_WhenCallWithManyInputs_AddDataForDrillDownPropertiesOfMetaModel()
        {
            // Arrange
            var fromDateInput = new DateTime(2017, 4, 1);
            var toDateInput = new DateTime(2017, 4, 18);
            var levelInput = 1;
            var currencyInput = -1;
            var customerIdInput = 1;

            var inputedMetaModel = new CashOutWinLossMetaModel
            {
                CurrencyId = -2,
                IsExcludedTestData = true,
                IsExportExcel = false,
                FromDate = new DateTime(2017, 4, 1)
            };

            var excel = Substitute.ForPartsOf<CashOutWinLossExcel>(string.Empty, new ConvertingOption(), inputedMetaModel, new CashOutWinLossViewModel());
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<CashOutWinLossMetaModel>(),
                Arg.Any<CashOutWinLossViewModel>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            // Action
            _controller.Index(inputedMetaModel, fromDateInput, toDateInput, levelInput, currencyInput, customerIdInput);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.AreEqual(currencyInput, inputedMetaModel.DrillDownCurrencyId);
                Assert.AreEqual(levelInput, inputedMetaModel.DrillDownLevel);
                Assert.AreEqual(customerIdInput, inputedMetaModel.DrillDownCustomerId);
                Assert.AreEqual(fromDateInput, inputedMetaModel.DrillDownFromDate);
                Assert.AreEqual(toDateInput, inputedMetaModel.DrillDownToDate);
            });
        }

        [Test]
        public void Index_IsExportToExcelWithLevelAndCurrencyGreaterThanZero_ChangeCurrencyIdOfMetaModel()
        {
            // Arrange
            const int CustId = 1, Level = 1;
            var fromDate = new DateTime(2017, 4, 1);
            var drillDownFromDate = new DateTime(2017, 5, 1);
            var toDate = new DateTime(2017, 4, 18);
            var drillDownToDate = new DateTime(2017, 5, 18);
            var initialCurrencyId = 1;
            var changedCurrencyId = -2;

            var metaModel = new CashOutWinLossMetaModel
            {
                CurrencyId = initialCurrencyId,
                IsExcludedTestData = true,
                DrillDownLevel = Level,
                DrillDownCurrencyId = changedCurrencyId,
                DrillDownCustomerId = CustId,
                IsExportExcel = true,
                DrillDownFromDate = drillDownFromDate,
                DrillDownToDate = drillDownToDate,
                FromDate = fromDate,
                ToDate = toDate
            };

            var excel = Substitute.ForPartsOf<CashOutWinLossExcel>(string.Empty, new ConvertingOption(), metaModel, new CashOutWinLossViewModel());
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<CashOutWinLossMetaModel>(),
                Arg.Any<CashOutWinLossViewModel>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            _sessionService.UserSession.CurrencyList = new Dictionary<int, string>() { { initialCurrencyId, "Currency" } };

            // Action
            _controller.Index(metaModel, drillDownFromDate, drillDownToDate, Level, changedCurrencyId, CustId);

            // Assert
            Assert.AreEqual(changedCurrencyId, metaModel.CurrencyId);
            Assert.AreEqual(fromDate, metaModel.FromDate);
            Assert.AreEqual(toDate, metaModel.ToDate);
        }

        [Test]
        public void Index_IsExportToExcelWithLevelGreaterThanZeroAndCurrencyLessThanZero_ChangeFromDateAndToDateOfMetaModel()
        {
            // Arrange
            const int CustId = 1, Level = 1, CurrencyId = -2, DrillDownCurrencyId = -1;
            var initialFromDate = new DateTime(2017, 4, 1);
            var initialToDate = new DateTime(2017, 4, 18);
            var changedFromDate = new DateTime(2017, 5, 1);
            var changedToDate = new DateTime(2017, 5, 18);

            var metaModel = new CashOutWinLossMetaModel
            {
                CurrencyId = CurrencyId,
                IsExcludedTestData = true,
                DrillDownLevel = Level,
                DrillDownCurrencyId = DrillDownCurrencyId,
                DrillDownCustomerId = CustId,
                IsExportExcel = true,
                DrillDownFromDate = changedFromDate,
                DrillDownToDate = changedToDate,
                FromDate = initialFromDate,
                ToDate = initialToDate
            };

            var excel = Substitute.ForPartsOf<CashOutWinLossExcel>(string.Empty, new ConvertingOption(), metaModel, new CashOutWinLossViewModel());
            excel.When(x => x.Export()).DoNotCallBase();
            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<string>(),
                Arg.Any<ConvertingOption>(),
                Arg.Any<CashOutWinLossMetaModel>(),
                Arg.Any<CashOutWinLossViewModel>())
                .Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            // Action
            _controller.Index(metaModel, changedFromDate, changedToDate, Level, DrillDownCurrencyId, CustId);

            // Assert
            Assert.AreEqual(CurrencyId, metaModel.CurrencyId);
            Assert.AreEqual(changedFromDate, metaModel.FromDate);
            Assert.AreEqual(changedToDate, metaModel.ToDate);
        }

        private static bool IsValidMetaModelForLevel0(CashOutWinLossMetaModel metaModel, int level)
        {
            return metaModel.DrillDownCurrencyId == -2 && metaModel.DrillDownLevel == level &&
                metaModel.DrillDownCustomerId == 1 && metaModel.DrillDownFromDate == new DateTime(2017, 4, 1) &&
                metaModel.DrillDownToDate == new DateTime(2017, 4, 18) && metaModel.AgentListXml == "XmlData";
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