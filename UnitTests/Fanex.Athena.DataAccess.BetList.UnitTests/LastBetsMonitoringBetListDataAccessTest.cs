namespace Fanex.Athena.DataAccess.BetList.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.BetList.DTO;
    using Model.BetList.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LastBetsMonitoringBetListDataAccessTest
    {
        private IObjectDb _objectDb;
        private LastBetsMonitoringBetListDataAccess _dataAccess;

        [SetUp]
        public void SetUp()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<LastBetsMonitoringBetListDataAccess>();
        }

        [Test]
        public void GetLastBetsMonitoringData_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Monitor_BetlistSel_New")).Returns(_objectDb);
            var metaModel = GetLastBetsMonitoringMetaModel();

            // Action
            _dataAccess.GetLastBetsMonitoringData(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("userid", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
            _objectDb.Received().AddInParameter("BetType", DbType.String, metaModel.BetType);
            _objectDb.Received().AddInParameter("Currency", DbType.String, metaModel.Currency);
            _objectDb.Received().AddInParameter("SpecialLevel", DbType.String, metaModel.Level);
            _objectDb.Received().AddInParameter("TransId", DbType.Int64, metaModel.TransId);
            _objectDb.Received().AddInParameter("LimitRowCount", DbType.Int32, metaModel.PageSize);
            _objectDb.Received().AddInParameter("SportId", DbType.Int32, metaModel.SportId);
            _objectDb.Received().AddInParameter("LeagueId", DbType.Int32, metaModel.LeagueId);
            _objectDb.Received().AddInParameter("MatchId", DbType.Int32, metaModel.MatchId);
            _objectDb.Received().AddInParameter("MinStake", DbType.Decimal, metaModel.StakeFrom);
            _objectDb.Received().AddInParameter("MaxStake", DbType.Decimal, metaModel.StakeTo);
            _objectDb.Received().AddInParameter("LiveIndicator", DbType.Int32, metaModel.LiveStatus);
            _objectDb.Received().AddInParameter("BaseCurrency", DbType.String, metaModel.BasedCurrency);
            _objectDb.Received().AddInParameter("BetFrom", DbType.Byte, metaModel.BetFrom);
        }

        [Test]
        public void GetMatchList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Any<string>()).Returns(_objectDb);
            var expected = new BetListMatch { MatchId = 1, LeagueId = 2 };
            _objectDb.Query<BetListMatch>().Returns(new List<BetListMatch> { expected });

            // Action
            var result = _dataAccess.GetMatchList("SPNameTest")[0];

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        private LastBetsMonitoringMetaModel GetLastBetsMonitoringMetaModel()
        {
            return new LastBetsMonitoringMetaModel
            {
                UserId = 1,
                LicenseeId = 2,
                LicenseeType = "LicenseeTypeTest",
                BetType = "BetTypeTest",
                Currency = "CurrencyTest",
                Level = "LevelTest",
                TransId = 3,
                PageSize = 4,
                SportId = 5,
                LeagueId = 6,
                MatchId = 7,
                StakeFrom = 10,
                StakeTo = 20,
                LiveStatus = 8,
                BasedCurrency = "BasedCurrencyTest",
                BetFrom = 9
            };
        }
    }
}