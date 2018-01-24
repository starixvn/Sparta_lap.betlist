namespace Fanex.Athena.Service.Report.UnitTests
{
    using System;
    using DataAccess.Report.Interfaces;
    using Fanex.Result.Core.LiveCasino.DTO;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Result;

    public class ResultServiceTest
    {
        private IResultDataAccess _dataAccess;
        private ResultService _service;

        [SetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IResultDataAccess>();
            _service = new ResultService(_dataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetOutRight_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultMetalModel();

            // Action
            _service.GetOutRight(metaModel);

            // Assert
            _dataAccess.Received().GetOutRight(metaModel);
        }

        [Test]
        public void GetSports_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int LicenseeId = 1;
            var date = new DateTime(2018, 1, 1);

            // Action
            _service.GetSports(date, LicenseeId);

            // Assert
            _dataAccess.Received().GetSports(date, LicenseeId);
        }

        [Test]
        public void GetLeagues_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int LicenseeId = 1;
            var date = new DateTime(2018, 1, 1);

            // Action
            _service.GetLeagues(date, LicenseeId);

            // Assert
            _dataAccess.Received().GetLeagues(date, LicenseeId);
        }

        [Test]
        public void GetMatches_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultMetalModel();

            // Action
            _service.GetMatches(metaModel);

            // Assert
            _dataAccess.Received().GetMatches<Sportsbook>(metaModel);
        }

        [Test]
        public void GetLiveCasinoMatches_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultMetalModel();

            // Action
            _service.GetLiveCasinoMatches(metaModel);

            // Assert
            _dataAccess.Received().GetMatches<LiveCasino>(metaModel);
        }

        [Test]
        public void GetNumberGameMatches_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultMetalModel();

            // Action
            _service.GetNumberGameMatches(metaModel);

            // Assert
            _dataAccess.Received().GetNumberGameMatches(metaModel);
        }

        [Test]
        public void GetRacingMatches_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var metaModel = new ResultMetalModel();

            // Action
            _service.GetRacingMatches(metaModel);

            // Assert
            _dataAccess.Received().GetRacingMatches(metaModel);
        }

        [Test]
        public void GetSportsbookGameDetail_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int MatchId = 1;

            // Action
            _service.GetSportsbookGameDetail(MatchId);

            // Assert
            _dataAccess.Received().GetSportsbookGameDetail(MatchId);
        }

        [Test]
        public void GetPopUpMatch_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int MatchId = 1;
            const int BettypeId = 2;
            const long BetId = 3;

            // Action
            _service.GetPopUpMatch(MatchId, BettypeId, BetId);

            // Assert
            _dataAccess.Received().GetPopUpMatches<Sportsbook>(MatchId, -1, BettypeId, BetId);
        }

        [Test]
        public void GetPopUpLiveCasinoMatch_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int MatchId = 1;
            const int BettypeId = 2;
            const long BetId = 3;

            // Action
            _service.GetPopUpLiveCasinoMatch(MatchId, BettypeId, BetId);

            // Assert
            _dataAccess.Received().GetPopUpMatches<LiveCasino>(MatchId, -1, BettypeId, BetId);
        }

        [Test]
        public void GetPopUpRacingMatch_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int MatchId = 1;
            const int RaceNo = 2;
            const int BettypeId = 3;
            const long BetId = 4;

            // Action
            _service.GetPopUpRacingMatch(MatchId, RaceNo, BettypeId, BetId);

            // Assert
            _dataAccess.Received().GetPopUpMatches<Racing>(MatchId, RaceNo, BettypeId, BetId);
        }

        [Test]
        public void GetPopUpParlayMatch_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const long RefNo = 4;

            // Action
            _service.GetPopUpParlayMatch(RefNo);

            // Assert
            _dataAccess.Received().GetPopUpParalayMatch(RefNo);
        }

        [Test]
        public void GetPopUpNumberGameMatchs_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            const int MatchId = 1;

            // Action
            _service.GetPopUpNumberGameMatchs(MatchId);

            // Assert
            _dataAccess.Received().GetPopUpNumberGameMatch(MatchId);
        }
    }
}