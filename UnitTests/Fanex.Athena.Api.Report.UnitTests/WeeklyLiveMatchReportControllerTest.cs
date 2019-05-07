namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Base.UnitTests;
    using Excel.Report;
    using Fanex.Athena.Service.Report.Interfaces;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using Model.Report.ResultModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    internal class WeeklyLiveMatchReportControllerTest : BaseApiControllerHelperTest
    {
        private IWeeklyLiveMatchService _service;
        private WeeklyLiveMatchReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            MockSessionData();

            _service = Substitute.For<IWeeklyLiveMatchService>();
            _controller = Substitute.ForPartsOf<WeeklyLiveMatchReportController>(_service, SessionService, ProductService, LangService);
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
        public async Task Index_InvalidRequest_ReturnEmptyData()
        {
            // Arrange
            var model = new WeeklyLiveMatchMetaModel
            {
                CurrencyId = 1
            };

            // Action
            var result = await _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.WeekHeaders);
                Assert.Zero(result.NumberOfWeeks);
                Assert.IsEmpty(result.Data);
                Assert.IsEmpty(result.TotalData);
            });
        }

        [Test]
        public async Task Index_NoDataReturned_ReturnEmptyData()
        {
            // Arrange
            var model = new WeeklyLiveMatchMetaModel
            {
                CurrencyId = -1
            };

            _service.GetData(Arg.Any<WeeklyLiveMatchMetaModel>()).Returns(new WeeklyLiveMatchResultModel
            {
                DataList = Enumerable.Empty<WeeklyLiveMatch>()
            });
            _service.GetWeekHeaders(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>()).Returns(Enumerable.Empty<string>());

            // Action
            var result = await _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.WeekHeaders);
                Assert.Zero(result.NumberOfWeeks);
                Assert.IsEmpty(result.Data);
                Assert.IsEmpty(result.TotalData);
            });
        }

        [Test]
        public async Task Index_ValidModel_ReturnCorrectData()
        {
            // Arrange
            const int NumberOfWeek = 3;
            var fromDate = new DateTime(2017, 1, 1);
            var model = new WeeklyLiveMatchMetaModel
            {
                CurrencyId = -1,
                FromDate = fromDate,
                ToDate = fromDate.AddDays((NumberOfWeek - 1) * 7)
            };

            var data = new List<WeeklyLiveMatch>
            {
                new WeeklyLiveMatch { Index = 1, SportName = "Sport 1", NumbersOfMatches = new[] { 1, 2, 3 } },
                new WeeklyLiveMatch { Index = 2, SportName = "Sport 2", NumbersOfMatches = new[] { 2, 3 } },
                new WeeklyLiveMatch { Index = 3, SportName = "Sport 3", NumbersOfMatches = new[] { 3 } }
            };
            var totalData = data.Select(d => d.Index).ToArray();
            _service.GetData(Arg.Any<WeeklyLiveMatchMetaModel>()).Returns(new WeeklyLiveMatchResultModel
            {
                TotalData = totalData,
                DataList = data,
                NumberOfWeeks = NumberOfWeek
            });

            var weekHeaders = new[] { "H1", "H2", "H3" };
            _service.GetWeekHeaders(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>()).Returns(weekHeaders);

            // Action
            var result = await _controller.Index(model);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.WeekHeaders, Is.EqualTo(weekHeaders));
                Assert.That(result.NumberOfWeeks, Is.EqualTo(NumberOfWeek));
                Assert.That(result.Data[0].Cell[0], Is.EqualTo(data[0].Index));
                Assert.That(result.TotalData, Is.EqualTo(totalData));
            });
        }

        [Test]
        public async Task Index_ExportExcel_ExportWithCorrectData()
        {
            // Arrange
            var model = new WeeklyLiveMatchMetaModel
            {
                CurrencyId = -1,
                IsExportExcel = true
            };

            _service.GetData(Arg.Any<WeeklyLiveMatchMetaModel>()).Returns(new WeeklyLiveMatchResultModel
            {
                DataList = Enumerable.Empty<WeeklyLiveMatch>()
            });
            _service.GetWeekHeaders(Arg.Any<DateTime>(), Arg.Any<DateTime>(), Arg.Any<int>()).Returns(Enumerable.Empty<string>());

            var expectedFileName = "WeeklyLiveMatches.xls";

            var excel = Substitute.ForPartsOf<WeeklyLiveMatchExcel>();
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                Arg.Any<string>(),
                Arg.Any<WeeklyLiveMatchMetaModel>(),
                Arg.Any<WeeklyLiveMatchResultModel>(),
                Arg.Any<IEnumerable<string>>()).ReturnsForAnyArgs(excel);

            // Action
            await _controller.Index(model);

            // Assert
            _controller.Received().ExcelStream(Arg.Any<IWorkbook>(), expectedFileName);
        }
    }
}