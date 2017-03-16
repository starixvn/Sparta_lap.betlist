namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Web.Script.Serialization;
    using Data;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Core.NumberGame.DTO;
    using Result.Core.Racing.DTO;
    using Result.Core.Sportsbook.DTO;

    [TestFixture]
    public class ResultDataAccessTest
    {
        private ResultDataAccess _dataAccess;
        private BalanceUp _balance;
        private ResultMetalModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _balance = new BalanceUp
            {
                MoveBetsDay = new DateTime(2016, 8, 10),
                BalanceUpDay = new DateTime(2016, 8, 31)
            };

            _dataAccess = Substitute.ForPartsOf<ResultDataAccess>(_balance);

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
            dynamic result = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(arg => result = arg));

            // Action
            _dataAccess.GetOutRight(_metaModel);

            // Assert
            Assert.That(
                new JavaScriptSerializer().Serialize(result),
                Is.EqualTo(@"{""fromdate"":""\/Date(1483203600000)\/"",""todate"":""\/Date(1485968400000)\/"",""isshowall"":true}"));
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
            MapReaderData(reader, readerDataMapping);

            var expectedResult = new List<Outright>
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
                Assert.That(result[0].EventDate, Is.EqualTo(expectedResult[0].EventDate));
                Assert.That(result[0].SportType, Is.EqualTo(expectedResult[0].SportType));
                Assert.That(result[0].LeagueId, Is.EqualTo(expectedResult[0].LeagueId));
                Assert.That(result[0].TeamId, Is.EqualTo(expectedResult[0].TeamId));
                Assert.That(result[0].Status, Is.EqualTo(expectedResult[0].Status));
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
            MapReaderData(reader, readerDataMapping);
            var date = new DateTime(2017, 3, 16);
            var licenseeId = 1;

            // Action
            var result = _dataAccess.GetSports(date, licenseeId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].SportId, Is.EqualTo(expectedResult[0].SportId));
            });
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
            var expectedResult = @"{""eventdate"":""\/Date(1489597200000)\/"",""sporttype"":1,""isshowall"":true,""movebetsday"":""\/Date(1470762000000)\/""}";

            // Action
            _dataAccess.GetLeagues(date, sportId);
            var result = new JavaScriptSerializer().Serialize(param);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
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
            MapReaderData(reader, readerDataMapping);

            var date = new DateTime(2017, 3, 16);
            var sportId = 1;

            // Action
            var result = _dataAccess.GetLeagues(date, sportId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].LeagueId, Is.EqualTo(expectedResult[0].LeagueId));
            });
        }

        [Test]
        public void GetNumberGameMatches_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_Result_MatchSelNumberGameByLeague")).Returns(objectDb);
            _metaModel.LeagueId = 1;

            dynamic param = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(x => param = x));

            // Action
            _dataAccess.GetNumberGameMatches(_metaModel);
            var result = new JavaScriptSerializer().Serialize(param);

            // Assert
            Assert.That(result, Is.EqualTo("{\"fromdate\":\"\\/Date(1483203600000)\\/\",\"leagueid\":1,\"isshowall\":true}"));
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
            MapReaderData(reader, readerDataMapping);

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
                Assert.That(result[0].LeagueId, Is.EqualTo(expectedResult[0].LeagueId));
                Assert.That(result[0].MatchId, Is.EqualTo(expectedResult[0].MatchId));
                Assert.That(result[0].EventStatus, Is.EqualTo(expectedResult[0].EventStatus));
                Assert.That(result[0].EventDate, Is.EqualTo(expectedResult[0].EventDate));
                Assert.That(result[0].IsPending, Is.EqualTo(expectedResult[0].IsPending));
                Assert.That(result[0].KickOffTime, Is.EqualTo(expectedResult[0].KickOffTime));
                Assert.That(result[0].MatchCode, Is.EqualTo(expectedResult[0].MatchCode));
                Assert.That(result[0].Seg[0], Is.EqualTo(expectedResult[0].Seg[0]));
                Assert.That(result[0].Ball[0], Is.EqualTo(expectedResult[0].Ball[0]));
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

            // Action
            _dataAccess.GetRacingMatches(_metaModel);
            var result = new JavaScriptSerializer().Serialize(param);

            // Assert
            Assert.That(result, Is.EqualTo("{\"fromdate\":\"\\/Date(1483203600000)\\/\",\"leagueid\":2,\"sporttype\":1,\"isshowall\":true}"));
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
            MapReaderData(reader, readerDataMapping);

            var expectedResult = GetExpectedRacingData();

            // Action
            var result = _dataAccess.GetRacingMatches(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].LeagueId, Is.EqualTo(expectedResult[0].LeagueId));
                Assert.That(result[0].EngineId, Is.EqualTo(expectedResult[0].EngineId));
                Assert.That(result[0].EngineNo, Is.EqualTo(expectedResult[0].EngineNo));
                Assert.That(result[0].IsFavor, Is.EqualTo(expectedResult[0].IsFavor));
                Assert.That(result[0].IsSCR, Is.EqualTo(expectedResult[0].IsSCR));
                Assert.That(result[0].Place, Is.EqualTo(expectedResult[0].Place));
                Assert.That(result[0].PlaceOdds, Is.EqualTo(expectedResult[0].PlaceOdds));
                Assert.That(result[0].WinOdds, Is.EqualTo(expectedResult[0].WinOdds));
                Assert.That(result[0].RaceName, Is.EqualTo(expectedResult[0].RaceName));
                Assert.That(result[0].KickOffTime, Is.EqualTo(expectedResult[0].KickOffTime));
                Assert.That(result[0].Favourite, Is.EqualTo(expectedResult[0].Favourite));
                Assert.That(result[0].RowCount, Is.EqualTo(expectedResult[0].RowCount));
                Assert.That(result[0].SCROfMatch, Is.EqualTo(expectedResult[0].SCROfMatch));
                Assert.That(result[0].TFOfMatch, Is.EqualTo(expectedResult[0].TFOfMatch));
            });
        }

        [Test]
        public void GetPopUpMatches_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            _dataAccess.GetObjectDb(Arg.Is("Lrf_BetList_SelMatchResult")).Returns(objectDb);
            dynamic param = null;
            objectDb.ExecuteReader(Arg.Do<dynamic>(x => param = x));
            int matchId = 1, raceNo = 2, licenseeId = 3, userId = 4, bettypeId = 5;
            long? betId = 6;

            // Action
            _dataAccess.GetPopUpMatches<Sportsbook>(matchId, raceNo, licenseeId, userId, bettypeId, betId);
            var result = new JavaScriptSerializer().Serialize(param);

            // Assert
            Assert.That(result, Is.EqualTo("{\"matchid\":1,\"raceno\":\"2\",\"licenseeid\":3,\"userid\":4,\"bettype\":5,\"betid\":6}"));
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
            MapReaderData(reader, dataMapping);
            int matchId = 1, raceNo = 2, licenseeId = 3, userId = 4, bettypeId = 5;
            long? betId = 6;

            var serializer = new JavaScriptSerializer();
            var expectedResult = serializer.Serialize(new List<Sportsbook> { GetExpectedSportsbookData() });

            // Action
            var result = serializer.Serialize(_dataAccess.GetPopUpMatches<Sportsbook>(matchId, raceNo, licenseeId, userId, bettypeId, betId));

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
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

        private static void MapReaderData(IDataReader reader, Dictionary<string, object> readerDataMapping)
        {
            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });
        }
    }
}