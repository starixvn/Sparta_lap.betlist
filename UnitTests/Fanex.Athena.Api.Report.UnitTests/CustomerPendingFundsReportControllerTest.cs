namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerPendingFundsReportControllerTest : ApiReportBaseControllerTest
    {
        private ICustPendingFundsDataAccess _dataAccess;
        private CustomerPendingFundsReportController _controller;

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();

            _dataAccess = Substitute.For<ICustPendingFundsDataAccess>();

            _controller = Substitute.ForPartsOf<CustomerPendingFundsReportController>(_dataAccess, SessionService, ProductService);

            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void TearDown()
        {
            base.TearDown();

            _controller = null;
            _dataAccess = null;
        }

        [Test]
        public void Index_ValidModel_GetDataFromDataAccess()
        {
            // Arrange
            CustPendingFunds totalData;
            int totalRows;
            var metaModel = CreateValidMetaModel(nameof(CustPendingFunds.CustId), "asc");
            var expectedData = SetUpForGettingData(metaModel, out totalData, out totalRows);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(expectedData, Is.EqualTo(result.Data));
        }

        [Test]
        public void Index_ExportToExcel_ExportWithCorrectData()
        {
            // Arrange
            CustPendingFunds totalData;
            int totalRows;
            var metaModel = CreateValidMetaModel(nameof(CustPendingFunds.CustId), "asc");
            metaModel.IsExportExcel = true;
            var expectedDataList = SetUpForGettingData(metaModel, out totalData, out totalRows);
            const int PageSizeExcel = int.MaxValue - 1000;

            var actualDataList = Enumerable.Empty<CustPendingFunds>();
            var excel = Substitute.ForPartsOf<CustomerPendingFundsExcel>(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Do<IEnumerable<CustPendingFunds>>(x => actualDataList = x),
                   Arg.Any<CustPendingFunds>());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<ConvertingOption>(),
                   Arg.Any<IEnumerable<CustPendingFunds>>(),
                   Arg.Any<CustPendingFunds>()).Returns(excel);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(1, Is.EqualTo(metaModel.PageIndex));
                Assert.That(PageSizeExcel, Is.EqualTo(metaModel.PageSize));
                Assert.That(actualDataList, Is.EqualTo(expectedDataList.AsEnumerable()));
            });
        }

        private static CustomerPendingFundsMetaModel CreateValidMetaModel(string sortColumn, string sortDirection)
        {
            return new CustomerPendingFundsMetaModel
            {
                AgentListXml = string.Empty,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };
        }

        private IEnumerable<CustPendingFunds> SetUpForGettingData(CustomerPendingFundsMetaModel metaModel, out CustPendingFunds totalData, out int totalRows)
        {
            var expectedSortedData = new SortableList<CustPendingFunds>();
            var nonSortedData = new SortableList<CustPendingFunds>();

            var random = new Random();
            var total = random.Next(5, 10);

            for (int i = 0; i < total; i++)
            {
                var item = new CustPendingFunds
                {
                    CustId = random.Next(1, 10),
                    ExternalId = random.Next(1, 10).ToString(),
                    SystemId = random.Next(1, 10).ToString(),
                    BetCounts = random.Next(1, 10),
                    PendingFunds = random.Next(1, 10)
                };
                nonSortedData.Add(item);
                expectedSortedData.Add(item);
            }

            _dataAccess.GetDataList(metaModel, out totalData, out totalRows).Returns(x =>
            {
                x[1] = new CustPendingFunds();
                x[2] = total;
                return nonSortedData;
            });

            expectedSortedData.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase));

            return expectedSortedData;
        }
    }
}