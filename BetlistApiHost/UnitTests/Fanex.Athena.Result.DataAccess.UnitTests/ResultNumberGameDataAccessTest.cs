namespace Fanex.Athena.Result.DataAccess.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Common.DataAccess.UnitTests;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using Fanex.Result.Core.NumberGame.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Result.DataAccess;
    using Result.Model.MetaModel;
    using UnitTest.Common.Helpers;

    public class ResultNumberGameDataAccessTest : BaseDataAccessHelperTest<ResultNumberGameDataAccess>
    {
        private const int MatchId = 122;

        private const string GetNumberGameMatchesSp = "Lrf_Result_MatchSelNumberGameByLeague";

        private const string GetPopUpNumberGameMatchSp = "Lrf_BetList_SelMatchResultNumberGame";

        private readonly Dictionary<string, object> _readerDataMapping = new Dictionary<string, object>
            {
                { "leagueid", 111 }, { "matchid", 222 }, { "eventstatus", "win" }, { "eventdate", "03/16/2017" }, { "isrefund", "1" },
                { "kickofftime", "1/1/2017" }, { "matchcode", "test" }, { "seq", 3 }, { "number", 4 }
            };

        private readonly ResultMetaModel _metaModel = new ResultMetaModel
        {
            FromDate = new DateTime(2017, 1, 1),
            ToDate = new DateTime(2017, 2, 2),
            SportId = 123
        };

        [Test]
        public void GetPopUpNumberGameMatch_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpNumberGameMatchSp)).Returns(ObjectDb);
            dynamic result = null;
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(x => result = x));
            var expected = new
            {
                matchId = MatchId
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

            MapReaderData(_readerDataMapping);
            var dataItem = GetSampleDataItem();
            dataItem.IsPending = true;
            var expectedResult = new List<NumberGame> { dataItem };

            // Action
            var result = DataAccess.GetPopUpNumberGameMatch(222);

            // Assert
            Assert.That(ObjectHelper.ToJson(result), Is.EqualTo(ObjectHelper.ToJson(expectedResult)));
        }

        [Test]
        public void GetPopUpNumberGameMatch_IsRefundColumnNotExist_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpNumberGameMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, true, false);
            MapReaderData(_readerDataMapping);
            var dataItem = GetSampleDataItem();
            dataItem.IsPending = false;
            var expectedResult = new List<NumberGame> { dataItem };

            // Action
            var result = DataAccess.GetPopUpNumberGameMatch(222);

            // Assert
            Assert.That(ObjectHelper.ToJson(result), Is.EqualTo(ObjectHelper.ToJson(expectedResult)));
        }

        [Test]
        public void GetNumberGameMatches_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetNumberGameMatchesSp)).Returns(ObjectDb);
            _metaModel.LeagueId = 1;

            dynamic result = null;
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(x => result = x));
            var expectedResult = new
            {
                fromdate = _metaModel.FromDate,
                leagueid = _metaModel.LeagueId,
                sporttype = _metaModel.SportId,
                isshowall = true
            };

            // Action
            DataAccess.GetNumberGameMatches(_metaModel);

            // Assert
            Assert.That(result, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetNumberGameMatches_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetNumberGameMatchesSp)).Returns(ObjectDb);
            _metaModel.LeagueId = 1;

            Reader.Read().Returns(true, true, false);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("isrefund");

            var readerDataMapping = new Dictionary<string, object>
            {
                { "leagueid", 1 }, { "matchid", 2 }, { "eventstatus", "win" }, { "eventdate", "03/16/2017" }, { "isrefund", "1" },
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
            var result = DataAccess.GetNumberGameMatches(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        private static NumberGame GetSampleDataItem()
        {
            return new NumberGame
            {
                LeagueId = 111,
                MatchId = 222,
                EventStatus = "win",
                EventDate = new DateTime(2017, 3, 16),
                IsPending = true,
                KickOffTime = new DateTime(2017, 1, 1),
                MatchCode = "test",
                Seg = new List<int> { 3, 3 },
                Ball = new List<int> { 4, 4 }
            };
        }
    }
}