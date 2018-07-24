namespace Fanex.Athena.Api.BetList.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Base.UnitTests;
    using Constant.DataDomain;
    using DataAccess.BetList.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Excel.BetList;
    using Excel.BetList.Builders;
    using Excel.BetList.Model;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DataDomain;
    using Model.Core.DTO.Configuration;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using Session.Model;
    using UnitTest.Common.Helpers;
    using VendorAdapter.Interfaces;

    public class BettingTicketCenterControllerTest : BaseApiControllerHelperTest
    {
        private IBalanceUpDataAccess _balanceUpDataAccess;

        private IBettingTicketCenterDataAccess _dataAccess;

        private IBetListHTMLBuilder _betlistHtmlBuilder;

        private ICacheRefAdapter _cacheRefAdapter;

        private ITicketHelper _ticketHelper;

        private BettingTicketCenterController _controller;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _dataAccess = Substitute.For<IBettingTicketCenterDataAccess>();
            _betlistHtmlBuilder = Substitute.For<IBetListHTMLBuilder>();
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            _ticketHelper = Substitute.For<ITicketHelper>();

            var balanceUp = new BalanceUp { BalanceUpDay = new DateTime(2018, 1, 1), MoveBetsDay = new DateTime(2018, 1, 2) };
            _balanceUpDataAccess.GetBalanceUp().Returns(balanceUp);
            SessionService.GenerateAgentListXml(Arg.Any<int>()).Returns("<xml></xml>");

            _controller = Substitute.ForPartsOf<BettingTicketCenterController>(
                SessionService,
                ProductService,
                LangService,
                _balanceUpDataAccess,
                _dataAccess,
                _betlistHtmlBuilder,
                _cacheRefAdapter,
                _ticketHelper);
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _balanceUpDataAccess = null;
            _dataAccess = null;
            _betlistHtmlBuilder = null;
            _cacheRefAdapter = null;
            _ticketHelper = null;
            _controller = null;
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

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _dataAccess.GetBetListByRefno(metaModel).Returns(new TicketResult());

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received(1).GetBetListByRefno(metaModel);
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

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _dataAccess.GetBetListByRefno(metaModel).Returns(new TicketResult());

            var excel = SetUpForExporttingExcel();

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            excel.Received(1).Export(Arg.Is<BettingTicketCenterExcelModel>(x => !x.ShowTotal));
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
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var metaModel = new BaseBettingTicketMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 2
            };

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
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var ticketResult = new TicketResult
            {
                SumStake = FakeHelper.Create<int>(),
                TotalRows = FakeHelper.Create<int>()
            };
            _dataAccess.GetRunningBetList(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received(1).GetRunningBetList(metaModel);
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

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var excel = SetUpForExporttingExcel();

            var ticketResult = new TicketResult
            {
                SumStake = FakeHelper.Create<int>(),
                TotalRows = FakeHelper.Create<int>()
            };
            _dataAccess.GetRunningBetList(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByRunning(metaModel);

            // Assert
            excel.Received(1).Export(Arg.Is<BettingTicketCenterExcelModel>(x => x.ShowTotal && x.ShowSumStake));
        }

        [Test]
        public void BuildTicketText_IsNotCombinationMixParlay_ReturnNormalOddText()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };
            var ticketResult = new TicketResult();
            var ticket = new LrfTicket { BetTypeId = (int)BetType.Handicap, Status = BetStatus.Won };
            ticketResult.MainTicketList.Add(ticket);

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _dataAccess.GetBetListByRefno(metaModel).Returns(ticketResult);
            _ticketHelper.ClientDetailSystemParlayFunction.Returns(string.Empty);

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.Empty);
        }

        [Test]
        public void BuildTicketText_IsCombinationMixParlay_ReturnCustomOddText()
        {
            // Arrange
            var metaModel = new RefNoBetListMetaModel
            {
                RefNo = FakeHelper.Create<int>()
            };
            var ticketResult = new TicketResult();
            var ticket = new LrfTicket { BetTypeId = (int)BetType.CombinationMixParlay, Status = BetStatus.Lose };
            ticketResult.MainTicketList.Add(ticket);

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _dataAccess.GetBetListByRefno(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.That(_ticketHelper.ClientDetailSystemParlayFunction, Is.Not.Empty);
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
            var ticket = new LrfTicket
            {
                BetTypeId = (int)BetType.OverUnder,
                Status = BetStatus.Lose,
                ExternalId = FakeHelper.Create<string>(),
                UserName = FakeHelper.Create<string>()
            };
            ticketResult.MainTicketList.Add(ticket);

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>() });
            _dataAccess.GetBetListByRefno(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByRefno(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data[0].Index, Is.EqualTo(1));
                Assert.That(result.Data[0].ExternalId, Is.EqualTo(ticket.ExternalId));
                Assert.That(result.Data[0].SystemId, Is.EqualTo(ticket.UserName));
            });
        }

        [Test]
        public void GetBySingleCustomer_InvalidCurrency_ReturnEmpty()
        {
            // Arrange
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var metaModel = new SingleCustomersBetListMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 2,
                UserName = FakeHelper.Create<string>()
            };

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
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var ticketResult = new TicketResult
            {
                SumWinLoss = FakeHelper.Create<decimal>(),
                SumLicenseeWL = FakeHelper.Create<decimal>(),
                SumComm = FakeHelper.Create<decimal>(),
                SumPlayerComm = FakeHelper.Create<decimal>(),
                SumStake = FakeHelper.Create<decimal>(),
                TotalRows = FakeHelper.Create<int>()
            };
            _dataAccess.GetBetListForSingleCustomer(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetBySingleCustomer(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received(1).GetBetListForSingleCustomer(metaModel);
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(
                    result.Total.SumWinLoss.Contains(ticketResult.SumWinLoss.ToString(FormatKey.DecimalFormat))
                        && result.Total.SumWinLoss.Contains(ticketResult.SumPlayerComm.ToString(FormatKey.DecimalFormat)),
                    Is.True);
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
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var excel = SetUpForExporttingExcel();

            var ticketResult = new TicketResult
            {
                SumWinLoss = FakeHelper.Create<decimal>(),
                SumLicenseeWL = FakeHelper.Create<decimal>(),
                SumComm = FakeHelper.Create<decimal>(),
                SumPlayerComm = FakeHelper.Create<decimal>(),
                SumStake = FakeHelper.Create<decimal>(),
                TotalRows = FakeHelper.Create<int>()
            };
            _dataAccess.GetBetListForSingleCustomer(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetBySingleCustomer(metaModel);

            // Assert
            excel.Received(1).Export(Arg.Is<BettingTicketCenterExcelModel>(
                 x => x.ShowTotal
                     && x.ShowSumStake
                     && x.ShowWinLoss
                     && x.ShowLicenseeWl));
        }

        [Test]
        public void GetByMultipleCustomers_InvalidCurrency_ReturnEmpty()
        {
            // Arrange
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var metaModel = new MultipleCustomersBetListMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 2,
                UserName = FakeHelper.Create<string>()
            };

            // Action
            var result = _controller.GetByMultipleCustomers(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public void GetByMultipleCustomers_DateRangeIsInvalid_ReturnEmpty()
        {
            // Arrange
            var metaModel = new MultipleCustomersBetListMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today.AddDays(-1),
                UserName = FakeHelper.Create<string>()
            };

            // Action
            var result = _controller.GetByMultipleCustomers(metaModel);

            // Assert
            Assert.That(result.Data, Is.Empty);
        }

        [Test]
        public void GetByMultipleCustomers_ValidMetaModel_GetDataFromDataAccess()
        {
            // Arrange
            var metaModel = new MultipleCustomersBetListMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 1,
                UserName = FakeHelper.Create<string>()
            };

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>(), IsAllowedCustomerComm = true });
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var ticketResult = new TicketResult
            {
                SumWinLoss = FakeHelper.Create<decimal>(),
                SumLicenseeWL = FakeHelper.Create<decimal>(),
                SumComm = FakeHelper.Create<decimal>(),
                SumPlayerComm = FakeHelper.Create<decimal>(),
                SumStake = FakeHelper.Create<decimal>(),
                TotalRows = FakeHelper.Create<int>()
            };
            _dataAccess.GetBetListForMultipleCustomers(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByMultipleCustomers(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                _dataAccess.Received(1).GetBetListForMultipleCustomers(metaModel);
                Assert.That(result.Data, Is.Not.Null);
                Assert.That(
                    result.Total.SumWinLoss.Contains(ticketResult.SumWinLoss.ToString(FormatKey.DecimalFormat))
                        && result.Total.SumWinLoss.Contains(ticketResult.SumPlayerComm.ToString(FormatKey.DecimalFormat)),
                    Is.True);
                Assert.That(result.Total.SumLicenseeWL, Is.EqualTo((ticketResult.SumLicenseeWL + ticketResult.SumComm).ToString(FormatKey.DecimalFormat)));
                Assert.That(result.Total.SumStake, Is.EqualTo(ticketResult.SumStake.ToString(FormatKey.DecimalFormat)));
                Assert.That(result.TotalRows, Is.EqualTo(ticketResult.TotalRows));
            });
        }

        [Test]
        public void GetByMultipleCustomers_Export_ExportWithCorrectParrameters()
        {
            // Arrange
            var metaModel = new MultipleCustomersBetListMetaModel
            {
                FromDate = DateTime.Today,
                ToDate = DateTime.Today,
                ProductCodeList = "C1,C2",
                CurrencyId = 1,
                UserName = FakeHelper.Create<string>(),
                IsExport = true
            };

            SessionService.LicenseeSession.Returns(new LicenseeSession { LicenseeId = FakeHelper.Create<int>(), IsAllowedCustomerComm = true });
            SessionService.UserSession.Returns(new UserSession
            {
                CurrencyList = new Dictionary<int, string>
                {
                    { 1, "Currency" }
                }
            });

            var excel = SetUpForExporttingExcel();

            var ticketResult = new TicketResult
            {
                SumWinLoss = FakeHelper.Create<decimal>(),
                SumLicenseeWL = FakeHelper.Create<decimal>(),
                SumComm = FakeHelper.Create<decimal>(),
                SumPlayerComm = FakeHelper.Create<decimal>(),
                SumStake = FakeHelper.Create<decimal>(),
                TotalRows = FakeHelper.Create<int>()
            };
            _dataAccess.GetBetListForMultipleCustomers(metaModel).Returns(ticketResult);

            // Action
            var result = _controller.GetByMultipleCustomers(metaModel);

            // Assert
            excel.Received(1).Export(Arg.Is<BettingTicketCenterExcelModel>(
                 x => x.ShowTotal
                     && x.ShowSumStake
                     && x.ShowWinLoss
                     && x.ShowLicenseeWl));
        }

        private BettingTicketCenterExcel SetUpForExporttingExcel()
        {
            var excel = Substitute.ForPartsOf<BettingTicketCenterExcel>(Substitute.For<IBettingTicketCenterExcelBuilder>());
            excel.When(x => x.Export(Arg.Any<BettingTicketCenterExcelModel>())).DoNotCallBase();
            _controller.CreateExcel().Returns(excel);
            _controller.When(x => x.ExcelStream(Arg.Any<IWorkbook>(), Arg.Any<string>())).DoNotCallBase();

            return excel;
        }
    }
}