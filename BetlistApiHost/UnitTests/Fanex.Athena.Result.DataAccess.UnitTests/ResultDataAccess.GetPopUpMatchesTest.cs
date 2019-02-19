namespace Fanex.Athena.DataAccess.Report.UnitTests.ResultReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using Fanex.Result.Core.Racing.DTO;
    using Fanex.Result.Core.Sportsbook.DTO;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Model.MetaModel;
    using UnitTest.Common.Helpers;

    public partial class ResultDataAccessTest
    {
        [TestCase(null, null)]
        [TestCase(0, "0")]
        [TestCase(1, "1")]
        public void GetPopUpMatches_WhenCall_ParametersPassedCorrectly(int? raceNo, string expectedRaceNo)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpMatchesSp)).Returns(ObjectDb);
            dynamic result = null;
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(x => result = x));

            var metaModel = BuildResultPopupMetalModel();
            metaModel.RaceNo = raceNo;

            var expectedResult = new
            {
                matchid = metaModel.MatchId,
                raceno = expectedRaceNo,
                bettype = metaModel.Bettype,
                betid = metaModel.BetId
            };

            // Action
            DataAccess.GetPopUpMatches<Sportsbook>(metaModel);

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

            var metaModel = BuildResultPopupMetalModel();
            metaModel.BetId = betId;

            // Action
            var result = DataAccess.GetPopUpMatches<Sportsbook>(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
        }

        [TestCase(221)]
        [TestCase(222)]
        [TestCase(223)]
        [TestCase(224)]
        [TestCase(225)]
        [TestCase(226)]
        [TestCase(227)]
        public void GetPopUpMatches_TypeIsSportsbookAndFastMarketBetType_ReturnCorrectData(int betType)
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

            var metaModel = BuildResultPopupMetalModel();
            metaModel.Handicap1 = 16;
            metaModel.Bettype = betType;

            var expectedData = GetExpectedSportsbookData();
            expectedData.BetID = metaModel.Handicap1.ToString();
            expectedData.Bettype = betType;
            var expectedResult = new List<Sportsbook> { expectedData };

            // Action
            var result = DataAccess.GetPopUpMatches<Sportsbook>(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Tobe.SameProperties(expectedResult[0]));
            });
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
            var result = DataAccess.GetPopUpMatches<Sportsbook>(BuildResultPopupMetalModel());

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
            var result = DataAccess.GetPopUpMatches<Sportsbook>(BuildResultPopupMetalModel());

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
            var result = DataAccess.GetPopUpMatches<Racing>(BuildResultPopupMetalModel());

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
            var result = DataAccess.GetPopUpMatches<Racing>(BuildResultPopupMetalModel());

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
            TestDelegate result = () => DataAccess.GetPopUpMatches<object>(BuildResultPopupMetalModel());

            // Assert
            Assert.Throws<NotSupportedException>(result, "The type 'object' is not supported.");
        }

        private ResultPopupMetaModel BuildResultPopupMetalModel()
        {
            return new ResultPopupMetaModel
            {
                MatchId = MatchId,
                RaceNo = RaceNo,
                Bettype = BettypeId,
                BetId = BetId
            };
        }
    }
}