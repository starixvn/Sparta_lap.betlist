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
                { "eventdate", "03/16/2017"},
                { "sporttype", 1},
                { "leagueid", 2},
                { "teamid", 3},
                { "status", "win"}
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
                { "sporttype", "sport1"}
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

            // Action
            _dataAccess.GetLeagues(date, sportId);
            var result = new JavaScriptSerializer().Serialize(param);

            // Assert
            Assert.That(result, Is.EqualTo("{\"eventdate\":\"\\/Date(1489597200000)\\/\",\"sporttype\":1,\"isshowall\":true,\"movebetsday\":\"\\/Date(1470762000000)\\/\"}"));
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
                { "leagueid", 1}
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
            reader.Read().Returns(true, false);

            reader.FieldCount.Returns(1);
            reader.GetName(0).Returns("isrefund");
            var readerDataMapping = new Dictionary<string, object>
            {
                { "matchid", 0 },
                { "leagueid", 1 },
                { "matchid", 2 },
                { "eventstatus", "win" },
                { "eventdate", "03/16/2017" },
                { "isrefund", "1" },
                { "kickofftime", "1/1/2017" },
                { "matchcode", "test" },
                { "seq", 3 },
                { "number", 4 }
            };
            MapReaderData(reader, readerDataMapping);

            var expectedResult = new List<NumberGame>
            {
                new NumberGame
                {
                    LeagueId = 1,
                    MatchId = 0,
                    EventStatus = "win",
                    EventDate = new DateTime(2017, 3, 16),
                    IsPending = true,
                    KickOffTime = new DateTime(2017, 1, 1),
                    MatchCode = "test",
                    Seg = new List<int> { 3 },
                    Ball = new List<int> { 4 }
                }
            };

            // Action
            var result = _dataAccess.GetNumberGameMatches(_metaModel);

            // Assert
            Assert.That(result, Is.EqualTo("{\"fromdate\":\"\\/Date(1483203600000)\\/\",\"leagueid\":1,\"isshowall\":true}"));
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