namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Core.NumberGame.DTO;

    public partial class ResultDataAccessTest
    {
        [Test]
        public void GetPopUpNumberGameMatch_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpNumberGameMatchSp)).Returns(ObjectDb);
            dynamic result = null;
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(x => result = x));
            var expected = new
            {
                matchid = MatchId
            };

            // Action
            DataAccess.GetPopUpNumberGameMatch(MatchId);

            // Assert
            Assert.That(result, Tobe.SameProperties(expected));
        }

        [Test]
        public void GetPopUpNumberGameMatch_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpNumberGameMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("isrefund");
            var matchId = 2;
            var readerDataMapping = new Dictionary<string, object>
            {
                { "leagueid", 1 }, { "matchid", matchId }, { "eventstatus", "win" }, { "eventdate", "03/16/2017" }, { "isrefund", "1" },
                { "kickofftime", "1/1/2017" }, { "matchcode", "test" }, { "seq", 3 }, { "number", 4 }
            };
            MapReaderData(readerDataMapping);

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
            var result = DataAccess.GetPopUpNumberGameMatch(matchId);

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
            DataAccess.GetObjectDb(Arg.Is(GetPopUpNumberGameMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, true, false);
            var matchId = 2;
            var readerDataMapping = new Dictionary<string, object>
            {
                { "leagueid", 1 }, { "matchid", matchId }, { "eventstatus", "win" }, { "eventdate", "03/16/2017" }, { "isrefund", "1" },
                { "kickofftime", "1/1/2017" }, { "matchcode", "test" }, { "seq", 3 }, { "number", 4 }
            };
            MapReaderData(readerDataMapping);

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
            var result = DataAccess.GetPopUpNumberGameMatch(matchId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [Test]
        public void GetPopUpNumberGameMatch_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetPopUpNumberGameMatchSp,
                () => DataAccess.GetPopUpNumberGameMatch(0));
        }
    }
}