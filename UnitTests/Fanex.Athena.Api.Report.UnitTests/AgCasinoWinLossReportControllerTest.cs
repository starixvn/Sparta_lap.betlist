namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Extension.Utility;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Report;
    using Fanex.Athena.Model.Report.ViewModel;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Core.DTO.Users;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class AgCasinoWinLossReportControllerTest : BaseApiReportControllerHelperTest
    {
        private IThirdPartyWinLossService _service;
        private AgCasinoWinLossReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            MockSessionData();

            _service = Substitute.For<IThirdPartyWinLossService>();
            _controller = Substitute.ForPartsOf<AgCasinoWinLossReportController>(_service, SessionService, ProductService);

            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _service = null;
            _controller = null;
        }

        [Test]
        public void Index_InvalidModel_ReturnEmptyData()
        {
            // Arrange
            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = 1
            };
            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string>()
            });
            StubLicenseeSessionData(new LicenseeSession { LicenseeId = 0 });

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
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = -1
            };

            _service.GetSummaryWinloss(Arg.Any<ThirdPartyWinLossMetaModel>(), string.Empty).ReturnsForAnyArgs(x =>
            {
                return new SortableReportResultModel<ThirdPartyWinLoss>();
            });

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
            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = -1
            };

            SetUpForGettingData();

            // Action
            _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(model.DrillDownLevel, Is.Zero);
                Assert.That(model.DrillDownCustomerId, Is.Zero);
                _service.ReceivedWithAnyArgs().GetSummaryWinloss(model);
            });
        }

        [Test]
        public void Index_ValidModelLevel0_ReturnCorrectData()
        {
            // Arrange
            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = -1
            };

            var expectedData = SetUpForGettingData();
            var expectedTotal = expectedData.DataList;

            // Action
            var result = _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.EqualTo(expectedData.DataList));
                Assert.That(result.Total, Is.EqualTo(expectedTotal));
                Assert.That(result.SummaryTotal, Is.EqualTo(expectedData.SummaryTotal));
            });
        }

        [Test]
        public void Index_ValidModelLevel1_CallServiceWithCorrectParameter()
        {
            // Arrange
            var level = 2;
            var fdate = DateTime.Now;
            var tdate = DateTime.Now.AddDays(1);
            var currencyId = 2;
            var custId = 3;
            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = -1
            };

            var expectedData = SetUpForGettingData();
            var expectedTotal = expectedData.DataList;

            // Action
            _controller.Index(model, fdate, tdate, level, currencyId, custId);

            // Assert
            Assert.Multiple(() =>
            {
                _service.ReceivedWithAnyArgs().GetAgentWinlossDetail(model);
                SessionService.Received().GenerateAgentListXml(currencyId, Arg.Any<bool>());
                Assert.That(model.DrillDownCurrencyId, Is.EqualTo(currencyId));
                Assert.That(model.DrillDownLevel, Is.EqualTo(level));
                Assert.That(model.DrillDownCustomerId, Is.EqualTo(custId));
                Assert.That(model.DrillDownFromDate, Is.EqualTo(fdate));
                Assert.That(model.DrillDownToDate, Is.EqualTo(tdate));
            });
        }

        [Test]
        public void Index_ValidModelLevel1_ReturnCorrectData()
        {
            // Arrange
            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = -1
            };

            var expectedData = SetUpForGettingData(1);
            var expectedTotal = expectedData.DataList;

            // Action
            var result = _controller.Index(model, DateTime.Now, DateTime.Now.AddDays(1), 1);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data, Is.EqualTo(expectedData.DataList));
                Assert.That(result.Total, Is.EqualTo(expectedTotal));
                Assert.That(result.SummaryTotal, Is.EqualTo(expectedData.SummaryTotal));
            });
        }

        [Test]
        public void Index_ValidModelLevel2_ReturnCorrectData()
        {
            // Arrange
            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = -1
            };

            var expectedData = SetUpForGettingData(2);
            var expectedTotal = expectedData.DataList;

            // Action
            var result = _controller.Index(model, DateTime.Now, DateTime.Now.AddDays(1), 2);

            // Assert
            Assert.Multiple(() =>
            {
                _service.ReceivedWithAnyArgs().GetCustomerWinlossDetail(model);
                Assert.That(result.Data, Is.EqualTo(expectedData.DataList));
                Assert.That(result.Total, Is.EqualTo(expectedTotal));
                Assert.That(result.SummaryTotal, Is.EqualTo(expectedData.SummaryTotal));
            });
        }

        [Test]
        public void Index_ExportExcelLevel0_ExportWithCorrectData()
        {
            // Arrange
            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string> { { 0, "CurrencyName0" }, { 1, "CurrencyName1" } },
            });

            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = 0,
                IsExportExcel = true,
                DrillDownCurrencyId = 1,
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(1)
            };

            var expectedFileName = $"AGWinLoss_{model.FromDate:ddMMMyyyy}_All.xls";

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
            var currencyId = 2;

            StubUserSessionData(new UserSession
            {
                CurrencyList = new Dictionary<int, string> { { 1, "CurrencyName1" }, { 2, "CurrencyName2" } },
            });

            var model = new ThirdPartyWinLossMetaModel
            {
                CurrencyId = 1,
                IsExportExcel = true,
                FromDate = DateTime.Now.AddDays(-2),
                ToDate = DateTime.Now.AddDays(-1)
            };

            var fdate = DateTime.Now;
            var tdate = DateTime.Now.AddDays(1);
            var custId = 4;

            var expectedFileName = $"AGWinLoss_{fdate:ddMMMyyyy}_CurrencyName2.xls";

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

        private SortableReportResultModel<ThirdPartyWinLoss> SetUpForGettingData(int level = 0)
        {
            var expectedData = new SortableReportResultModel<ThirdPartyWinLoss>
            {
                DataList = new SortableList<ThirdPartyWinLoss> { new ThirdPartyWinLoss { Index = 1 } },
                SummaryTotal = new Dictionary<string, ThirdPartyWinLoss> { { nameof(ThirdPartyWinLoss), new ThirdPartyWinLoss() } }
            };

            if (level == 0)
            {
                _service.GetSummaryWinloss(Arg.Any<ThirdPartyWinLossMetaModel>(), "Lrf_Rpt_AGCasinoWinloss").Returns(expectedData);
            }
            else if (level == 1)
            {
                _service.GetAgentWinlossDetail(Arg.Any<ThirdPartyWinLossMetaModel>(), "LRF_Rpt_AGCasinoWinloss_Ag").Returns(expectedData);
            }
            else
            {
                _service.GetCustomerWinlossDetail(Arg.Any<ThirdPartyWinLossMetaModel>(), "LRF_Rpt_AGCasinoWinloss_Mem").Returns(expectedData);
            }

            _service.TotalCalc(Arg.Any<IEnumerable<ThirdPartyWinLoss>>()).Returns(expectedData.DataList);

            return expectedData;
        }

        private void SetUpForExportExcel()
        {
            var excel = Substitute.ForPartsOf<ThirdPartyWinLossExcel>(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   new ThirdPartyWinLossMetaModel(),
                   new ThirdPartyWinLossViewModel());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   new ThirdPartyWinLossViewModel()).ReturnsForAnyArgs(excel);
        }
    }
}