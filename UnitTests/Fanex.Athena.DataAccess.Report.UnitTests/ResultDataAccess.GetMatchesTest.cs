namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Core.LiveCasino.DTO;
    using Result.Core.Sportsbook.DTO;
    using UnitTest.Common.Helpers;

    public partial class ResultDataAccessTest
    {
        [Test]
        public void GetMatches_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetMatchesSp)).Returns(ObjectDb);
            dynamic param = null;
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(x => param = x));
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
            DataAccess.GetMatches<Sportsbook>(_metaModel);

            // Assert
            Assert.That(param, Tobe.SameProperties(expected));
        }

        [Test]
        public void GetMatches_TypeIsSportsbook_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetMatchesSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);

            Reader.FieldCount.Returns(3);
            Reader.GetName(0).Returns("lcolor");
            Reader.GetName(1).Returns("LiveAwayScore");
            Reader.GetName(2).Returns("LiveHomeScore");

            var dataMapping = GetReaderDataMappingForSportsbookType();
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(Reader, dataMapping);

            var sportsbookData = GetExpectedSportsbookData();
            sportsbookData.BetID = "0";
            sportsbookData.Bettype = 0;
            var expectedResult = new List<Sportsbook> { sportsbookData };

            // Action
            var result = DataAccess.GetMatches<Sportsbook>(_metaModel);

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
            DataAccess.GetObjectDb(Arg.Is(GetMatchesSp)).Returns(ObjectDb);

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
            var result = DataAccess.GetMatches<LiveCasino>(_metaModel);

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
            DataAccess.GetObjectDb(Arg.Is(GetMatchesSp)).Returns(ObjectDb);

            // Action
            TestDelegate result = () => DataAccess.GetMatches<object>(_metaModel);

            // Assert
            Assert.Throws<NotSupportedException>(result, "The type 'object' is not supported.");
        }

        [Test]
        public void GetMatches_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetMatchesSp,
                () => DataAccess.GetMatches<object>(_metaModel));
        }
    }
}