namespace Fanex.Athena.Api.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Web.Http;
    using System.Web.Http.Hosting;
    using System.Web.Http.Results;
    using DataAccess.Report.Interfaces;
    using Model.Core.DataDomain;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;
    using Resources;

    public partial class PlatformWinLossReportControllerTest : BaseApiReportControllerHelperTest
    {
        private const int ExpectedCustomerComm = 999;

        private PlatformWinLossReportController _controller;
        private IPlatformWinLossDataAccess _dataAccess;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            MockSessionData();

            _dataAccess = Substitute.For<IPlatformWinLossDataAccess>();
            _controller = Substitute.ForPartsOf<PlatformWinLossReportController>(SessionService, ProductService, _dataAccess);
            _controller.Request = new HttpRequestMessage();
            _controller.Request.Properties.Add(HttpPropertyKeys.HttpConfigurationKey, new HttpConfiguration());
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _dataAccess = null;
            _controller = null;
        }

        [TestCase(2)]
        public void Index_WhenCallWithLevel0_ReturnCorrectData(int numberOfRow)
        {
            // Arrange
            PlatformWinLossSummary totalData;

            // Mock data for Level = 0
            SetUpDataAccessGetSummaryData();

            // Action
            var result = _controller.Index(CreateMetaModel(0));

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.ReceivedWithAnyArgs().GetSummaryData(Arg.Any<PlatformWinLossMetaModel>(), out totalData);
                Assert.That(result.Data.Count, Is.EqualTo(numberOfRow));
                Assert.That(result.Total.CustomerComm, Is.EqualTo(ExpectedCustomerComm));
            });
        }

        [Test]
        public void Index_WhenCallWithInvalidModel_ReturnBadRequestWithCorrectMessage()
        {
            // Arrange
            var metaModel = new PlatformWinLossMetaModel { CurrencyId = 99 };

            // Action
            var result = _controller.Index(metaModel) as BadRequestErrorMessageResult;

            // Assert
            Assert.That(result.Message, Is.EqualTo(ViewResources.InvalidParams));
        }

        [Test]
        public void Index_WhenCallWithLevel1_ReturnCorrectData()
        {
            // Arrange
            PlatformWinLossMember totalData;

            // Mock data for Level = 1
            StubDataAccessGetMemberData();

            // Action
            var result = _controller.Index(CreateMetaModel(1));

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.ReceivedWithAnyArgs().GetMemberData(Arg.Any<PlatformWinLossMetaModel>(), out totalData);
                Assert.That(result.Data.Count, Is.EqualTo(3));
                Assert.That(result.Total.CustomerComm, Is.EqualTo(ExpectedCustomerComm));
            });
        }

        private static PlatformWinLossMetaModel CreateMetaModel(int level, bool isExportExcel = false)
        {
            return new PlatformWinLossMetaModel
            {
                Level = level,
                PlatformType = PlatformType.Asia,
                Year = 2017,
                Month = 1,
                FromDay = 1,
                ToDay = 2,
                IsExportExcel = isExportExcel,
                ProductOptions = new List<ProductOption>
                {
                    new ProductOption
                    {
                        Code = "SB",
                        Status = true
                    }
                }
            };
        }

        private void StubDataAccessGetMemberData()
        {
            var memberData = new List<PlatformWinLossMember>
            {
                new PlatformWinLossMember(),
                new PlatformWinLossMember(),
                new PlatformWinLossMember()
            };
            PlatformWinLossMember totalMemberData;
            _dataAccess.GetMemberData(Arg.Any<PlatformWinLossMetaModel>(), out totalMemberData).ReturnsForAnyArgs(x =>
            {
                x[1] = new PlatformWinLossMember
                {
                    CustomerComm = ExpectedCustomerComm
                };
                return memberData;
            });
        }

        private void SetUpDataAccessGetSummaryData()
        {
            var summaryData = new List<PlatformWinLossSummary>
            {
                new PlatformWinLossSummary(),
                new PlatformWinLossSummary()
            };
            PlatformWinLossSummary totalSummaryData;
            _dataAccess.GetSummaryData(Arg.Any<PlatformWinLossMetaModel>(), out totalSummaryData).ReturnsForAnyArgs(x =>
            {
                x[1] = new PlatformWinLossSummary
                {
                    CustomerComm = ExpectedCustomerComm
                };
                return summaryData;
            });
        }
    }
}