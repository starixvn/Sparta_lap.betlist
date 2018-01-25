namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Core.LiveCasino.DTO;
    using Result.Core.Racing.DTO;
    using Result.Core.Sportsbook.DTO;
    using UnitTest.Common.Helpers;

    public partial class ResultDataAccessTest
    {
        [TestCase(-2, null)]
        [TestCase(-1, null)]
        [TestCase(0, "0")]
        [TestCase(1, "1")]
        public void GetPopUpMatches_WhenCall_ParametersPassedCorrectly(int raceNo, string expectedRaceNo)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpMatchesSp)).Returns(ObjectDb);
            dynamic result = null;
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(x => result = x));

            var expectedResult = new
            {
                matchid = MatchId,
                raceno = expectedRaceNo,
                bettype = BettypeId,
                betid = BetId
            };

            // Action
            DataAccess.GetPopUpMatches<Sportsbook>(MatchId, raceNo, BettypeId, BetId);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [TestCase(null, "0")]
        [TestCase(1, "1")]
        public void GetPopUpMatches_TypeIsSportsbook_ReturnCorrectData(int? betId, string expectedBetId)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpMatchesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            Reader.FieldCount.Returns(3);
            Reader.GetName(0).Returns("lcolor");
            Reader.GetName(1).Returns("LiveAwayScore");
            Reader.GetName(2).Returns("LiveHomeScore");

            var dataMapping = GetReaderDataMappingForSportsbookType();
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            MapReaderData(dataMapping);

            var expectedData = GetExpectedSportsbookData();
            expectedData.BetID = expectedBetId;
            var expectedResult = new List<Sportsbook> { expectedData };

            // Action
            var result = DataAccess.GetPopUpMatches<Sportsbook>(MatchId, RaceNo, BettypeId, betId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GGetPopUpMatches_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetPopUpMatchesSp,
                () => DataAccess.GetPopUpMatches<Sportsbook>(MatchId, RaceNo, BettypeId, BetId));
        }

        [Test]
        public void GetPopUpMatches_TypeIsSportsbookWithSomeReturnedNullValues_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpMatchesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("lcolor");

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["betstatus"] = DBNull.Value;
            dataMapping["lcolor"] = "Color";
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            dataMappingForGameDetail["ResultStatusID"] = DBNull.Value;
            dataMappingForGameDetail["SettlementStatusID"] = DBNull.Value;
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            MapReaderData(dataMapping);

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.BetStatus = string.Empty;
            sportsbookData.Color = "Color";
            sportsbookData.LiveAwayScore = null;
            sportsbookData.LiveHomeScore = null;
            sportsbookData.GameDetails[0].ResultStatusID = 0;
            sportsbookData.GameDetails[0].SettlementStatusID = 0;
            var expectedResult = new List<Sportsbook> { sportsbookData };

            // Action
            var result = DataAccess.GetPopUpMatches<Sportsbook>(MatchId, RaceNo, BettypeId, BetId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpMatches_TypeIsSportsbookWithSomeReturnedNullValuesAndColumnColorNotExist_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpMatchesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            Reader.FieldCount.Returns(0);

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["betstatus"] = DBNull.Value;
            dataMapping["lcolor"] = DBNull.Value;
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            dataMappingForGameDetail["ResultStatusID"] = DBNull.Value;
            dataMappingForGameDetail["SettlementStatusID"] = DBNull.Value;
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(Reader, dataMapping);

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.BetStatus = string.Empty;
            sportsbookData.Color = string.Empty;
            sportsbookData.LiveAwayScore = null;
            sportsbookData.LiveHomeScore = null;
            sportsbookData.GameDetails[0].ResultStatusID = 0;
            sportsbookData.GameDetails[0].SettlementStatusID = 0;
            var expectedResult = new List<Sportsbook> { sportsbookData };

            // Action
            var result = DataAccess.GetPopUpMatches<Sportsbook>(MatchId, RaceNo, BettypeId, BetId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpMatches_TypeIsLiveCasino_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpMatchesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "showtime", "showtime" }, { "leagueid", 1 }, { "eventstatus", "win" }, { "finalawayscore", 2 }, { "finalhomescore", 3 },
                { "eventdate", "03/17/2017" }, { "IsPending", true }, { "kickofftime", "01/01/2017" }, { "IsHTCal", true }, { "Calculate", true },
                { "betstatus", "betstatus" }, { "homeid2", 4 }
            };
            MapReaderData(readerDataMapping);

            var expectedResult = new List<LiveCasino>
            {
                new LiveCasino
                {
                    ShowTime = "showtime",
                    LeagueId = 1,
                    EventStatus = "win",
                    FinalAwayScore = 2,
                    FinalHomeScore = 3,
                    EventDate = new DateTime(2017, 3, 17),
                    IsPending = true,
                    KickOffTime = new DateTime(2017, 1, 1),
                    IsHTCal = true,
                    Calculate = true,
                    BetStatus = "betstatus",
                    Bettype = 4
                }
            };

            // Action
            var result = DataAccess.GetPopUpMatches<LiveCasino>(MatchId, RaceNo, BettypeId, BetId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpMatches_TypeIsLiveCasinoWithSomeReturnedNullValues_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpMatchesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "showtime", "showtime" }, { "leagueid", 1 }, { "eventstatus", "win" }, { "finalawayscore", 2 }, { "finalhomescore", 3 },
                { "eventdate", "03/17/2017" }, { "IsPending", true }, { "kickofftime", "01/01/2017" }, { "IsHTCal", true }, { "Calculate", true },
                { "betstatus", DBNull.Value }, { "homeid2", 4 }
            };
            MapReaderData(readerDataMapping);

            var expectedResult = new List<LiveCasino>
            {
                new LiveCasino
                {
                    ShowTime = "showtime",
                    LeagueId = 1,
                    EventStatus = "win",
                    FinalAwayScore = 2,
                    FinalHomeScore = 3,
                    EventDate = new DateTime(2017, 3, 17),
                    IsPending = true,
                    KickOffTime = new DateTime(2017, 1, 1),
                    IsHTCal = true,
                    Calculate = true,
                    BetStatus = string.Empty,
                    Bettype = 4
                }
            };

            // Action
            var result = DataAccess.GetPopUpMatches<LiveCasino>(MatchId, RaceNo, BettypeId, BetId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpMatches_TypeIsRacing_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpMatchesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "meetingid", 1 }, { "HorseId", "HorseId" }, { "HorseNo", "HorseNo" }, { "IsFavor", "1" }, { "IsSCR", "1" },
                { "place", 2 }, { "placeodds", 10m }, { "odds", 0.2m }, { "racename", 3 }, { "kickofftime", "03/17/2017" },
                { "rowcount", 10 }, { "SCR", "SCR" }, { "TF", "TF" }
            };
            MapReaderData(readerDataMapping);

            var expectedResult = new List<Racing>
            {
                new Racing
                {
                    LeagueId = 1,
                    EngineId = "HorseId",
                    EngineNo = "HorseNo",
                    IsFavor = true,
                    IsSCR = true,
                    Place = 2,
                    PlaceOdds = 10m,
                    WinOdds = 0.2m,
                    RaceName = 3,
                    KickOffTime = new DateTime(2017, 3, 17),
                    Favourite = "1",
                    RowCount = 10,
                    SCROfMatch = "SCR",
                    TFOfMatch = "TF"
                }
            };

            // Action
            var result = DataAccess.GetPopUpMatches<Racing>(MatchId, RaceNo, BettypeId, BetId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpMatches_TypeIsRacingWithSomeReturnedNullValues_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpMatchesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "meetingid", 1 }, { "HorseId", "HorseId" }, { "HorseNo", "HorseNo" }, { "IsFavor", DBNull.Value }, { "IsSCR", DBNull.Value },
                { "place", 2 }, { "placeodds", 10m }, { "odds", 0.2m }, { "racename", 3 }, { "kickofftime", DBNull.Value },
                { "rowcount", 10 }, { "SCR", "SCR" }, { "TF", "TF" }
            };
            MapReaderData(readerDataMapping);

            var expectedResult = new List<Racing>
            {
                new Racing
                {
                    LeagueId = 1,
                    EngineId = "HorseId",
                    EngineNo = "HorseNo",
                    IsFavor = false,
                    IsSCR = false,
                    Place = 2,
                    PlaceOdds = 10m,
                    WinOdds = 0.2m,
                    RaceName = 3,
                    KickOffTime = DateTime.MinValue,
                    Favourite = string.Empty,
                    RowCount = 10,
                    SCROfMatch = "SCR",
                    TFOfMatch = "TF"
                }
            };

            // Action
            var result = DataAccess.GetPopUpMatches<Racing>(MatchId, RaceNo, BettypeId, BetId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpMatches_TypeIsNotSupported_ThrowException()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpMatchesSp)).Returns(ObjectDb);

            // Action
            TestDelegate result = () => DataAccess.GetPopUpMatches<object>(MatchId, RaceNo, BettypeId, BetId);

            // Assert
            Assert.Throws<NotSupportedException>(result, "The type 'object' is not supported.");
        }
    }
}