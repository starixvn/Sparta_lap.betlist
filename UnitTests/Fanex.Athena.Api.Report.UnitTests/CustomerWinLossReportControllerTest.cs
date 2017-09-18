namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Excel.Base.Entities;
    using Excel.Report;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerWinLossReportControllerTest : ApiReportBaseControllerTest
    {
        private ICustWinlossDataAccess _dataAccess;
        private CustomerWinLossReportController _controller;

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();

            _dataAccess = Substitute.For<ICustWinlossDataAccess>();

            _controller = Substitute.ForPartsOf<CustomerWinLossReportController>(_dataAccess, SessionService, ProductService);

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
        public void Index_InvalidModel_ReturnEmptyData()
        {
            var model = new CustomerWinLossMetaModel
            {
                IsExportExcel = false,
                FromDate = DateTime.Now.AddDays(1),
                ToDate = DateTime.Now
            };

            var result = _controller.Index(model);

            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.Data);
                Assert.That(result.TotalRows, Is.EqualTo(0));
            });
        }

        [Test]
        public void Index_ValidModel_GetDataFromDataAccess()
        {
            // Arrange
            CustomerWinLoss totalData;
            int totalRows;
            var metaModel = CreateValidMetaModel(nameof(CustomerWinLoss.CustId), "asc");
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
            CustomerWinLoss totalData;
            int totalRows;
            var metaModel = CreateValidMetaModel(nameof(CustomerWinLoss.CustId), "asc");
            metaModel.IsExportExcel = true;
            var expectedDataList = SetUpForGettingData(metaModel, out totalData, out totalRows);

            MockProductAccess(new string[] { Product.BA });

            var actualDataList = Enumerable.Empty<CustomerWinLoss>();
            var actualShowingOption = new ShowingOption();
            var excel = Substitute.ForPartsOf<CustomerWinLossExcel>(
                   Arg.Any<string>(),
                   Arg.Do<ShowingOption>(x => actualShowingOption = x),
                   Arg.Any<ConvertingOption>(),
                   new CustomerWinLossMetaModel(),
                   Arg.Do<IEnumerable<CustomerWinLoss>>(x => actualDataList = x),
                   Arg.Any<CustomerWinLoss>());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<ShowingOption>(),
                   Arg.Any<ConvertingOption>(),
                   new CustomerWinLossMetaModel(),
                   Arg.Any<IEnumerable<CustomerWinLoss>>(),
                   Arg.Any<CustomerWinLoss>()).ReturnsForAnyArgs(excel);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(actualDataList, Is.EqualTo(expectedDataList.AsEnumerable()));
                Assert.That(actualShowingOption.BAVisible, Is.EqualTo(true));
            });
        }

        private static CustomerWinLossMetaModel CreateValidMetaModel(string sortColumn, string sortDirection)
        {
            return new CustomerWinLossMetaModel
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(1),
                ProductOptions = new List<ProductOption> { new ProductOption { Status = true } },
                AgentListXml = string.Empty,
                SortColumn = sortColumn,
                SortDirection = sortDirection
            };
        }

        private IEnumerable<CustomerWinLoss> SetUpForGettingData(CustomerWinLossMetaModel metaModel, out CustomerWinLoss totalData, out int totalRows)
        {
            var expectedSortedData = new SortableList<CustomerWinLoss>();
            var nonSortedData = new SortableList<CustomerWinLoss>();

            var random = new Random();
            var total = random.Next(5, 10);

            for (int i = 0; i < total; i++)
            {
                var item = new CustomerWinLoss
                {
                    CustId = random.Next(1, 10),
                    ExternalId = random.Next(1, 10).ToString(),
                    SystemId = random.Next(1, 10).ToString()
                };
                nonSortedData.Add(item);
                expectedSortedData.Add(item);
            }

            _dataAccess.GetDataList(metaModel, out totalData, out totalRows).Returns(x =>
            {
                x[1] = new CustomerWinLoss();
                x[2] = total;
                return nonSortedData;
            });

            expectedSortedData.Sort(metaModel.SortColumn, metaModel.SortDirection.Equals("asc", StringComparison.OrdinalIgnoreCase));
            var index = 1;
            foreach (var item in expectedSortedData)
            {
                item.Index = index++;
            }

            return expectedSortedData;
        }
    }
}