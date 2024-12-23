namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Collections.Generic;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;
    using Common.Utility.Services.Interfaces;
    using Excel.Model;
    using Fanex.Athena.BetList.DataAccess.Interfaces;
    using Fanex.Athena.BetList.Excel;
    using Fanex.Athena.BetList.Service.Interfaces;
    using Fanex.Athena.Common.Utility.Collections;
    using Fanex.Athena.Resources;
    using Fanex.Athena.UnitTest.Common.Extensions;
    using Fanex.BetList.Core.Entities;
    using Model.DTO;
    using Model.MetaModel;
    using Newtonsoft.Json;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using Utility;

    public class BettingTicketCenterAdvancedSearchControllerTest
    {
        private const int RowsExceedLimitErrorCode = 1001;

        private IAppSettings _appSettings;

        private IBettingTicketCenterAdvancedSearchDataAccess _dataAccess;

        private IBettingTicketCenterZipBuilder _zipBuilder;

        private IBettingTicketCenterExcel _bettingTicketCenterExcel;

        private ITicketBuilderService _ticketBuilderService;

        private ITempFileService _tempFileService;

        private IIPService _ipService;

        [SetUp]
        public void SetUp()
        {
            _ticketBuilderService = Substitute.For<ITicketBuilderService>();
            _appSettings = Substitute.For<IAppSettings>();
            _dataAccess = Substitute.For<IBettingTicketCenterAdvancedSearchDataAccess>();
            _zipBuilder = Substitute.For<IBettingTicketCenterZipBuilder>();
            _bettingTicketCenterExcel = Substitute.For<IBettingTicketCenterExcel>();
            _ipService = Substitute.For<IIPService>();
            _tempFileService = Substitute.For<ITempFileService>();
        }

        [TearDown]
        public void TearDown()
        {
            _appSettings = null;
            _dataAccess = null;
            _zipBuilder = null;
            _bettingTicketCenterExcel = null;
            _ticketBuilderService = null;
            _tempFileService = null;
        }

        [Test]
        public async Task GetData_WhenGet_GenerateDataWithCorrectOption()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<AdvancedSearchBetListMetaModel>(x => x.IsExport, false);

                var totalRow = FakeHelper.Create<int>();
                var maxTotalRows = FakeHelper.Create<int>();
                _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel, totalRow).Returns(totalRow);
                _appSettings.GetValue("MaxTicketLimit").Returns(totalRow.ToString());

                var ticketResult = new TicketResult();
                _dataAccess.GetBetListForMultipleCustomersAsync(metaModel).Returns(ticketResult);

                // Action
                await controller.GetData(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).ReturnData(
                        ticketResult,
                        Arg.Is<BettingTicketCenterOption>(x =>
                            x.ShowTotal
                            && x.ShowSumStake
                            && x.ShowLicenseeWl
                            && x.ShowWinLoss
                            && x.ShowExternalId
                            && x.ShowSystemId
                            && x.IsAllowedCustomerComm == metaModel.HasPlayerCom),
                        metaModel);
                });
            }
        }

        [Test]
        public async Task GetData_WhenExport_RowsExceedLimitation_ReturnError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<AdvancedSearchBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                var maxTotalRows = FakeHelper.Create<int>();
                _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel, (totalRow - 1)).Returns(totalRow);
                _appSettings.GetValue("MaxTicketLimit").Returns((totalRow - 1).ToString());

                // Action
                var result = (await controller.GetExportStatus(metaModel) as JsonResult).Data;
                var errorCode = result.GetValue("StatusCode");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(errorCode, Is.EqualTo(RowsExceedLimitErrorCode));
                });
            }
        }

        [Test]
        public async Task GetData_WhenExportExcel_RowsExceedMaxRowPerFile_NoMainTicket_ReturnError()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<AdvancedSearchBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                var maxRowPerFile = totalRow - 1;
                _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel, (totalRow + 1)).Returns(totalRow);
                _appSettings.GetValue("MaxTicketLimit").Returns((totalRow + 1).ToString());
                _appSettings.GetValue("MaxRowPerExcelFile").Returns(maxRowPerFile.ToString());

                var ticketResult = new GroupTicketResult
                {
                    TotalRows = 0
                };
                _dataAccess.GetBetListForMultipleCustomersAsync(metaModel, maxRowPerFile).Returns(ticketResult);

                // Action
                var result = await controller.GetData(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
                });
            }
        }

        [Test]
        public async Task GetData_WhenExportExcel_ExportWithCorrectParameter()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<AdvancedSearchBetListMetaModel>(x => x.IsExport, true);
                var filePath = FakeHelper.Create<string>();

                var totalRow = FakeHelper.Create<int>();
                var maxRowPerFile = totalRow + 1;
                _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel, (totalRow + 1)).Returns(totalRow);
                _appSettings.GetValue("MaxTicketLimit").Returns((totalRow + 1).ToString());
                _appSettings.GetValue("MaxRowPerExcelFile").Returns(maxRowPerFile.ToString());
                _appSettings.GetValue("MaxRowForMultipleExcelFiles").Returns(maxRowPerFile.ToString());

                var ticketResult = new GroupTicketResult
                {
                    TotalRows = totalRow,
                };

                ticketResult.DataPath.Add(filePath);
                _dataAccess.GetBetListForMultipleCustomersAsync(metaModel, maxRowPerFile).Returns(ticketResult);

                var data = new TicketResult { MainTicketList = new SortableList<LrfTicket> { FakeHelper.Create<LrfTicket>() } };
                var jsonData = JsonConvert.SerializeObject(data);
                _tempFileService.ReadString(Arg.Is<string>(filePath)).Returns(jsonData);

                // Action
                await controller.GetData(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    controller.Received(1).ExportExcel(
                        Arg.Is<BettingTicketCenterExcelModel>(x =>
                            x.Option.ShowTotal
                            && x.Option.ShowSumStake
                            && x.Option.ShowLicenseeWl
                            && x.Option.ShowWinLoss
                            && x.Option.ShowExternalId
                            && x.Option.ShowSystemId
                            && x.TotalText == $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}"
                            && x.Option.IsAllowedCustomerComm == metaModel.HasPlayerCom));
                });
            }
        }

        [Test]
        public async Task GetData_ExportZipFile_ZipBuilderReturnNull_ReturnNull()
        {
            using (var controller = CreateController())
            {
                // Arrange
                var metaModel = FakeHelper.Create<AdvancedSearchBetListMetaModel>(x => x.IsExport, true);

                var totalRow = FakeHelper.Create<int>();
                _dataAccess.GetTotalRowForMultipleCustomersAsync(metaModel, (totalRow + 1)).Returns(totalRow);
                _appSettings.GetValue("MaxTicketLimit").Returns((totalRow + 1).ToString());

                var maxRowPerFile = totalRow - 1;
                _appSettings.GetValue("MaxRowPerExcelFile").Returns(maxRowPerFile.ToString());
                _appSettings.GetValue("MaxRowForMultipleExcelFiles").Returns(maxRowPerFile.ToString());

                var ticketResult = new GroupTicketResult
                {
                    TotalRows = totalRow,
                    DataPath = new List<string> { "/Test" }
                };
                _dataAccess.GetBetListForMultipleCustomersAsync(metaModel, maxRowPerFile).Returns(ticketResult);

                SetUpForResponse(controller, false);
                _zipBuilder.BuildZip(
                    controller.HttpContext.Response,
                    ticketResult,
                    Arg.Any<string>(),
                    Arg.Any<AdvancedSearchBetListMetaModel>(),
                    Arg.Any<string>()).Returns(x => null);

                // Action
                var result = await controller.GetData(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(result, Is.Null);
                });
            }
        }

        private BettingTicketCenterAdvancedSearchController CreateController()
        {
            var controller = Substitute.ForPartsOf<BettingTicketCenterAdvancedSearchController>(
                _appSettings,
                _dataAccess,
                _ipService,
                _ticketBuilderService,
                _zipBuilder,
                _bettingTicketCenterExcel,
                _tempFileService);

            controller.When(x => x.ReturnData(Arg.Any<TicketResult>(), Arg.Any<BettingTicketCenterOption>(), Arg.Any<ICommonValueMetaModel>())).DoNotCallBase();
            controller.When(x => x.ExportExcel(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            return controller;
        }

        private static void SetUpForResponse(BettingTicketCenterAdvancedSearchController controller, bool isClientConnected)
        {
            var response = Substitute.For<HttpResponseBase>();
            response.IsClientConnected.Returns(isClientConnected);

            var httpContext = Substitute.For<HttpContextBase>();
            httpContext.Response.Returns(response);

            var context = new ControllerContext(httpContext, new System.Web.Routing.RouteData(), controller);
            controller.ControllerContext = context;
        }
    }
}