namespace Fanex.Athena.BetList.Controller.UnitTests
{
    using System.Linq;
    using System.Net;
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
    using Service.Interfaces;
    using UnitTest.Common.Helpers;

    public class BettingTicketCenterControllerTest : BaseControllerHelperTest
    {
        private IBettingTicketCenterService _service;

        private IBettingTicketCenterExcel _bettingTicketCenterExcel;

        [OneTimeSetUp]
        public void FixtureSetUp()
        {
            _service = Substitute.For<IBettingTicketCenterService>();
            _bettingTicketCenterExcel = Substitute.For<IBettingTicketCenterExcel>();
        }

        [OneTimeTearDown]
        public void FixtureTearDown()
        {
            _service = null;
            _bettingTicketCenterExcel = null;
        }

        [Test]
        public void GetBySingleCustomer_WhenGet_ReturnData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<SingleCustomersBetListMetaModel>();
                metaModel.IsExport = false;

                var ticketResult = new TicketResult();
                _service.GetBetListForSingleCustomer(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _service.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom).Returns(ticketText);

                var sumWL = FakeHelper.Create<string>();
                _service.BuildSumWinLoss(ticketResult, metaModel.HasPlayerCom).Returns(sumWL);

                // Action
                var result = (controller.GetBySingleCustomer(metaModel) as JsonResult).Data;
                var data = result.GetType().GetProperty("Data").GetValue(result, null);
                var total = result.GetType().GetProperty("Total").GetValue(result, null);
                var totalRows = result.GetType().GetProperty("TotalRows").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.EqualTo(ticketText));
                    Assert.That(total.GetType().GetProperty("SumWinLoss").GetValue(total, null), Is.EqualTo(sumWL));
                    Assert.That(
                        total.GetType().GetProperty("SumLicenseeWL").GetValue(total, null),
                        Is.EqualTo((ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat)));
                    Assert.That(
                        total.GetType().GetProperty("SumStake").GetValue(total, null),
                        Is.EqualTo(ticketResult.SumStake.ToString(FormatKey.DecimalFormat)));
                    Assert.That(totalRows, Is.EqualTo(ticketResult.TotalRows));
                });
            }
        }

        [Test]
        public void GetBySingleCustomer_WhenExport_EmptyData_ReturnNoContentStatus()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<SingleCustomersBetListMetaModel>();
                metaModel.IsExport = true;

                var ticketResult = new TicketResult();
                _service.GetBetListForSingleCustomer(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                _service.GetTitle(metaModel).Returns(string.Empty);

                // Action
                var result = controller.GetBySingleCustomer(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
            }
        }

        [Test]
        public void GetBySingleCustomer_WhenExport_ExportCorrectData()
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
                _service.GetBetListForSingleCustomer(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var title = FakeHelper.Create<string>();
                _service.GetTitle(metaModel).Returns(title);

                var workbook = Substitute.For<IWorkbook>();
                _bettingTicketCenterExcel.Export(Arg.Any<BettingTicketCenterExcelModel>()).Returns(workbook);

                // Action
                controller.GetBySingleCustomer(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    _bettingTicketCenterExcel.Received().Export(Arg.Is<BettingTicketCenterExcelModel>(x => x.ShowTotal
                       && x.ShowSumStake
                       && x.ShowLicenseeWl
                       && x.ShowWinLoss
                       && x.CustomerCommVisible == metaModel.HasPlayerCom
                       && x.Title == title));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public void GetByRefno_WhenGet_ReturnData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<RefNoBetListMetaModel>();
                metaModel.IsExport = false;

                var ticketResult = new TicketResult();
                _service.GetBetListByRefno(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _service.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom).Returns(ticketText);

                // Action
                var result = (controller.GetByRefno(metaModel) as JsonResult).Data;
                var data = result.GetType().GetProperty("Data").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.EqualTo(ticketText));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public void GetByRefno_WhenExport_EmptyData_ReturnNoContentStatus()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<RefNoBetListMetaModel>();
                metaModel.IsExport = true;

                var ticketResult = new TicketResult();
                _service.GetBetListByRefno(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                // Action
                var result = controller.GetByRefno(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
            }
        }

        [Test]
        public void GetByRefno_WhenExport_ExportCorrectData()
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
                _service.GetBetListByRefno(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var title = FakeHelper.Create<string>();
                _service.GetTitle(metaModel).Returns(title);

                var workbook = Substitute.For<IWorkbook>();
                _bettingTicketCenterExcel.Export(Arg.Any<BettingTicketCenterExcelModel>()).Returns(workbook);

                // Action
                controller.GetByRefno(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    _bettingTicketCenterExcel.Received().Export(Arg.Is<BettingTicketCenterExcelModel>(x => !x.ShowTotal
                       && !x.ShowSumStake
                       && !x.ShowLicenseeWl
                       && !x.ShowWinLoss
                       && x.CustomerCommVisible == metaModel.HasPlayerCom
                       && x.Title == title));
                    AssertClearData(ticketResult);
                });
            }
        }

        [Test]
        public void GetByRunning_WhenGet_ReturnData()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<BaseBettingTicketMetaModel>();
                metaModel.IsExport = false;

                var ticketResult = new TicketResult();
                _service.GetRunningBetList(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var ticketText = FakeHelper.CreateMany<TicketText>(2).ToList();
                _service.BuildTicketText(ticketResult, metaModel, metaModel.HasPlayerCom).Returns(ticketText);

                // Action
                var result = (controller.GetByRunning(metaModel) as JsonResult).Data;
                var data = result.GetType().GetProperty("Data").GetValue(result, null);
                var total = result.GetType().GetProperty("Total").GetValue(result, null);
                var totalRows = result.GetType().GetProperty("TotalRows").GetValue(result, null);

                // Assert
                Assert.Multiple(() =>
                {
                    Assert.That(data, Is.EqualTo(ticketText));
                    Assert.That(
                        total.GetType().GetProperty("SumStake").GetValue(total, null),
                        Is.EqualTo(ticketResult.SumStake.ToString(FormatKey.DecimalFormat)));
                    Assert.That(totalRows, Is.EqualTo(ticketResult.TotalRows));
                });
            }
        }

        [Test]
        public void GetByRunning_WhenExport_EmptyData_ReturnNoContentStatus()
        {
            using (var controller = CreateController())
            {
                // Assert
                var metaModel = FakeHelper.Create<BaseBettingTicketMetaModel>();
                metaModel.IsExport = true;

                var ticketResult = new TicketResult();
                _service.GetRunningBetList(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                _service.GetTitle(metaModel).Returns(string.Empty);

                // Action
                var result = controller.GetByRunning(metaModel) as HttpStatusCodeResult;

                // Assert
                Assert.That(result.StatusCode, Is.EqualTo((int)HttpStatusCode.NoContent));
            }
        }

        [Test]
        public void GetByRunning_WhenExport_ExportCorrectData()
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
                _service.GetRunningBetList(metaModel, metaModel.UserCurrencyList).Returns(ticketResult);

                var title = FakeHelper.Create<string>();
                _service.GetTitle(metaModel).Returns(title);

                var workbook = Substitute.For<IWorkbook>();
                _bettingTicketCenterExcel.Export(Arg.Any<BettingTicketCenterExcelModel>()).Returns(workbook);

                // Action
                controller.GetByRunning(metaModel);

                // Assert
                Assert.Multiple(() =>
                {
                    _bettingTicketCenterExcel.Received().Export(Arg.Is<BettingTicketCenterExcelModel>(x => x.ShowTotal
                       && x.ShowSumStake
                       && !x.ShowLicenseeWl
                       && !x.ShowWinLoss
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
                _bettingTicketCenterExcel);
        }
    }
}