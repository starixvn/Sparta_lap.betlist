namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.Configuration.Interfaces;
    using Data;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DailyStatementBetListDataAccessTest
    {
        private static readonly BalanceUp BalanceUp = new BalanceUp
        {
            MoveBetsDay = new DateTime(2016, 1, 1),
            BalanceUpDay = new DateTime(2016, 2, 1)
        };

        private BetListMetaModel _metaModel;

        private IObjectDb _objectDb;

        private IBalanceUpDataAccess _balanceUpDataAccess;

        private DailyStatementBetListDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _metaModel = CreateBetListMetaModel();
            _objectDb = Substitute.For<IObjectDb>();
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _dataAccess = Substitute.ForPartsOf<DailyStatementBetListDataAccess>(_balanceUpDataAccess);
            _balanceUpDataAccess.GetBalanceUp().Returns(BalanceUp);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetDailyStatementData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Statement_Mem")).Returns(_objectDb);

            // Action
            _dataAccess.GetDailyStatementData(_metaModel, true);

            // Assert
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, _metaModel.CustId);
            _objectDb.Received().AddInParameter("FromDate", DbType.DateTime, _metaModel.FromDate);
            _objectDb.Received().AddInParameter("ToDate", DbType.DateTime, _metaModel.ToDate);
            _objectDb.Received().AddInParameter("MoveBetsDay", DbType.DateTime, BalanceUp.MoveBetsDay);
            _objectDb.Received().AddInParameter("BalanceUpDay", DbType.DateTime, BalanceUp.BalanceUpDay);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, _metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex", DbType.Int32, _metaModel.PageIndex);
            _objectDb.Received().AddOutParameter("totalrows", DbType.Int32, 4);
        }

        [Test]
        public void GetDailyStatementData_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new BetListMetaModel();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Statement_Mem")).Returns(_objectDb);
            var reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            ConfigureReaderForGetDailyStatementData(reader, _metaModel);
            var expected = GetDailyStatementData(_metaModel);

            // Action
            var result = _dataAccess.GetDailyStatementData(_metaModel, true);
            var firstItem = result.First();

            // Assert
            Assert.That(firstItem.TransType, Is.EqualTo(expected.TransType));
            Assert.That(firstItem.CustId, Is.EqualTo(expected.CustId));
            Assert.That(firstItem.ExternalId, Is.EqualTo(expected.ExternalId));
            Assert.That(firstItem.SystemId, Is.EqualTo(expected.SystemId));
            Assert.That(firstItem.FullStatementDate, Is.EqualTo(expected.FullStatementDate));
            Assert.That(firstItem.TransTypeHasBetList, Is.EqualTo(expected.TransTypeHasBetList));
            Assert.That(firstItem.Index, Is.EqualTo(expected.Index));
            Assert.That(firstItem.Date, Is.EqualTo(expected.Date));
            Assert.That(firstItem.Remark, Is.EqualTo(expected.Remark));
            Assert.That(firstItem.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(firstItem.CreditDebit, Is.EqualTo(expected.CreditDebit));
            Assert.That(firstItem.Commission, Is.EqualTo(expected.Commission));
            Assert.That(firstItem.Balance, Is.EqualTo(expected.Balance));
        }

        private static BetListMetaModel CreateBetListMetaModel()
        {
            return new BetListMetaModel
            {
                BaseCurrency = "BaseCurrencyTest",
                CustId = 1,
                FromDate = new DateTime(2016, 8, 10),
                ToDate = new DateTime(2016, 8, 19),
                PageSize = 10,
                PageIndex = 1,
                ExternalId = "ExternalIdTest",
                SystemId = "SystemIdTest",
            };
        }

        private static void ConfigureReaderForGetDailyStatementData(IDataReader reader, BetListMetaModel metaModel)
        {
            var commissionColumn = "commission" + metaModel.BaseCurrency;
            var totalAmtColumn = "totalamt" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;

            reader["winlostdate"].Returns(new DateTime(2016, 8, 19));
            reader["transtype"].Returns("com");
            reader["transdesc"].Returns("betting statement");
            reader[commissionColumn].Returns(10.5m);
            reader[turnoverColumn].Returns(11.5m);
            reader[totalAmtColumn].Returns(1200.5m);
        }

        private static DailyStatement GetDailyStatementData(BetListMetaModel metaModel)
        {
            return new DailyStatement
            {
                TransType = "1",
                CustId = metaModel.CustId,
                ExternalId = metaModel.ExternalId,
                SystemId = metaModel.SystemId,
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