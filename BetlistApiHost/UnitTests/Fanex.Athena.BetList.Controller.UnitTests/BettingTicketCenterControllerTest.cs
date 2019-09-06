namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Linq;
    using System.Net;
    using System.Threading.Tasks;
    using System.Web.Mvc;
    using Common.Constant;
    using Common.Utility.Collections;
    using Excel;
    using Excel.Model;
    using Model.DTO;
    using Model.MetaModel;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Service.Interfaces;
    using UnitTest.Common.Extensions;
    using UnitTest.Common.Helpers;

    public class BettingTicketCenterControllerTest : BaseControllerHelperTest
    {
        private IBettingTicketCenterService _service;

        private IBettingTicketCenterExcel _bettingTicketCenterExcel;
        private ITicketBuilderService _ticketBuilderService;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _service = Substitute.For<IBettingTicketCenterService>();
            _bettingTicketCenterExcel = Substitute.For<IBettingTicketCenterExcel>();
            _ticketBuilderService = Substitute.For<ITicketBuilderService>();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _ticketBuilderService = null;
            _service = null;
            _bettingTicketCenterExcel = null;
        }

        [Test]
        public async Task GetBySingleCustomer_WhenGet_ReturnData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<SingleCustomersBetListMetaModel>();
                metaModel.IsExport = false;

                var ticketResult = new TicketResult();
                _service.GetBetListForSingleCustomerAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _ticketBuilderService.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom).Returns(ticketText);

                var sumWL = FakeHelper.Create<string>();
                _service.BuildSumWinLoss(ticketResult, metaModel.HasPlayerCom).Returns(sumWL);

                // Action
                var result = (await controller.GetBySingleCustomer(metaModel) as JsonResult).Data;
                var data = result.GetValue("Data");
                var total = result.GetValue("Total");
                var totalRows = result.GetValue("TotalRows");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.EqualTo(ticketText));
                    Assert.That(total.GetValue("SumWinLoss"), Is.EqualTo(sumWL));
                    Assert.That(
                        total.GetValue("SumLicenseeWL"),
                        Is.EqualTo((ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat)));
                    Assert.That(
                        total.GetValue("SumStake"),
                        Is.EqualTo(ticketResult.SumStake.ToString(FormatKey.DecimalFormat)));
                    Assert.That(totalRows, Is.EqualTo(ticketResult.TotalRows));
                });
            }
        }

        [Test]
        public async Task GetBySingleCustomer_WhenExport_EmptyData_ReturnNoContentStatus()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<SingleCustomersBetListMetaModel>();
                metaModel.IsExport = true;

                var ticketResult = new TicketResult();
                _service.GetBetListForSingleCustomerAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                _service.GetTitle(metaModel).Returns(string.Empty);

                // Action
                var result = await controller.GetBySingleCustomer(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
            }
        }

        [Test]
        public async Task GetBySingleCustomer_WhenExport_ExportCorrectData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<SingleCustomersBetListMetaModel>();
                metaModel.IsExport = true;

                var ticketResult = new TicketResult
                {
                    MainTicketList = new SortableList<LrfTicket>
                    {
                        FakeHelper.Create<LrfTicket>()
                    }
                };
                _service.GetBetListForSingleCustomerAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var title = FakeHelper.Create<string>();
                _service.GetTitle(metaModel).Returns(title);

                var workbook = Substitute.For<IWorkbook>();
                _bettingTicketCenterExcel.Export(Arg.Any<BettingTicketCenterExcelModel>()).Returns(workbook);

                // Action
                await controller.GetBySingleCustomer(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    _bettingTicketCenterExcel.Received().Export(Arg.Is<BettingTicketCenterExcelModel>(x => x.Option.ShowTotal
                       && x.Option.ShowSumStake
                       && x.Option.ShowLicenseeWl
                       && x.Option.ShowWinLoss
                       && x.Option.TotalText == ViewResources.Total
                       && x.CustomerCommVisible == metaModel.HasPlayerCom
                       && x.Title == title));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public async Task GetByRefno_WhenGet_ReturnData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<RefNoBetListMetaModel>();
                metaModel.IsExport = false;

                var ticketResult = new TicketResult();
                _service.GetBetListByRefnoAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _ticketBuilderService.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom).Returns(ticketText);

                // Action
                var result = (await controller.GetByRefno(metaModel) as JsonResult).Data;
                var data = result.GetValue("Data");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.EqualTo(ticketText));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public async Task GetByRefno_WhenExport_EmptyData_ReturnNoContentStatus()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<RefNoBetListMetaModel>();
                metaModel.IsExport = true;

                var ticketResult = new TicketResult();
                _service.GetBetListByRefnoAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                // Action
                var result = await controller.GetByRefno(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
            }
        }

        [Test]
        public async Task GetByRefno_WhenExport_ExportCorrectData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<RefNoBetListMetaModel>();
                metaModel.IsExport = true;

                var ticketResult = new TicketResult
                {
                    MainTicketList = new SortableList<LrfTicket>
                    {
                        FakeHelper.Create<LrfTicket>()
                    }
                };
                _service.GetBetListByRefnoAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var title = FakeHelper.Create<string>();
                _service.GetTitle(metaModel).Returns(title);

                var workbook = Substitute.For<IWorkbook>();
                _bettingTicketCenterExcel.Export(Arg.Any<BettingTicketCenterExcelModel>()).Returns(workbook);

                // Action
                await controller.GetByRefno(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    _bettingTicketCenterExcel.Received().Export(Arg.Is<BettingTicketCenterExcelModel>(x => !x.Option.ShowTotal
                       && !x.Option.ShowSumStake
                       && x.Option.ShowLicenseeWl
                       && x.Option.ShowWinLoss
                       && x.CustomerCommVisible == metaModel.HasPlayerCom
                       && x.Title == title));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public async Task GetByRunning_WhenGet_ReturnData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<BaseBettingTicketMetaModel>();
                metaModel.IsExport = false;

                var ticketResult = new TicketResult();
                _service.GetRunningBetListAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _ticketBuilderService.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom).Returns(ticketText);

                // Action
                var result = (await controller.GetByRunning(metaModel) as JsonResult).Data;
                var data = result.GetValue("Data");
                var total = result.GetValue("Total");
                var totalRows = result.GetValue("TotalRows");

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.EqualTo(ticketText));
                    Assert.That(
                        total.GetValue("SumStake"),
                        Is.EqualTo(ticketResult.SumStake.ToString(FormatKey.DecimalFormat)));
                    Assert.That(totalRows, Is.EqualTo(ticketResult.TotalRows));
                });
            }
        }

        [Test]
        public async Task GetByRunning_WhenExport_EmptyData_ReturnNoContentStatus()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<BaseBettingTicketMetaModel>();
                metaModel.IsExport = true;

                var ticketResult = new TicketResult();
                _service.GetRunningBetListAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                _service.GetTitle(metaModel).Returns(string.Empty);

                // Action
                var result = await controller.GetByRunning(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
            }
        }

        [Test]
        public async Task GetByRunning_WhenExport_ExportCorrectData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<BaseBettingTicketMetaModel>();
                metaModel.IsExport = true;

                var ticketResult = new TicketResult
                {
                    MainTicketList = new SortableList<LrfTicket>
                    {
                        FakeHelper.Create<LrfTicket>()
                    }
                };
                _service.GetRunningBetListAsync(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var title = FakeHelper.Create<string>();
                _service.GetTitle(metaModel).Returns(title);

                var workbook = Substitute.For<IWorkbook>();
                _bettingTicketCenterExcel.Export(Arg.Any<BettingTicketCenterExcelModel>()).Returns(workbook);

                // Action
                await controller.GetByRunning(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    _bettingTicketCenterExcel.Received().Export(Arg.Is<BettingTicketCenterExcelModel>(x => x.Option.ShowTotal
                       && x.Option.ShowSumStake
                       && !x.Option.ShowLicenseeWl
                       && !x.Option.ShowWinLoss
                       && x.Option.TotalText == $"{ViewResources.Totalin} {metaModel.BaseCurrencyName}"
                       && x.CustomerCommVisible == metaModel.HasPlayerCom
                       && x.Title == title));
                    AssertClearData(ticketResult);
                });
            }
        }

        private BettingTicketCenterController CreateController()
        {
            return new BettingTicketCenterController(
                _service,
                _bettingTicketCenterExcel, _ticketBuilderService);
        }
    }
}