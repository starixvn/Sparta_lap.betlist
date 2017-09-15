namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
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

    public class ColossusBetsWinLossReportControllerTest : ApiReportBaseControllerTest
    {
        private IColossusBetsWinLossService _service;
        private ColossusBetsWinLossReportController _controller;

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();

            _service = Substitute.For<IColossusBetsWinLossService>();

            _controller = Substitute.ForPartsOf<ColossusBetsWinLossReportController>(_service, SessionService, ProductService);

            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();
        }

        [Test]
        public void Index_InvalidModel_ReturnEmptyData()
        {
            var model = new ThirdPartyWinLossMetaModel
            {
                IsExportExcel = false,
                CurrencyId = 1,
            };
            CreateFakekUserSessionCurrencyList(new Dictionary<int, string>());
            MockLicenseeSessionData(new LicenseeSession { LicenseeId = 0 });

            var result = _controller.Index(model);

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
            var model = CreateValidMetaModel();

            IDictionary<string, ColossusBetWinLoss> summaryTotal;
            SetUpForGettingData(out summaryTotal);

            _controller.Index(model);

            Assert.Multiple(() =>
            {
                Assert.That(0, Is.EqualTo(model.DrillDownLevel));
                Assert.That(0, Is.EqualTo(model.DrillDownCustomerId));
            });
        }

        [Test]
        public void Index_ValidModelLevel0_ReturnCorrectData()
        {
            var model = CreateValidMetaModel();

            IDictionary<string, ColossusBetWinLoss> summaryTotal;
            var expectedData = SetUpForGettingData(out summaryTotal);
            var expectedTotal = expectedData.AsEnumerable();

            var result = _controller.Index(model);

            Assert.Multiple(() =>
            {
                Assert.AreEqual(expectedData, result.Data);
                Assert.AreEqual(expectedTotal, result.Total);
                Assert.AreEqual(summaryTotal, result.SummaryTotal);
            });
        }

        [Test]
        public void Index_ValidModelLevel1_CallServiceWithCorrectParameter()
        {
            var model = CreateValidMetaModel();
            var level = 1;
            var fdate = DateTime.Now;
            var tdate = DateTime.Now.AddDays(1);
            var currencyId = 2;
            var custId = 3;

            IDictionary<string, ColossusBetWinLoss> summaryTotal;
            SetUpForGettingData(out summaryTotal, level);

            _controller.Index(model, fdate, tdate, level, currencyId, custId);

            Assert.Multiple(() =>
            {
                Assert.That(level, Is.EqualTo(model.DrillDownLevel));
                Assert.That(custId, Is.EqualTo(model.DrillDownCustomerId));
                Assert.That(fdate, Is.EqualTo(model.DrillDownFromDate));
                Assert.That(tdate, Is.EqualTo(model.DrillDownToDate));
                SessionService.Received().GenerateAgentListXml(currencyId, Arg.Any<bool>());
            });
        }

        [Test]
        public void Index_ExportExcelLevel0_ExportWithCorrectData()
        {
            var currencyId = 0;
            var drilldownCurrencyId = 1;
            var model = CreateValidMetaModel(currencyId, drilldownCurrencyId);
            model.IsExportExcel = true;

            var expectedFileName = "ColossusBetsWinLoss_" + model.FromDate.ToString("ddMMMyyy") + "_All.xls";

            IDictionary<string, ColossusBetWinLoss> summaryTotal;
            SetUpForGettingData(out summaryTotal);

            var excel = Substitute.ForPartsOf<ColossusBetsWinLossExcel>(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Any<ThirdPartyWinLossMetaModel>(),
                   new ColossusBetWinLossViewModel());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Any<ThirdPartyWinLossMetaModel>(),
                   new ColossusBetWinLossViewModel()).ReturnsForAnyArgs(excel);

            _controller.Index(model);

            Assert.Multiple(() =>
            {
                Assert.That(model.CurrencyId, Is.Not.EqualTo(model.DrillDownCurrencyId));
                Assert.That(model.FromDate, Is.Not.EqualTo(model.DrillDownFromDate));
                Assert.That(model.ToDate, Is.Not.EqualTo(model.DrillDownToDate));
                _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), expectedFileName);
            });
        }

        [Test]
        public void Index_ExportExcelLevel1_ExportWithCorrectData()
        {
            var modelCurrencyId = 1;
            var drilldownCurrencyId = 2;
            var model = CreateValidMetaModel(modelCurrencyId, drilldownCurrencyId);
            model.IsExportExcel = true;
            var level = 1;
            var fdate = DateTime.Now;
            var tdate = DateTime.Now.AddDays(1);
            var currencyId = 3;
            var custId = 4;

            var expectedFileName = "ColossusBetsWinLoss_" + fdate.ToString("ddMMMyyy") + "_" + nameof(ThirdPartyWinLossMetaModel.DrillDownCurrencyId) + ".xls";

            IDictionary<string, ColossusBetWinLoss> summaryTotal;
            SetUpForGettingData(out summaryTotal, level);

            var excel = Substitute.ForPartsOf<ColossusBetsWinLossExcel>(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Any<ThirdPartyWinLossMetaModel>(),
                   new ColossusBetWinLossViewModel());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Any<ThirdPartyWinLossMetaModel>(),
                   new ColossusBetWinLossViewModel()).ReturnsForAnyArgs(excel);

            _controller.Index(model, fdate, tdate, level, currencyId, custId);

            Assert.Multiple(() =>
            {
                Assert.That(model.DrillDownCurrencyId, Is.EqualTo(model.CurrencyId));
                Assert.That(model.DrillDownFromDate, Is.EqualTo(model.FromDate));
                Assert.That(model.DrillDownToDate, Is.EqualTo(model.ToDate));
                _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), expectedFileName);
            });
        }

        private ThirdPartyWinLossMetaModel CreateValidMetaModel(int currencyId = 1, int drillDownCurrencyId = 2)
        {
            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = currencyId,
                AgentListXml = string.Empty,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(1),
                DrillDownCurrencyId = drillDownCurrencyId
            };
            CreateFakekUserSessionCurrencyList(new Dictionary<int, string> { { model.CurrencyId, nameof(model.CurrencyId) }, { drillDownCurrencyId, nameof(model.DrillDownCurrencyId) } });
            MockLicenseeSessionData(new LicenseeSession { LicenseeId = 0 });
            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);

            return model;
        }

        private SortableList<ColossusBetWinLoss> SetUpForGettingData(out IDictionary<string, ColossusBetWinLoss> summaryTotal, int level = 0)
        {
            var expectedData = new SortableList<ColossusBetWinLoss> { new ColossusBetWinLoss { Index = 1 } };
            var expectedSummaryData = new Dictionary<string, ColossusBetWinLoss> { { nameof(ColossusBetWinLoss), new ColossusBetWinLoss() } };

            if (level == 0)
            {
                _service.GetSummaryWinloss(Arg.Any<ThirdPartyWinLossMetaModel>(), out summaryTotal).Returns(x =>
                {
                    x[1] = expectedSummaryData;
                    return expectedData;
                });
            }
            else
            {
                _service.GetWinlossDetail(Arg.Any<ThirdPartyWinLossMetaModel>(), out summaryTotal).Returns(x =>
                {
                    x[1] = expectedSummaryData;
                    return expectedData;
                });
            }

            _service.CalculateTotal(Arg.Any<SortableList<ColossusBetWinLoss>>()).Returns(expectedData.AsEnumerable());

            summaryTotal = expectedSummaryData;
            return expectedData;
        }

        private void CreateFakekUserSessionCurrencyList(IDictionary<int, string> currencyList)
        {
            MockUserSessionData(new UserSession
            {
                CurrencyList = currencyList
            });
        }
    }
}