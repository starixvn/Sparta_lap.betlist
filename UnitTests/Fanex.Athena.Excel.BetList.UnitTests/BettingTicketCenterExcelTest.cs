using Fanex.Athena.Excel.BetList.Builders;
using Fanex.Athena.Excel.BetList.Model;
using Fanex.Athena.Model.BetList.DTO;
using Fanex.Athena.Resources;
using Fanex.Athena.UnitTest.Common.Helpers;
using NPOI.SS.UserModel;
using NSubstitute;
using NUnit.Framework;

namespace Fanex.Athena.Excel.BetList.UnitTests
{
    [TestFixture]
    internal class BettingTicketCenterExcelTest
    {
        private IBettingTicketCenterExcelBuilder _builder;
        private BettingTicketCenterExcel _excel;

        [SetUp]
        public void Setup()
        {
            _builder = Substitute.For<IBettingTicketCenterExcelBuilder>();
            _excel = Substitute.ForPartsOf<BettingTicketCenterExcel>(_builder);
        }

        [Test]
        public void Export_HasData_CallRelevantMethods()
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket());
            var model = new BettingTicketCenterExcelModel
            {
                Data = ticketResult
            };
            var workbook = Substitute.For<IWorkbook>();
            _builder.GetWorkbook().Returns(workbook);
            _excel.When(x => x.BuildSheets(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            // Action
            var result = _excel.Export(model);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).InitializeFonts();
                _builder.Received(1).InitializeCellStyles();
                _excel.Received(1).BuildSheets(model);
                Assert.That(result, Is.SameAs(workbook));
            });
        }

        [Test]
        public void Export_NoData_CallRelevantMethods()
        {
            // Arrange
            var model = new BettingTicketCenterExcelModel
            {
                Title = FakeHelper.Create<string>(),
                CustomerCommVisible = true,
                Data = new TicketResult()
            };
            var workbook = Substitute.For<IWorkbook>();
            _builder.GetWorkbook().Returns(workbook);
            _excel.When(x => x.BuildSheets(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            // Action
            var result = _excel.Export(model);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).CreateSheet(ViewResources.BettingTicketCenter);
                _builder.Received(1).AddTitle(model.Title);
                _builder.Received(1).AddHeader(model.CustomerCommVisible);
                _builder.Received(1).AddNoInfoRow();
                Assert.That(result, Is.SameAs(workbook));
            });
        }

        [Test]
        public void BuildSheets_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket());
            var model = new BettingTicketCenterExcelModel
            {
                Title = FakeHelper.Create<string>(),
                Data = ticketResult
            };

            _excel.When(x => x.AssignSheetData(Arg.Any<BettingTicketCenterExcelModel>(), Arg.Any<int>(), Arg.Any<int>())).DoNotCallBase();
            _excel.When(x => x.AddFooter(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            // Action
            _excel.BuildSheets(model);

            // Assert
            Assert.Multiple(() =>
            {
                _builder.Received(1).CreateSheet(ViewResources.Page + "1");
                _builder.Received(1).AddTitle(model.Title);
                _builder.Received(1).AddHeader(model.CustomerCommVisible);
                _excel.Received(1).AssignSheetData(model, 0, 1);
                _builder.Received(1).GetWorkbook();
            });
        }

        [TestCase(false)]
        [TestCase(true)]
        public void BuildSheets_ShowTotal_CallAddFooter(bool isShowTotal)
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket());
            var model = new BettingTicketCenterExcelModel
            {
                ShowTotal = isShowTotal,
                Title = FakeHelper.Create<string>(),
                Data = ticketResult
            };

            _excel.When(x => x.AssignSheetData(Arg.Any<BettingTicketCenterExcelModel>(), Arg.Any<int>(), Arg.Any<int>())).DoNotCallBase();
            _excel.When(x => x.AddFooter(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();

            // Action
            _excel.BuildSheets(model);

            // Assert
            if (isShowTotal)
            {
                _excel.Received(1).AddFooter(model);
            }
            else
            {
                _excel.DidNotReceive().AddFooter(model);
            }
        }

        [Test]
        public void AssignSheetData_WhenCall_CallRelevantMethods()
        {
            // Arrange
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket());
            var model = new BettingTicketCenterExcelModel
            {
                Data = ticketResult,
                LicenseeId = FakeHelper.Create<int>()
            };

            // Action
            _excel.AssignSheetData(model, 0, 1);

            // Assert
            _builder.Received(1).AssignRowData(model.Data, model.Data.MainTicketList[0], model.CustomerCommVisible, model.LicenseeId);
        }

        [TestCase(false, false, false, false)]
        [TestCase(false, true, false, false)]
        [TestCase(false, false, true, false)]
        [TestCase(true, true, true, true)]
        public void AddFooter_WhenCall_CallRelevantMethods(bool isShowSumStake, bool isShowSumWL, bool isShowPlayerComm, bool isShowSumLicenseeWL)
        {
            // Arrange
            const decimal SumPlayerComm = 0.12m;
            const decimal SumComm = 0.34m;
            var sumStake = FakeHelper.Create<decimal>();
            var sumWL = FakeHelper.Create<decimal>();
            var sumLicenseeWL = FakeHelper.Create<decimal>();
            var model = new BettingTicketCenterExcelModel
            {
                ShowSumStake = isShowSumStake,
                ShowWinLoss = isShowSumWL,
                ShowLicenseeWl = isShowSumLicenseeWL,
                CustomerCommVisible = isShowPlayerComm,
                Data = new TicketResult
                {
                    SumStake = sumStake,
                    SumWinLoss = sumWL,
                    SumPlayerComm = SumPlayerComm,
                    SumLicenseeWL = sumLicenseeWL,
                    SumComm = SumComm
                }
            };
            var expectedSumStake = isShowSumStake ? (decimal?)sumStake : null;
            var expectedSumWL = isShowSumWL ? (decimal?)sumWL : null;
            var expectedSumPlayerComm = isShowSumWL && isShowPlayerComm ? (decimal?)SumPlayerComm : null;
            var expectedSumLicenseeWL = isShowSumLicenseeWL ? (decimal?)(sumLicenseeWL + SumComm) : null;

            // Action
            _excel.AddFooter(model);

            // Assert
            _builder.Received(1).AddFooter(expectedSumStake, expectedSumWL, expectedSumPlayerComm, expectedSumLicenseeWL);
        }
    }
}