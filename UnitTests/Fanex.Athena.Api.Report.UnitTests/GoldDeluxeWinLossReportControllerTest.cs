namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Base.UnitTests;
    using Constant.DataDomain;
    using DataAccess.Report.Interfaces;
    using Excel.Base.Entities;
    using Excel.Report;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Session.Enum;
    using Session.Model;

    public class GoldDeluxeWinLossReportControllerTest : BaseApiControllerHelperTest
    {
        private IGoldDeluxeWinLossDataAccess _dataAccess;
        private GoldDeluxeWinLossReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _dataAccess = Substitute.For<IGoldDeluxeWinLossDataAccess>();

            _controller = Substitute.ForPartsOf<GoldDeluxeWinLossReportController>(
                _dataAccess,
                SessionService,
                ProductService,
                LangService);

            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _controller = null;
            _dataAccess = null;
        }

        [Test]
        public void Index_InvalidModel_ReturnEmptyData()
        {
            // Arrange
            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = 1,
                ProductId = ProductTypes.GoldDeluxe
            };
            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>(),
                ApplicationMode = AppMode.Self,
                FunctionIdList = new int[0]
            });

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.Data);
                Assert.IsEmpty(result.Total);
                Assert.IsEmpty(result.SummaryTotal);
            });
        }

        [Test]
        public void Index_NoDataReturnedFromService_ReturnEmptyData()
        {
            // Arrange
            var metaModel = CreateValidMetaModel();

            _dataAccess.GetSummaryWinloss(Arg.Any<ThirdPartyWinLossMetaModel>()).Returns(
                new ReportResultModel<GoldDeluxeWinLoss>());

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.Data);
                Assert.IsEmpty(result.Total);
                Assert.IsEmpty(result.SummaryTotal);
            });
        }

        [Test]
        public void Index_ValidModelLevel0_CallServiceWithCorrectParameter()
        {
            // Arrange
            var model = CreateValidMetaModel();

            SetUpForGettingData();

            // Action
            _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.DrillDownLevel, Is.Zero);
                Assert.That(model.DrillDownCustomerId, Is.Zero);
                Assert.That(model.LicenseeId, Is.EqualTo(SessionLicenseeId));
                _dataAccess.ReceivedWithAnyArgs().GetSummaryWinloss(model);
            });
        }

        [Test]
        public void Index_ValidModelLevel0_ReturnCorrectData()
        {
            // Arrange
            var model = CreateValidMetaModel();

            var expectedResult = SetUpForGettingData();

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.EqualTo(expectedResult.DataList));
                Assert.That(result.Total, Is.EqualTo(expectedResult.TotalDataList));
                Assert.That(result.SummaryTotal, Is.EqualTo(expectedResult.SummaryTotal));
            });
        }

        [Test]
        public void Index_ValidModelLevel1_CallServiceWithCorrectParameter()
        {
            // Arrange
            var model = CreateValidMetaModel();
            var level = 1;
            var fdate = DateTime.Now;
            var tdate = DateTime.Now.AddDays(1);
            var currencyId = 2;
            var custId = 3;

            SetUpForGettingData(level);

            // Action
            _controller.Index(model, fdate, tdate, level, currencyId, custId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.DrillDownLevel, Is.EqualTo(level));
                Assert.That(model.DrillDownCustomerId, Is.EqualTo(custId));
                Assert.That(model.DrillDownFromDate, Is.EqualTo(fdate));
                Assert.That(model.DrillDownToDate, Is.EqualTo(tdate));
                Assert.That(model.LicenseeId, Is.EqualTo(SessionLicenseeId));
                SessionService.Received().GenerateAgentListXml(currencyId, Arg.Any<bool>());
                _dataAccess.ReceivedWithAnyArgs().GetAgentWinlossDetail(model);
            });
        }

        [Test]
        public void Index_ValidModelLevel2_CallServiceWithCorrectParameter()
        {
            // Arrange
            var model = CreateValidMetaModel();
            var level = 2;
            var fdate = DateTime.Now;
            var tdate = DateTime.Now.AddDays(1);
            var currencyId = 2;
            var custId = 3;

            SetUpForGettingData(level);

            // Action
            _controller.Index(model, fdate, tdate, level, currencyId, custId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.DrillDownLevel, Is.EqualTo(level));
                Assert.That(model.DrillDownCustomerId, Is.EqualTo(custId));
                Assert.That(model.DrillDownFromDate, Is.EqualTo(fdate));
                Assert.That(model.DrillDownToDate, Is.EqualTo(tdate));
                Assert.That(model.LicenseeId, Is.EqualTo(SessionLicenseeId));
                SessionService.Received().GenerateAgentListXml(currencyId, Arg.Any<bool>());
                _dataAccess.ReceivedWithAnyArgs().GetCustomerWinlossDetail(model);
            });
        }

        [Test]
        public void Index_ExportExcelLevel0_ExportWithCorrectData()
        {
            // Arrange
            var currencyId = 0;
            var drilldownCurrencyId = 1;
            var model = CreateValidMetaModel(currencyId, drilldownCurrencyId);
            model.IsExportExcel = true;

            var expectedFileName = $"GDWinLoss_{model.FromDate:ddMMMyyyy}_All.xls";

            SetUpForGettingData();
            SetUpForExportExcel();

            // Action
            _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.FromDate, Is.Not.EqualTo(model.DrillDownFromDate));
                Assert.That(model.ToDate, Is.Not.EqualTo(model.DrillDownToDate));
                _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), expectedFileName);
            });
        }

        [TestCase(1)]
        [TestCase(2)]
        public void Index_ExportExcel_ExportWithCorrectData(int level)
        {
            // Arrange
            var modelCurrencyId = 1;
            var currencyId = 2;
            var model = CreateValidMetaModel(modelCurrencyId, currencyId);
            model.IsExportExcel = true;
            var fdate = DateTime.Now;
            var tdate = DateTime.Now.AddDays(1);
            var custId = 4;

            var expectedFileName = $"GDWinLoss_{fdate:ddMMMyyyy}_{nameof(ThirdPartyWinLossMetaModel.DrillDownCurrencyId)}.xls";

            SetUpForGettingData(level);
            SetUpForExportExcel();

            // Action
            _controller.Index(model, fdate, tdate, level, currencyId, custId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.CurrencyId, Is.EqualTo(model.DrillDownCurrencyId));
                Assert.That(model.FromDate, Is.EqualTo(model.DrillDownFromDate));
                Assert.That(model.ToDate, Is.EqualTo(model.DrillDownToDate));
                _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), expectedFileName);
            });
        }

        private ThirdPartyWinLossMetaModel CreateValidMetaModel(int currencyId = 1, int drillDownCurrencyId = 2)
        {
            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = currencyId,
                AgentListXml = string.Empty,
                FromDate = DateTime.Now.AddDays(-2),
                ToDate = DateTime.Now.AddDays(-1),
                DrillDownCurrencyId = drillDownCurrencyId,
                ProductId = ProductTypes.GoldDeluxe,
                ProductCode = nameof(ThirdPartyWinLossMetaModel.ProductCode)
            };
            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { model.CurrencyId, nameof(model.CurrencyId) },
                    { drillDownCurrencyId, nameof(model.DrillDownCurrencyId) }
                },
                ApplicationMode = AppMode.Self,
                FunctionIdList = new int[0]
            });
            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);

            return model;
        }

        private ReportResultModel<GoldDeluxeWinLoss> SetUpForGettingData(int level = 0)
        {
            var expectedData = new List<GoldDeluxeWinLoss> { new GoldDeluxeWinLoss { Index = 1 } };
            var expectedSummaryData = new Dictionary<string, GoldDeluxeWinLoss> { { nameof(GoldDeluxeWinLoss), new GoldDeluxeWinLoss() } };
            var expectedTotalData = new List<GoldDeluxeWinLoss> { new GoldDeluxeWinLoss { Index = 0 } };

            var expectedResult = new ReportResultModel<GoldDeluxeWinLoss>
            {
                DataList = expectedData,
                SummaryTotal = expectedSummaryData,
                TotalDataList = expectedTotalData
            };

            if (level == 0)
            {
                _dataAccess.GetSummaryWinloss(Arg.Any<ThirdPartyWinLossMetaModel>()).Returns(expectedResult);
            }
            else if (level == 1)
            {
                _dataAccess.GetAgentWinlossDetail(Arg.Any<ThirdPartyWinLossMetaModel>()).Returns(expectedResult);
            }
            else
            {
                _dataAccess.GetCustomerWinlossDetail(Arg.Any<ThirdPartyWinLossMetaModel>()).Returns(expectedResult);
            }

            return expectedResult;
        }

        private void SetUpForExportExcel()
        {
            var excel = Substitute.ForPartsOf<GoldDeluxeWinLossExcel>(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   new ThirdPartyWinLossMetaModel(),
                   new ReportResultModel<GoldDeluxeWinLoss>
                   {
                       DataList = new List<GoldDeluxeWinLoss>(),
                       SummaryTotal = new Dictionary<string, GoldDeluxeWinLoss>(),
                       TotalDataList = new List<GoldDeluxeWinLoss>()
                   });
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   new ThirdPartyWinLossMetaModel(),
                   new ReportResultModel<GoldDeluxeWinLoss>()).ReturnsForAnyArgs(excel);
        }
    }
}