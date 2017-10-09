namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Data;
    using Data;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public partial class BetListDataAccessTest
    {
        private readonly BetListMetaModel _metaModel;
        private IObjectDb _objectDb;
        private BetListDataAccess _dataAccess;

        public BetListDataAccessTest()
        {
            _metaModel = GetBetListMetaModel();
        }

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<BetListDataAccess>();
        }

        [TearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetMixParlayDetailTicket_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_SelParlayDetail")).Returns(_objectDb);
            long transId = 1;

            // Action
            _dataAccess.GetMixParlayDetailTicket(transId);

            // Assert
            _objectDb.Received().AddInParameter("TransId", DbType.Int64, transId);
        }

        [Test]
        public void GetSystemParlayDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_BetList_SelCombinationParlayDetail")).Returns(_objectDb);
            var metaModel = GetSystemParlayDetailMetaModel();

            var dataset = new DataSet();
            _objectDb.ExecuteDataSet().Returns(dataset);

            // Action
            _dataAccess.GetSystemParlayDetail(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("refno", DbType.Int64, metaModel.RefNo);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("balanceupday", DbType.DateTime, metaModel.BalanceUpDay);
            _objectDb.Received().AddOutParameter("outstanding", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("outstandingeur", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("outstandingusd", DbType.Currency, 8);
            _objectDb.Received().AddOutParameter("outstandinggbp", DbType.Currency, 8);
        }

        [Test]
        public void GetDailyStatementData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Statement_Mem")).Returns(_objectDb);

            // Action
            _dataAccess.GetDailyStatementData(_metaModel, true);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, _metaModel.MoveBetsDay);
            _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, _metaModel.BalanceUpDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
        }

        [Test]
        public void GetDailyStatementData_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Statement_Mem")).Returns(_objectDb);
            var reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            ConfigureReaderForGetDailyStatementData(reader, _metaModel);
            var expected = GetDailyStatementData(_metaModel);

            // Action
            var result = _dataAccess.GetDailyStatementData(_metaModel, true)[0];

            // Assert
            Assert.That(result.TransType, Is.EqualTo(expected.TransType));
            Assert.That(result.CustId, Is.EqualTo(expected.CustId));
            Assert.That(result.ExternalId, Is.EqualTo(expected.ExternalId));
            Assert.That(result.SystemId, Is.EqualTo(expected.SystemId));
            Assert.That(result.FullStatementDate, Is.EqualTo(expected.FullStatementDate));
            Assert.That(result.TransTypeHasBetList, Is.EqualTo(expected.TransTypeHasBetList));
            Assert.That(result.Index, Is.EqualTo(expected.Index));
            Assert.That(result.Date, Is.EqualTo(expected.Date));
            Assert.That(result.Remark, Is.EqualTo(expected.Remark));
            Assert.That(result.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(result.CreditDebit, Is.EqualTo(expected.CreditDebit));
            Assert.That(result.Commission, Is.EqualTo(expected.Commission));
            Assert.That(result.Balance, Is.EqualTo(expected.Balance));
        }

        private static SystemParlayDetailMetaModel GetSystemParlayDetailMetaModel()
        {
            return new SystemParlayDetailMetaModel
            {
                RefNo = 1,
                UserId = 2,
                LicenseeId = 3,
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 18)
            };
        }

        private static BetListMetaModel GetBetListMetaModel()
        {
            return new BetListMetaModel
            {
                BaseCurrency = "BaseCurrencyTest",
                CustId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 19),
                MoveBetsDay = new DateTime(2016, 1, 1),
                BalanceUpDay = new DateTime(2016, 2, 1),
                PageSize = 10,
                PageIndex = 1,
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest",
                LicenseeId = 2,
                UserId = 3,
                CurrencyId = 4,
                ProductCodeList = "SB,NG",
                SortBy = "SortByTest",
                SortOrder = "SortOrderTest",
                LeagueId = 5,
                RaceNo = "RaceNoTest",
                DrawFilter = 6,
                MultipleBettype = "MultipleBettypeTest",
                LiveIndicator = 7,
                PlatformType = 9
            };
        }

        private void ConfigureReaderForGetDailyStatementData(IDataReader reader, BetListMetaModel _metaModel)
        {
            var commissionColumn = "commission" + _metaModel.BaseCurrency;
            var totalAmtColumn = "totalamt" + _metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + _metaModel.BaseCurrency;

            reader["winlostdate"].Returns(new DateTime(2016, 8, 19));
            reader["transtype"].Returns("com");
            reader["transdesc"].Returns("betting statement");
            reader[commissionColumn].Returns(10.5m);
            reader[turnoverColumn].Returns(11.5m);
            reader[totalAmtColumn].Returns(1200.5m);
        }

        private DailyStatement GetDailyStatementData(BetListMetaModel _metaModel)
        {
            return new DailyStatement
            {
                TransType = "1",
                CustId = _metaModel.CustId,
                ExternalId = _metaModel.ExternalId,
                SystemId = _metaModel.SystemId,
                FullStatementDate = "Friday <br/> 8/19/2016 12:00:00 AM",
                TransTypeHasBetList = true,
                Index = 1,
                Date = "8/19/2016",
                Remark = "<span class='bl-underdog'>COM <br/>- betting statement</span>",
                Turnover = "<span class='bl-underdog'>11.50</span>",
                CreditDebit = "<span class='bl-underdog'>1,200.50</span>",
                Commission = "<span class='bl-underdog'>10.50</span>",
                Balance = "<span class='bl-underdog'>1,211.00</span>",
            };
        }
    }
}