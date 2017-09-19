namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Excel.Base.Entities;
    using Excel.Report;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerBalanceReportControllerTest : ApiReportBaseControllerTest
    {
        private ICustBalanceDataAccess _dataAccess;
        private CustomerBalanceReportController _controller;

        [OneTimeSetUp]
        public override void SetUp()
        {
            base.SetUp();

            _dataAccess = Substitute.For<ICustBalanceDataAccess>();

            _controller = Substitute.ForPartsOf<CustomerBalanceReportController>(_dataAccess, SessionService, ProductService);

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
            var model = new CustomerBalanceMetaModel
            {
                IsExportExcel = false,
                FromDate = DateTime.Now.AddDays(1),
                ToDate = DateTime.Now
            };

            var result = _controller.Index(model);

            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.Data);
                Assert.That(0, Is.EqualTo(result.TotalRows));
            });
        }

        [Test]
        public void Index_ValidModel_GetDataFromDataAccess()
        {
            // Arrange
            CustBalance totalData;
            int totalRows;
            var metaModel = CreateValidMetaModel();
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
            CustBalance totalData;
            int totalRows;
            var metaModel = CreateValidMetaModel();
            metaModel.IsExportExcel = true;
            var expectedDataList = SetUpForGettingData(metaModel, out totalData, out totalRows);
            const int PageSizeExcel = int.MaxValue - 1000;

            MockProductAccess(new string[] { Product.BA });

            var actualDataList = Enumerable.Empty<CustBalance>();
            var excel = Substitute.ForPartsOf<CustomerBalanceExcel>(
                   Arg.Any<string>(),
                   Arg.Any<CustomerBalanceMetaModel>(),
                   Arg.Do<IEnumerable<CustBalance>>(x => actualDataList = x),
                   Arg.Any<CustBalance>(),
                   Arg.Any<ShowingOption>(),
                   Arg.Any<ConvertingOption>());
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(
                   Arg.Any<string>(),
                   Arg.Any<CustomerBalanceMetaModel>(),
                   Arg.Any<IEnumerable<CustBalance>>(),
                   Arg.Any<CustBalance>(),
                   Arg.Any<ShowingOption>(),
                   Arg.Any<ConvertingOption>()).Returns(excel);

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

        private static CustomerBalanceMetaModel CreateValidMetaModel()
        {
            return new CustomerBalanceMetaModel
            {
                FromDate = DateTime.Now,
                ToDate = DateTime.Now.AddDays(1),
                ProductOptions = new List<ProductOption> { new ProductOption { Status = true } },
                AgentListXml = string.Empty
            };
        }

        private IList<CustBalance> SetUpForGettingData(CustomerBalanceMetaModel metaModel, out CustBalance totalData, out int totalRows)
        {
            var expectedData = new List<CustBalance> { new CustBalance { Index = 1 } };

            _dataAccess.GetDataList(metaModel, out totalData, out totalRows).Returns(x =>
            {
                x[1] = new CustBalance { Index = 1 };
                x[2] = 1;
                return expectedData;
            });

            return expectedData;
        }
    }
}