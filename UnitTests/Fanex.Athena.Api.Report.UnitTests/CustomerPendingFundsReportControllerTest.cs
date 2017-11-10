namespace Fanex.Athena.Api.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Excel.Report;
    using Excel.Report.Models;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Model.Core.ResultModel;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerPendingFundsReportControllerTest : BaseApiReportControllerHelperTest
    {
        private ICustPendingFundsDataAccess _dataAccess;
        private CustomerPendingFundsReportController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _dataAccess = Substitute.For<ICustPendingFundsDataAccess>();

            _controller = Substitute.ForPartsOf<CustomerPendingFundsReportController>(AppSetting, _dataAccess, SessionService, ProductService);

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
            var model = Substitute.ForPartsOf<CustomerPendingFundsMetaModel>();
            model.IsValid.Returns(false);

            var result = _controller.Index(model);

            Assert.Multiple(() =>
            {
                Assert.IsEmpty(result.Data);
                Assert.That(result.TotalRows, Is.Zero);
            });
        }

        [Test]
        public void Index_ValidModel_GetDataFromDataAccess()
        {
            // Arrange
            var metaModel = CreateValidMetaModel();
            var expectedData = SetUpForGettingData(metaModel);

            // Action
            var result = _controller.Index(metaModel);

            // Assert
            Assert.That(result.Data, Is.EqualTo(expectedData));
        }

        [TestCase(true)]
        [TestCase(false)]
        public void Index_ExportToExcel_ExportWithCorrectData(bool isFakeLicensee)
        {
            // Arrange
            var metaModel = CreateValidMetaModel();
            metaModel.IsExportExcel = true;
            var expectedDataList = SetUpForGettingData(metaModel);
            const int PageSizeExcel = int.MaxValue - 1000;

            AppSetting.FakeLicenseeList.Returns(x => isFakeLicensee ? new int[1] { SessionLicenseeId } : new int[0]);

            var actualExcelModel = new CustomerPendingFundsExcelModel();
            var excel = Substitute.ForPartsOf<CustomerPendingFundsExcel>(Arg.Any<string>(), Arg.Do<CustomerPendingFundsExcelModel>(x => actualExcelModel = x));
            excel.When(x => x.Export()).DoNotCallBase();

            _controller.CreateExcel(Arg.Any<string>(), Arg.Any<CustomerPendingFundsExcelModel>()).Returns(excel);

            // Action
            _controller.Index(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(metaModel.PageIndex, Is.EqualTo(1));
                Assert.That(metaModel.PageSize, Is.EqualTo(PageSizeExcel));
                Assert.That(actualExcelModel.DataList, Is.EqualTo(expectedDataList.AsEnumerable()));
                Assert.That(actualExcelModel.SystemIdVisible, Is.EqualTo(!isFakeLicensee));
            });
        }

        private static CustomerPendingFundsMetaModel CreateValidMetaModel()
        {
            return new CustomerPendingFundsMetaModel
            {
                AgentListXml = string.Empty
            };
        }

        private IEnumerable<CustPendingFunds> SetUpForGettingData(CustomerPendingFundsMetaModel metaModel)
        {
            var expectedData = new List<CustPendingFunds>();

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
                expectedData.Add(item);
            }

            _dataAccess.GetDataList(metaModel).Returns(x =>
            {
                return new ReportResultModel<CustPendingFunds>
                {
                    TotalData = new CustPendingFunds(),
                    TotalRows = total,
                    DataList = expectedData
                };
            });

            return expectedData;
        }
    }
}