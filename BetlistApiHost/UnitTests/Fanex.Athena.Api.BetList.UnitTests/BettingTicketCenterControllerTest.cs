namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Base.UnitTests;
    using Excel.BetList;
    using Excel.BetList.Model;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using NSubstitute;
    using NUnit.Framework;
    using Service.BetList.Interfaces;
    using Session.Model;
    using UnitTest.Common.Helpers;

    public class BettingTicketCenterControllerTest : BaseApiControllerHelperTest
    {
        private IBettingTicketCenterService _service;

        private IBettingTicketCenterExcel _bettingTicketCenterExcel;

        private BettingTicketCenterController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _service = Substitute.For<IBettingTicketCenterService>();
            _bettingTicketCenterExcel = Substitute.For<IBettingTicketCenterExcel>();

            SessionService.GenerateAgentListXml(Arg.Any<int>()).Returns("<xml></xml>");

            _controller = Substitute.ForPartsOf<BettingTicketCenterController>(
                SessionService,
                ProductService,
                LangService,
                _service,
                _bettingTicketCenterExcel);

            MockSessionData();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _service = null;
            _controller = null;
        }

        [TearDown]
        public void TearDown()
        {
            _service.ClearReceivedCalls();
            _bettingTicketCenterExcel.ClearReceivedCalls();
        }

        [TestCase(0)]
        [TestCase(-1)]
        [TestCase(-80)]
        public void GetByRefno_MetaModelIsInvalid_ReturnEmpty(int refNo)
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = refNo
            };

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public void GetByRefno_ValidMetaModel_GetDataFromDataAccess()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };

            _service.GetBetListByRefno(metaModel, Arg.Any<IDictionary<int, string>>()).Returns(new TicketResult());

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _service.Received(1).GetBetListByRefno(metaModel, Arg.Any<IDictionary<int, string>>());
                Assert.That(result.Data, Is.Not.Null);
            });
        }

        [Test]
        public void GetByRefno_Export_ExportWithCorrectParrameters()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>(),
                IsExport = true
            };
            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket());
            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _service.GetBetListByRefno(metaModel, Arg.Any<IDictionary<int, string>>()).Returns(ticketResult);

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            _bettingTicketCenterExcel.Received(1).Export(Arg.Is<BettingTicketCenterExcelModel>(x => !x.ShowTotal));
        }

        [Test]
        public void GetByRunning_DateRangeIsInvalid_ReturnEmpty()
        {
            // Arrange
            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(-1)
            };

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [TestCase(null)]
        [TestCase("")]
        [TestCase("  ")]
        public void GetByRunning_NoProduct_ReturnEmpty(string productCode)
        {
            // Arrange
            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = productCode
            };

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public void GetByRunning_InvalidCurrency_ReturnEmpty()
        {
            // Arrange
            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2"
            };
            _service.IsValidCurrencyId(Arg.Any<int>(), Arg.Any<IDictionary<int, string>>()).Returns(false);

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public void GetByRunning_ValidMetaModel_GetDataFromDataAccess()
        {
            // Arrange
            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 1
            };

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });

            var ticketResult = new TicketResult
            {
                SumStake = FakeHelper.Create<int>(),
                TotalRows = FakeHelper.Create<int>()
            };
            _service.GetRunningBetList(metaModel, Arg.Any<IDictionary<int, string>>()).Returns(ticketResult);
            _service.IsValidCurrencyId(Arg.Any<int>(), Arg.Any<IDictionary<int, string>>()).Returns(true);

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _service.Received(1).GetRunningBetList(metaModel, Arg.Any<IDictionary<int, string>>());
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(result.Total.SumStake, Is.EqualTo(ticketResult.SumStake.ToString(FormatKey.DecimalFormat)));
                Assert.That(result.TotalRows, Is.EqualTo(ticketResult.TotalRows));
            });
        }

        [Test]
        public void GetByRunning_Export_ExportWithCorrectParrameters()
        {
            // Arrange
            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 1,
                IsExport = true
            };

            var ticketResult = new TicketResult
            {
                SumStake = FakeHelper.Create<int>(),
                TotalRows = FakeHelper.Create<int>()
            };
            ticketResult.MainTicketList.Add(new LrfTicket());
            _service.IsValidCurrencyId(Arg.Any<int>(), Arg.Any<IDictionary<int, string>>()).Returns(true);
            _service.GetRunningBetList(metaModel, Arg.Any<IDictionary<int, string>>()).Returns(ticketResult);

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            _bettingTicketCenterExcel.Received(1).Export(Arg.Is<BettingTicketCenterExcelModel>(x => x.ShowTotal && x.ShowSumStake));
        }

        [Test]
        public void BuildTicketText_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };
            var ticketResult = new TicketResult();
            var ticket = new TicketText
            {
                Index = 1,
                ExternalId = "2"
            };

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _service.GetBetListByRefno(metaModel, SessionService.UserSession.CurrencyList).Returns(ticketResult);
            _service.BuildTicketText(ticketResult, metaModel, Arg.Any<bool>()).Returns(new List<TicketText> { ticket });

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data[0].Index, Is.EqualTo(1));
                Assert.That(result.Data[0].ExternalId, Is.EqualTo(ticket.ExternalId));
            });
        }

        [Test]
        public void GetBySingleCustomer_InvalidCurrency_ReturnEmpty()
        {
            // Arrange
            var metaModel = new SingleCustomersBetListMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 2,
                UserName = FakeHelper.Create<string>()
            };
            _service.IsValidCurrencyId(Arg.Any<int>(), Arg.Any<IDictionary<int, string>>()).Returns(false);

            // Action
            var result = _controller.GetBySingleCustomer(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public void GetBySingleCustomer_DateRangeIsInvalid_ReturnEmpty()
        {
            // Arrange
            var metaModel = new SingleCustomersBetListMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(-1),
                UserName = FakeHelper.Create<string>()
            };

            // Action
            var result = _controller.GetBySingleCustomer(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public void GetBySingleCustomer_UserNameIsEmpty_ReturnEmpty()
        {
            // Arrange
            var metaModel = new SingleCustomersBetListMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today
            };

            // Action
            var result = _controller.GetBySingleCustomer(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public void GetBySingleCustomer_ValidMetaModel_GetDataFromDataAccess()
        {
            // Arrange
            var metaModel = new SingleCustomersBetListMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 1,
                UserName = FakeHelper.Create<string>()
            };

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>(), IsAllowedCustomerComm = true });
            _service.IsValidCurrencyId(Arg.Any<int>(), Arg.Any<IDictionary<int, string>>()).Returns(true);

            var ticketResult = new TicketResult
            {
                SumWinLoss = FakeHelper.Create<decimal>(),
                SumLicenseeWL = FakeHelper.Create<decimal>(),
                SumComm = FakeHelper.Create<decimal>(),
                SumPlayerComm = FakeHelper.Create<decimal>(),
                SumStake = FakeHelper.Create<decimal>(),
                TotalRows = FakeHelper.Create<int>()
            };
            _service.GetBetListForSingleCustomer(metaModel, Arg.Any<IDictionary<int, string>>()).Returns(ticketResult);

            // Action
            var result = _controller.GetBySingleCustomer(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _service.Received(1).GetBetListForSingleCustomer(metaModel, Arg.Any<IDictionary<int, string>>());
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(result.Total.SumLicenseeWL, Is.EqualTo((ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat)));
                Assert.That(result.Total.SumStake, Is.EqualTo(ticketResult.SumStake.ToString(FormatKey.DecimalFormat)));
                Assert.That(result.TotalRows, Is.EqualTo(ticketResult.TotalRows));
            });
        }

        [Test]
        public void GetBySingleCustomer_Export_ExportWithCorrectParrameters()
        {
            // Arrange
            var metaModel = new SingleCustomersBetListMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 1,
                UserName = FakeHelper.Create<string>(),
                IsExport = true
            };

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>(), IsAllowedCustomerComm = true });
            _service.IsValidCurrencyId(Arg.Any<int>(), Arg.Any<IDictionary<int, string>>()).Returns(true);

            var ticketResult = new TicketResult();
            ticketResult.MainTicketList.Add(new LrfTicket());
            _service.GetBetListForSingleCustomer(metaModel, Arg.Any<IDictionary<int, string>>()).Returns(ticketResult);

            // Action
            var result = _controller.GetBySingleCustomer(metaModel);

            // Assert
            _bettingTicketCenterExcel.Received(1).Export(Arg.Is<BettingTicketCenterExcelModel>(
                 x => x.ShowTotal
                     && x.ShowSumStake
                     && x.ShowWinLoss
                     && x.ShowLicenseeWl));
        }
    }
}