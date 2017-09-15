namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Users;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ViewModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NSubstitute.Core;
    using NUnit.Framework;
    using Service.Report.Interfaces;

    public class ThirdPartyWinLossReportControllerTest : ApiReportBaseControllerTest
    {
        private IAuthorizationService _authorizationService;
        private IThirdPartyWinLossService _thirdPartyWinLossService;
        private ThirdPartyWinLossReportController _controller;

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();

            _authorizationService = Substitute.For<IAuthorizationService>();
            _thirdPartyWinLossService = Substitute.For<IThirdPartyWinLossService>();

            _controller = Substitute.ForPartsOf<ThirdPartyWinLossReportController>(_thirdPartyWinLossService, _authorizationService, SessionService, ProductService);

            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();
        }

        [OneTimeTearDown]
        public override void TearDown()
        {
            base.TearDown();

            _controller = null;
            _authorizationService = null;
            _thirdPartyWinLossService = null;
        }

        [Test]
        public void Index_NoPermission_ReturnForbiddenStatus()
        {
            // Arrange
            var model = new ThirdPartyWinLossMetaModel
            {
                ProductId = ProductTypes.GoldDeluxe
            };

            MockUserSessionData(new UserSession
            {
                ApplicationMode = AppMode.Self,
                FunctionIdList = new List<int>()
            });
            _authorizationService.HasPermissions(Arg.Any<IEnumerable<int>>(), Arg.Any<string>()).Returns(false);

            // Action
            var result = _controller.Index(model) as HttpResponseMessage;

            // Assert
            Assert.That(result.StatusCode, Is.EqualTo(System.Net.HttpStatusCode.Forbidden));
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
            MockUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>(),
                ApplicationMode = AppMode.Self,
                FunctionIdList = new List<int>()
            });
            MockLicenseeSessionData(new LicenseeSession { LicenseeId = 0 });
            _authorizationService.HasPermissions(Arg.Any<IEnumerable<int>>(), Arg.Any<string>()).Returns(true);

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
        public void Index_ValidModelLevel0_CallServiceWithCorrectParameter()
        {
            // Arrange
            var model = CreateValidMetaModel();

            IDictionary<string, ThirdPartyWinLoss> summaryTotal;
            SetUpForGettingData(out summaryTotal);

            // Action
            _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.DrillDownLevel, Is.EqualTo(0));
                Assert.That(model.DrillDownCustomerId, Is.EqualTo(0));
                _thirdPartyWinLossService.ReceivedWithAnyArgs().GetSummaryWinloss(model, out summaryTotal);
            });
        }

        [Test]
        public void Index_ValidModelLevel0_ReturnCorrectData()
        {
            // Arrange
            var model = CreateValidMetaModel();

            IDictionary<string, ThirdPartyWinLoss> summaryTotal;
            var expectedData = SetUpForGettingData(out summaryTotal);
            var expectedTotal = expectedData.AsEnumerable();

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.EqualTo(expectedData));
                Assert.That(result.Total, Is.EqualTo(expectedTotal));
                Assert.That(result.SummaryTotal, Is.EqualTo(summaryTotal));
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

            IDictionary<string, ThirdPartyWinLoss> summaryTotal;
            SetUpForGettingData(out summaryTotal, level);

            // Action
            _controller.Index(model, fdate, tdate, level, currencyId, custId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.DrillDownLevel, Is.EqualTo(level));
                Assert.That(model.DrillDownCustomerId, Is.EqualTo(custId));
                Assert.That(model.DrillDownFromDate, Is.EqualTo(fdate));
                Assert.That(model.DrillDownToDate, Is.EqualTo(tdate));
                SessionService.Received().GenerateAgentListXml(currencyId, Arg.Any<bool>());
                _thirdPartyWinLossService.ReceivedWithAnyArgs().GetAgentWinlossDetail(model, out summaryTotal);
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

            IDictionary<string, ThirdPartyWinLoss> summaryTotal;
            SetUpForGettingData(out summaryTotal, level);

            // Action
            _controller.Index(model, fdate, tdate, level, currencyId, custId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.DrillDownLevel, Is.EqualTo(level));
                Assert.That(model.DrillDownCustomerId, Is.EqualTo(custId));
                Assert.That(model.DrillDownFromDate, Is.EqualTo(fdate));
                Assert.That(model.DrillDownToDate, Is.EqualTo(tdate));
                SessionService.Received().GenerateAgentListXml(currencyId, Arg.Any<bool>());
                _thirdPartyWinLossService.ReceivedWithAnyArgs().GetCustomerWinlossDetail(model, out summaryTotal);
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

            var expectedFileName = $"{model.ProductCode}WinLoss_{model.FromDate:ddMMMyyyy}_All.xls";

            IDictionary<string, ThirdPartyWinLoss> summaryTotal;
            SetUpForGettingData(out summaryTotal);
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

            var expectedFileName = $"{model.ProductCode}WinLoss_{fdate:ddMMMyyyy}_{nameof(ThirdPartyWinLossMetaModel.DrillDownCurrencyId)}.xls";

            IDictionary<string, ThirdPartyWinLoss> summaryTotal;
            SetUpForGettingData(out summaryTotal, level);
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
            MockUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { model.CurrencyId, nameof(model.CurrencyId) },
                    { drillDownCurrencyId, nameof(model.DrillDownCurrencyId) }
                },
                ApplicationMode = AppMode.Self,
                FunctionIdList = new List<int>()
            });
            MockLicenseeSessionData(new LicenseeSession { LicenseeId = 0 });
            _authorizationService.HasPermissions(Arg.Any<IEnumerable<int>>(), Arg.Any<string>()).Returns(true);
            SessionService.GenerateAgentListXml(Arg.Any<int>(), Arg.Any<bool>()).Returns(string.Empty);

            return model;
        }

        private SortableList<ThirdPartyWinLoss> SetUpForGettingData(out IDictionary<string, ThirdPartyWinLoss> summaryTotal, int level = 0)
        {
            var expectedData = new SortableList<ThirdPartyWinLoss> { new ThirdPartyWinLoss { Index = 1 } };
            var expectedSummaryData = new Dictionary<string, ThirdPartyWinLoss> { { nameof(ThirdPartyWinLoss), new ThirdPartyWinLoss() } };

            Func<CallInfo, SortableList<ThirdPartyWinLoss>> returnFunc = x =>
            {
                x[1] = expectedSummaryData;
                return expectedData;
            };

            if (level == 0)
            {
                _thirdPartyWinLossService.GetSummaryWinloss(Arg.Any<ThirdPartyWinLossMetaModel>(), out summaryTotal).Returns(returnFunc);
            }
            else if (level == 1)
            {
                _thirdPartyWinLossService.GetAgentWinlossDetail(Arg.Any<ThirdPartyWinLossMetaModel>(), out summaryTotal).Returns(returnFunc);
            }
            else
            {
                _thirdPartyWinLossService.GetCustomerWinlossDetail(Arg.Any<ThirdPartyWinLossMetaModel>(), out summaryTotal).Returns(returnFunc);
            }

            _thirdPartyWinLossService.TotalCalc(Arg.Any<SortableList<ThirdPartyWinLoss>>()).Returns(expectedData.AsEnumerable());

            summaryTotal = expectedSummaryData;
            return expectedData;
        }

        private void SetUpForExportExcel()
        {
            var excel = Substitute.ForPartsOf<ThirdPartyWinLossExcel>(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Any<ThirdPartyWinLossMetaModel>(),
                   new ThirdPartyWinLossViewModel());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Any<ThirdPartyWinLossMetaModel>(),
                   new ThirdPartyWinLossViewModel()).ReturnsForAnyArgs(excel);
        }
    }
}