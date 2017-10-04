namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.Configuration.Interfaces;
    using Data;
    using Fanex.Athena.UnitTest.Common.Extensions;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Core.LiveCasino.DTO;
    using Result.Core.NumberGame.DTO;
    using Result.Core.Racing.DTO;
    using Result.Core.Sportsbook.DTO;
    using UnitTest.Common.Helpers;

    [TestFixture]
    public class ResultDataAccessTest
    {
        private ResultDataAccess _dataAccess;
        private BalanceUp _balance;
        private ResultMetalModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };
            balanceUpDataAccess.GetBalanceUp().Returns(_balance);

            _dataAccess = Substitute.ForPartsOf<ResultDataAccess>(balanceUpDataAccess);

            _metaModel = new ResultMetalModel
            {
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2)
            };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _metaModel = null;
            _balance = null;
        }

        [Test]
        public void GetOutRight_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Result_Outright")).Returns(objectDb);
            dynamic param = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(arg => param = arg));
            var expectedParam = new
            {
                fromdate = _metaModel.FromDate,
                todate = _metaModel.ToDate,
                isshowall = true
            };

            // Action
            _dataAccess.GetOutRight(_metaModel);

            // Assert
            Assert.That(param, Tobe.SameProperties(expectedParam));
        }

        [Test]
        public void GetOutRight_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Result_Outright")).Returns(objectDb);

            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "eventdate", "03/16/2017" },
                { "sporttype", 1 },
                { "leagueid", 2 },
                { "teamid", 3 },
                { "status", "win" }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            var outrights = new List<Outright>
            {
                new Outright
                {
                    EventDate = new DateTime(2017, 3, 16),
                    SportType = 1,
                    LeagueId = 2,
                    TeamId = 3,
                    Status = "win"
                }
            };

            // Action
            var result = _dataAccess.GetOutRight(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(outrights.Count));
                Assert.That(result[0], Tobe.SameProperties(outrights[0]));
            });
        }

        [Test]
        public void GetSports_ParameterPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Result_SportSelByDate")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader().Returns(reader);

            var date = new DateTime(2017, 3, 16);
            var licenseeId = 1;

            // Action
            _dataAccess.GetSports(date, licenseeId);

            // Assert
            Assert.Multiple(() =>
            {
                objectDb.Received().AddInParameter("eventdate", DbType.DateTime, date);
                objectDb.Received().AddInParameter("licenseeid", DbType.Int32, licenseeId);
                objectDb.Received().AddInParameter("movebetsday", DbType.DateTime, _balance.MoveBetsDay);
            });
        }

        [Test]
        public void GetSports_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Result_SportSelByDate")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            var expectedResult = new List<Sport>
            {
                new Sport("sport1")
            };

            var readerDataMapping = new Dictionary<string, object>
            {
                { "sporttype", "sport1" }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);
            var date = new DateTime(2017, 3, 16);
            var licenseeId = 1;

            // Action
            var result = _dataAccess.GetSports(date, licenseeId);

            // Assert
            Assert.That(result[0].SportId, Is.EqualTo(expectedResult[0].SportId));
        }

        [Test]
        public void GetLeagues_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Result_LeagueSelByDate")).Returns(objectDb);
            dynamic param = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(arg => param = arg));

            var date = new DateTime(2017, 3, 16);
            var sportId = 1;
            var expectedResult = new
            {
                eventdate = date,
                sporttype = sportId,
                isshowall = true,
                movebetsday = _balance.MoveBetsDay
            };

            // Action
            _dataAccess.GetLeagues(date, sportId);
            var result = param;

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetLeagues_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Result_LeagueSelByDate")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false);

            var expectedResult = new List<League>
            {
                new League { LeagueId = 1 }
            };

            var readerDataMapping = new Dictionary<string, object>
            {
                { "leagueid", 1 }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            var date = new DateTime(2017, 3, 16);
            var sportId = 1;

            // Action
            var result = _dataAccess.GetLeagues(date, sportId);

            // Assert
            Assert.That(result[0].LeagueId, Is.EqualTo(expectedResult[0].LeagueId));
        }

        [Test]
        public void GetNumberGameMatches_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelNumberGameByLeague")).Returns(objectDb);
            _metaModel.LeagueId = 1;

            dynamic result = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(x => result = x));
            var expectedResult = new
            {
                fromdate = _metaModel.FromDate,
                leagueid = _metaModel.LeagueId,
                isshowall = true
            };

            // Action
            _dataAccess.GetNumberGameMatches(_metaModel);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetNumberGameMatches_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelNumberGameByLeague")).Returns(objectDb);
            _metaModel.LeagueId = 1;
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, true, false);
            reader.FieldCount.Returns(1);
            reader.GetName(0).Returns("isrefund");
            var readerDataMapping = new Dictionary<string, object>
            {
                { "leagueid", 1 }, { "matchid", 2 }, { "eventstatus", "win" }, { "eventdate", "03/16/2017" }, { "isrefund", "1" },
                { "kickofftime", "1/1/2017" }, { "matchcode", "test" }, { "seq", 3 }, { "number", 4 }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            var expectedResult = new List<NumberGame>
            {
                new NumberGame
                {
                    LeagueId = 1,
                    MatchId = 2,
                    EventStatus = "win",
                    EventDate = new DateTime(2017, 3, 16),
                    IsPending = true,
                    KickOffTime = new DateTime(2017, 1, 1),
                    MatchCode = "test",
                    Seg = new List<int> { 3, 3 },
                    Ball = new List<int> { 4, 4 }
                }
            };

            // Action
            var result = _dataAccess.GetNumberGameMatches(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetRacingMatches_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_RaceSelByMeeting")).Returns(objectDb);
            dynamic param = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(x => param = x));
            _metaModel.SportId = 1;
            _metaModel.LeagueId = 2;
            var expectedResult = new
            {
                fromdate = _metaModel.FromDate,
                leagueid = _metaModel.LeagueId,
                sporttype = _metaModel.SportId,
                isshowall = true
            };

            // Action
            _dataAccess.GetRacingMatches(_metaModel);
            var result = param;

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetRacingMatches_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_RaceSelByMeeting")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "meetingid", 1 }, { "HorseId", "1" }, { "HorseNo", "2" }, { "IsFavor", "1" }, { "IsSCR", "1" },
                { "place", 2 }, { "placeodds", 10.5m }, { "odds", 0.5m }, { "racename", 4 }, { "kickofftime", "03/16/2017" },
                { "rowcount", 4 }, { "SCR", "SCR" }, { "TF", "TF" }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            var expectedResult = GetExpectedRacingData();

            // Action
            var result = _dataAccess.GetRacingMatches(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpMatches_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResult")).Returns(objectDb);
            dynamic result = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(x => result = x));
            var matchId = 1;
            var raceNo = 2;
            var bettypeId = 5;
            long? betId = 6;

            var expectedResult = new
            {
                matchid = matchId,
                raceno = raceNo,
                bettype = bettypeId,
                betid = betId
            };

            // Action
            _dataAccess.GetPopUpMatches<Sportsbook>(matchId, raceNo, bettypeId, betId);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetPopUpMatches_TypeIsSportsbook_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResult")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);

            reader.FieldCount.Returns(3);
            reader.GetName(0).Returns("lcolor");
            reader.GetName(1).Returns("LiveAwayScore");
            reader.GetName(2).Returns("LiveHomeScore");

            var dataMapping = GetReaderDataMappingForSportsbookType();
            var dataMappingForGameDetail = GetReaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(reader, dataMapping);
            var matchId = 1;
            var raceNo = 2;
            var bettypeId = 5;
            long? betId = 6;

            var expectedResult = new List<Sportsbook> { GetExpectedSportsbookData() };

            // Action
            var result = _dataAccess.GetPopUpMatches<Sportsbook>(matchId, raceNo, bettypeId, betId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpMatches_TypeIsSportsbookWithSomeReturnedNullValues_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResult")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);

            reader.FieldCount.Returns(1);
            reader.GetName(0).Returns("lcolor");

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["betstatus"] = DBNull.Value;
            dataMapping["lcolor"] = DBNull.Value;
            var dataMappingForGameDetail = GetReaderDataMappingForGameDetail();
            dataMappingForGameDetail["ResultStatusID"] = DBNull.Value;
            dataMappingForGameDetail["SettlementStatusID"] = DBNull.Value;
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(reader, dataMapping);
            int matchId = 1, raceNo = 2, bettypeId = 5;
            long? betId = 6;

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.BetStatus = string.Empty;
            sportsbookData.Color = string.Empty;
            sportsbookData.LiveAwayScore = null;
            sportsbookData.LiveHomeScore = null;
            sportsbookData.GameDetails[0].ResultStatusID = 0;
            sportsbookData.GameDetails[0].SettlementStatusID = 0;
            var expectedResult = new List<Sportsbook> { sportsbookData };

            // Action
            var result = _dataAccess.GetPopUpMatches<Sportsbook>(matchId, raceNo, bettypeId, betId);

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
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResult")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);

            reader.FieldCount.Returns(0);

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["betstatus"] = DBNull.Value;
            dataMapping["lcolor"] = DBNull.Value;
            var dataMappingForGameDetail = GetReaderDataMappingForGameDetail();
            dataMappingForGameDetail["ResultStatusID"] = DBNull.Value;
            dataMappingForGameDetail["SettlementStatusID"] = DBNull.Value;
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(reader, dataMapping);
            int matchId = 1, raceNo = 2, bettypeId = 5;
            long? betId = 6;

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.BetStatus = string.Empty;
            sportsbookData.Color = string.Empty;
            sportsbookData.LiveAwayScore = null;
            sportsbookData.LiveHomeScore = null;
            sportsbookData.GameDetails[0].ResultStatusID = 0;
            sportsbookData.GameDetails[0].SettlementStatusID = 0;
            var expectedResult = new List<Sportsbook> { sportsbookData };

            // Action
            var result = _dataAccess.GetPopUpMatches<Sportsbook>(matchId, raceNo, bettypeId, betId);

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
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResult")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "showtime", "showtime" }, { "leagueid", 1 }, { "eventstatus", "win" }, { "finalawayscore", 2 }, { "finalhomescore", 3 },
                { "eventdate", "03/17/2017" }, { "IsPending", true }, { "kickofftime", "01/01/2017" }, { "IsHTCal", true }, { "Calculate", true },
                { "betstatus", "betstatus" }, { "homeid2", 4 }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            int matchId = 1, raceNo = 2, bettypeId = 5;
            long? betId = 6;

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
            var result = _dataAccess.GetPopUpMatches<LiveCasino>(matchId, raceNo, bettypeId, betId);

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
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResult")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "showtime", "showtime" }, { "leagueid", 1 }, { "eventstatus", "win" }, { "finalawayscore", 2 }, { "finalhomescore", 3 },
                { "eventdate", "03/17/2017" }, { "IsPending", true }, { "kickofftime", "01/01/2017" }, { "IsHTCal", true }, { "Calculate", true },
                { "betstatus", DBNull.Value }, { "homeid2", 4 }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            int matchId = 1, raceNo = 2, bettypeId = 5;
            long? betId = 6;

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
            var result = _dataAccess.GetPopUpMatches<LiveCasino>(matchId, raceNo, bettypeId, betId);

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
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResult")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "meetingid", 1 }, { "HorseId", "HorseId" }, { "HorseNo", "HorseNo" }, { "IsFavor", "1" }, { "IsSCR", "1" },
                { "place", 2 }, { "placeodds", 10m }, { "odds", 0.2m }, { "racename", 3 }, { "kickofftime", "03/17/2017" },
                { "rowcount", 10 }, { "SCR", "SCR" }, { "TF", "TF" }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            int matchId = 1, raceNo = 2, bettypeId = 5;
            long? betId = 6;

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
            var result = _dataAccess.GetPopUpMatches<Racing>(matchId, raceNo, bettypeId, betId);

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
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResult")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "meetingid", 1 }, { "HorseId", "HorseId" }, { "HorseNo", "HorseNo" }, { "IsFavor", DBNull.Value }, { "IsSCR", DBNull.Value },
                { "place", 2 }, { "placeodds", 10m }, { "odds", 0.2m }, { "racename", 3 }, { "kickofftime", DBNull.Value },
                { "rowcount", 10 }, { "SCR", "SCR" }, { "TF", "TF" }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            int matchId = 1, raceNo = 2, bettypeId = 5;
            long? betId = 6;

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
            var result = _dataAccess.GetPopUpMatches<Racing>(matchId, raceNo, bettypeId, betId);

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
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResult")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);

            int matchId = 1, raceNo = 2, bettypeId = 5;
            long? betId = 6;

            // Action
            TestDelegate result = () => _dataAccess.GetPopUpMatches<object>(matchId, raceNo, bettypeId, betId);

            // Assert
            Assert.Throws<NotSupportedException>(result, "The type 'object' is not supported.");
        }

        [Test]
        public void GetPopUpParalayMatch_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResultParlay")).Returns(objectDb);
            dynamic result = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(x => result = x));
            var refNo = 1;
            var expected = new
            {
                refno = refNo
            };

            // Action
            _dataAccess.GetPopUpParalayMatch(refNo);

            // Assert
            Assert.That(result, Tobe.SameProperties(expected));
        }

        [Test]
        public void GetPopUpParalayMatch_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResultParlay")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);
            reader.FieldCount.Returns(3);
            reader.GetName(0).Returns("lcolor");
            reader.GetName(1).Returns("bettype");
            reader.GetName(2).Returns("betcheck");
            var refNo = 1;

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["LeagueID"] = 11;
            dataMapping["sporttype"] = 12;
            dataMapping["bettype"] = 5;
            dataMapping["betcheck"] = "6";
            var dataMappingForGameDetail = GetReaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(reader, dataMapping);

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.LeagueId = 11;
            sportsbookData.RefNo = "1";
            sportsbookData.LiveHomeScore = null;
            sportsbookData.LiveAwayScore = null;

            var expectedResult = new List<Parlay>
            {
                new Parlay
                {
                    LeagueId = sportsbookData.LeagueId,
                    SportId = 12,
                    Sportbook = new List<Sportsbook> { sportsbookData }
                }
            };

            // Action
            var result = _dataAccess.GetPopUpParalayMatch(refNo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpParalayMatch_SomeColumnsAreNotExist_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResultParlay")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);
            var refNo = 1;

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["LeagueID"] = 11;
            dataMapping["sporttype"] = 12;
            var dataMappingForGameDetail = GetReaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(reader, dataMapping);

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.LeagueId = 11;
            sportsbookData.RefNo = "1";
            sportsbookData.Bettype = 0;
            sportsbookData.BetID = string.Empty;
            sportsbookData.Color = string.Empty;
            sportsbookData.LiveHomeScore = null;
            sportsbookData.LiveAwayScore = null;

            var expectedResult = new List<Parlay>
            {
                new Parlay
                {
                    LeagueId = sportsbookData.LeagueId,
                    SportId = 12,
                    Sportbook = new List<Sportsbook> { sportsbookData }
                }
            };

            // Action
            var result = _dataAccess.GetPopUpParalayMatch(refNo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpParalayMatch_SomeColumnsReturnedNullValue_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResultParlay")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);
            reader.FieldCount.Returns(1);
            reader.GetName(0).Returns("lcolor");
            var refNo = 1;

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["LeagueID"] = 11;
            dataMapping["sporttype"] = 12;
            dataMapping["betstatus"] = DBNull.Value;
            dataMapping["lcolor"] = DBNull.Value;

            var dataMappingForGameDetail = GetReaderDataMappingForGameDetail();
            dataMappingForGameDetail["ResultStatusID"] = DBNull.Value;
            dataMappingForGameDetail["SettlementStatusID"] = DBNull.Value;
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(reader, dataMapping);

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.LeagueId = 11;
            sportsbookData.RefNo = "1";
            sportsbookData.Bettype = 0;
            sportsbookData.BetID = string.Empty;
            sportsbookData.Color = string.Empty;
            sportsbookData.BetStatus = string.Empty;
            sportsbookData.LiveHomeScore = null;
            sportsbookData.LiveAwayScore = null;
            sportsbookData.GameDetails[0].ResultStatusID = 0;
            sportsbookData.GameDetails[0].SettlementStatusID = 0;

            var parlay = new Parlay { LeagueId = sportsbookData.LeagueId, SportId = 12, Sportbook = new List<Sportsbook> { sportsbookData } };
            var expectedResult = new List<Parlay> { parlay };

            // Action
            var result = _dataAccess.GetPopUpParalayMatch(refNo);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpParalayMatch_ParlayItemHasMultipleSportsbookItems_ReturnCorrectLengthOfSportsbookList()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResultParlay")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, true, false, true, false);
            reader.NextResult().Returns(true);
            var refNo = 1;

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["LeagueID"] = 11;
            dataMapping["sporttype"] = 12;
            var dataMappingForGameDetail = GetReaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(reader, dataMapping);

            // Action
            var result = _dataAccess.GetPopUpParalayMatch(refNo);

            // Assert
            Assert.That(result[0].Sportbook.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetPopUpNumberGameMatch_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResultNumberGame")).Returns(objectDb);
            dynamic result = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(x => result = x));
            var matchId = 1;
            var expected = new
            {
                matchid = matchId
            };

            // Action
            _dataAccess.GetPopUpNumberGameMatch(matchId);

            // Assert
            Assert.That(result, Tobe.SameProperties(expected));
        }

        [Test]
        public void GetPopUpNumberGameMatch_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResultNumberGame")).Returns(objectDb);
            var matchId = 2;
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, true, false);
            reader.FieldCount.Returns(1);
            reader.GetName(0).Returns("isrefund");
            var readerDataMapping = new Dictionary<string, object>
            {
                { "leagueid", 1 }, { "matchid", matchId }, { "eventstatus", "win" }, { "eventdate", "03/16/2017" }, { "isrefund", "1" },
                { "kickofftime", "1/1/2017" }, { "matchcode", "test" }, { "seq", 3 }, { "number", 4 }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            var expectedResult = new List<NumberGame>
            {
                new NumberGame
                {
                    LeagueId = 1,
                    MatchId = 2,
                    EventStatus = "win",
                    EventDate = new DateTime(2017, 3, 16),
                    IsPending = true,
                    KickOffTime = new DateTime(2017, 1, 1),
                    MatchCode = "test",
                    Seg = new List<int> { 3, 3 },
                    Ball = new List<int> { 4, 4 }
                }
            };

            // Action
            var result = _dataAccess.GetPopUpNumberGameMatch(matchId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpNumberGameMatch_IsRefundColumnNotExist_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResultNumberGame")).Returns(objectDb);
            var matchId = 2;
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, true, false);
            var readerDataMapping = new Dictionary<string, object>
            {
                { "leagueid", 1 }, { "matchid", matchId }, { "eventstatus", "win" }, { "eventdate", "03/16/2017" }, { "isrefund", "1" },
                { "kickofftime", "1/1/2017" }, { "matchcode", "test" }, { "seq", 3 }, { "number", 4 }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

            var expectedResult = new List<NumberGame>
            {
                new NumberGame
                {
                    LeagueId = 1,
                    MatchId = 2,
                    EventStatus = "win",
                    EventDate = new DateTime(2017, 3, 16),
                    IsPending = false,
                    KickOffTime = new DateTime(2017, 1, 1),
                    MatchCode = "test",
                    Seg = new List<int> { 3, 3 },
                    Ball = new List<int> { 4, 4 }
                }
            };

            // Action
            var result = _dataAccess.GetPopUpNumberGameMatch(matchId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetMatches_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelByLeague")).Returns(objectDb);
            dynamic param = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(x => param = x));
            _metaModel.SportId = 1;
            _metaModel.LeagueId = 2;

            var expected = new
            {
                fromdate = _metaModel.FromDate,
                sporttype = _metaModel.SportId,
                leagueid = _metaModel.LeagueId,
                movebetsday = _balance.MoveBetsDay
            };

            // Action
            _dataAccess.GetMatches<Sportsbook>(_metaModel);

            // Assert
            Assert.That(param, Tobe.SameProperties(expected));
        }

        [Test]
        public void GetMatches_TypeIsSportsbook_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelByLeague")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);

            reader.FieldCount.Returns(3);
            reader.GetName(0).Returns("lcolor");
            reader.GetName(1).Returns("LiveAwayScore");
            reader.GetName(2).Returns("LiveHomeScore");

            var dataMapping = GetReaderDataMappingForSportsbookType();
            var dataMappingForGameDetail = GetReaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(reader, dataMapping);

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.BetID = "0";
            sportsbookData.Bettype = 0;
            var expectedResult = new List<Sportsbook> { sportsbookData };

            // Action
            var result = _dataAccess.GetMatches<Sportsbook>(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetMatches_TypeIsLiveCasino_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelByLeague")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);
            reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>
            {
                { "showtime", "showtime" }, { "leagueid", 1 }, { "eventstatus", "win" }, { "finalawayscore", 2 }, { "finalhomescore", 3 },
                { "eventdate", "03/17/2017" }, { "IsPending", true }, { "kickofftime", "01/01/2017" }, { "IsHTCal", true }, { "Calculate", true },
                { "betstatus", "betstatus" }, { "homeid2", 4 }
            };
            DataAccessTestHelper.MapReaderData(reader, readerDataMapping);

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
            var result = _dataAccess.GetMatches<LiveCasino>(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetMatches_TypeIsNotSupported_ThrowException()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelByLeague")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader(Arg.Any<object>()).Returns(reader);

            // Action
            TestDelegate result = () => _dataAccess.GetMatches<object>(_metaModel);

            // Assert
            Assert.Throws<NotSupportedException>(result, "The type 'object' is not supported.");
        }

        [Test]
        public void GetSportsbookGameDetail_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelByLeague_Detail")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader().Returns(reader);
            var matchId = 1;

            // Action
            _dataAccess.GetSportsbookGameDetail(matchId);

            // Assert
            objectDb.Received().AddInParameter("matchid", DbType.Int32, matchId);
        }

        [Test]
        public void GetSportsbookGameDetail_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelByLeague_Detail")).Returns(objectDb);
            var reader = Substitute.For<IDataReader>();
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false, true, false);
            reader.NextResult().Returns(true);

            reader.FieldCount.Returns(3);
            reader.GetName(0).Returns("lcolor");
            reader.GetName(1).Returns("LiveAwayScore");
            reader.GetName(2).Returns("LiveHomeScore");

            var dataMapping = GetReaderDataMappingForSportsbookType();
            var dataMappingForGameDetail = GetReaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(reader, dataMapping);
            int matchId = 11;

            var expectedResult = GetExpectedSportsbookData();
            expectedResult.Bettype = 0;
            expectedResult.BetID = null;

            // Action
            var result = _dataAccess.GetSportsbookGameDetail(matchId);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        private static Sportsbook GetExpectedSportsbookData()
        {
            return new Sportsbook
            {
                AwayId = 1,
                HomeId = 2,
                BetStatus = "win",
                LeagueId = 3,
                MatchId = 4,
                EventStatus = "lose",
                FinalAwayScore = 5,
                HtAwayScore = 6,
                Color = "red",
                FinalHomeScore = 7,
                HtHomeScore = 8,
                EventDate = new DateTime(2017, 3, 16),
                IsNeutral = true,
                IsPending = true,
                KickOffTime = new DateTime(2017, 3, 16),
                MatchCode = "matchcode",
                GoalRecord = "goalrecord",
                IsHTCal = true,
                Calculate = true,
                RefNo = string.Empty,
                LiveAwayScore = 9,
                LiveHomeScore = 10,
                BetID = "6",
                Bettype = 5,
                GameDetails = GetExpectedGameDetailData()
            };
        }

        private static List<GameDetail> GetExpectedGameDetailData()
        {
            return new List<GameDetail>
            {
                new GameDetail
                {
                    MatchId = 4,
                    ResourceId = "ResourceId",
                    HomeValue = "HomeValue",
                    AwayValue = "AwayValue",
                    ResultStatusID = 1,
                    SettlementStatusID = 2,
                    OtherInfo = "OtherInfo"
                }
            };
        }

        private static void MergeReaderDataMapping(Dictionary<string, object> source, Dictionary<string, object> destination)
        {
            foreach (var item in destination)
            {
                source[item.Key] = item.Value;
            }
        }

        private static Dictionary<string, object> GetReaderDataMappingForSportsbookType()
        {
            return new Dictionary<string, object>
            {
                { "awayID", 1 }, { "homeID", 2 }, { "betstatus", "win" }, { "leagueid", 3 }, { "matchid", 4 },
                { "eventstatus", "lose" }, { "finalawayscore", 5 }, { "htawayscore", 6 }, { "lcolor", "red" },
                { "finalhomescore", 7 }, { "hthomescore", 8 }, { "eventdate", "03/16/2017" }, { "IsNeutral", true },
                { "IsPending", true }, { "kickofftime", "03/16/2017" }, { "matchcode", "matchcode" }, { "goalrecord", "goalrecord" },
                { "IsHTCal", true }, { "Calculate", true }, { "LiveAwayScore", 9 }, { "LiveHomeScore", 10 }
            };
        }

        private static Dictionary<string, object> GetReaderDataMappingForGameDetail()
        {
            return new Dictionary<string, object>
            {
                { "ResultStatusID", 1 }, { "SettlementStatusID", 2 }, { "matchid", 4 }, { "ResourceId", "ResourceId" },
                { "HomeValue", "HomeValue" }, { "AwayValue", "AwayValue" }, { "OtherInfo", "OtherInfo" }
            };
        }

        private static List<Racing> GetExpectedRacingData()
        {
            return new List<Racing>
            {
                new Racing
                {
                    LeagueId = 1,
                    EngineId = "1",
                    EngineNo = "2",
                    IsFavor = true,
                    IsSCR = true,
                    Place = 2,
                    PlaceOdds = 10.5m,
                    WinOdds = 0.5m,
                    RaceName = 4,
                    KickOffTime = new DateTime(2017, 3, 16),
                    Favourite = "1",
                    RowCount = 4,
                    SCROfMatch = "SCR",
                    TFOfMatch = "TF"
                }
            };
        }
    }
}