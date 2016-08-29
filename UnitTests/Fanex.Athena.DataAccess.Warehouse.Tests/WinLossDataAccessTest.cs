namespace Fanex.Athena.DataAccess.Warehouse.Tests
{
    using System.Data;
    using Data;
    using Model.Warehouse.DTO;
    using Model.Warehouse.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class WinLossDataAccessTest
    {
        private IObjectDb _objectDb;
        private WinLossDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<WinLossDataAccess>();
            _reader = Substitute.For<IDataReader>();

            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_rpt_DWWinloss")).Returns(_objectDb);
            _objectDb.ExecuteReader(Arg.Any<object>()).Returns(_reader);
        }

        [Test]
        public void GetDataList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var metaModel = new WinLossMetaModel { ReportBy = 1 };
            var expected = new WinLoss
            {
                ReportBy = 1,
                BetCount = 2,
                GroupByValue = "GroupByValueTest",
                MemberWinLoss = 10m,
                LicenseeWinloss = 20m,
                Turnover = 30m,
                Liveindicator = false,
                BettypeId = 0
            };
            ConfigureReaderForGetDataListMethod(expected);

            // Action
            var result = _dataAccess.GetDataList(metaModel)[0];

            // Assert
            Assert.That(result.ReportBy, Is.EqualTo(expected.ReportBy));
            Assert.That(result.BetCount, Is.EqualTo(expected.BetCount));
            Assert.That(result.GroupByValue, Is.EqualTo(expected.GroupByValue));
            Assert.That(result.MemberWinLoss, Is.EqualTo(expected.MemberWinLoss));
            Assert.That(result.LicenseeWinloss, Is.EqualTo(expected.LicenseeWinloss));
            Assert.That(result.Turnover, Is.EqualTo(expected.Turnover));
            Assert.That(result.Liveindicator, Is.EqualTo(expected.Liveindicator));
            Assert.That(result.BettypeId, Is.EqualTo(expected.BettypeId));
        }

        private void ConfigureReaderForGetDataListMethod(WinLoss expected)
        {
            _reader.Read().Returns(true, false);

            _reader["betcount"].Returns(expected.BetCount);
            _reader["MemberWinLost"].Returns(expected.MemberWinLoss);
            _reader["LicenseeWL"].Returns(expected.LicenseeWinloss);
            _reader["Turnover"].Returns(expected.Turnover);
            _reader["liveindicator"].Returns(1);
            _reader["date"].Returns(expected.GroupByValue);
        }
    }
}