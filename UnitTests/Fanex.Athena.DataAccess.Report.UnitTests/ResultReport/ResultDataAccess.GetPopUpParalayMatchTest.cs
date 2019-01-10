namespace Fanex.Athena.DataAccess.Report.UnitTests.ResultReport
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Core.Sportsbook.DTO;
    using UnitTest.Common.Helpers;

    public partial class ResultDataAccessTest
    {
        [Test]
        public void GetPopUpParalayMatch_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpParalayMatchSp)).Returns(ObjectDb);
            dynamic result = null;
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(x => result = x));
            var refNo = 1;
            var expected = new
            {
                refno = refNo
            };

            // Action
            DataAccess.GetPopUpParalayMatch(refNo);

            // Assert
            Assert.That(result, Tobe.SameProperties(expected));
        }

        [Test]
        public void GetPopUpParalayMatch_WhenCall_ReturnCorrectData()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetPopUpParalayMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            Reader.FieldCount.Returns(3);
            Reader.GetName(0).Returns("lcolor");
            Reader.GetName(1).Returns("bettype");
            Reader.GetName(2).Returns("betcheck");
            var refNo = 1;

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["LeagueID"] = 11;
            dataMapping["sporttype"] = 12;
            dataMapping["bettype"] = 5;
            dataMapping["betcheck"] = "6";
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            MapReaderData(dataMapping);

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
            var result = DataAccess.GetPopUpParalayMatch(refNo);

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
            DataAccess.GetObjectDb(Arg.Is(GetPopUpParalayMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            var refNo = 1;

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["LeagueID"] = 11;
            dataMapping["sporttype"] = 12;
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(Reader, dataMapping);

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
            var result = DataAccess.GetPopUpParalayMatch(refNo);

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
            DataAccess.GetObjectDb(Arg.Is(GetPopUpParalayMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, false, true, false);
            Reader.NextResult().Returns(true);
            Reader.FieldCount.Returns(1);
            Reader.GetName(0).Returns("lcolor");
            var refNo = 1;

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["LeagueID"] = 11;
            dataMapping["sporttype"] = 12;
            dataMapping["betstatus"] = DBNull.Value;
            dataMapping["lcolor"] = DBNull.Value;

            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            dataMappingForGameDetail["ResultStatusID"] = DBNull.Value;
            dataMappingForGameDetail["SettlementStatusID"] = DBNull.Value;
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(Reader, dataMapping);

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
            var result = DataAccess.GetPopUpParalayMatch(refNo);

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
            DataAccess.GetObjectDb(Arg.Is(GetPopUpParalayMatchSp)).Returns(ObjectDb);

            Reader.Read().Returns(true, true, false, true, false);
            Reader.NextResult().Returns(true);
            var refNo = 1;

            var dataMapping = GetReaderDataMappingForSportsbookType();
            dataMapping["LeagueID"] = 11;
            dataMapping["sporttype"] = 12;
            var dataMappingForGameDetail = GetreaderDataMappingForGameDetail();
            MergeReaderDataMapping(dataMapping, dataMappingForGameDetail);
            DataAccessTestHelper.MapReaderData(Reader, dataMapping);

            // Action
            var result = DataAccess.GetPopUpParalayMatch(refNo);

            // Assert
            Assert.That(result[0].Sportbook.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetPopUpParalayMatch_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetPopUpParalayMatchSp,
                () => DataAccess.GetPopUpParalayMatch(0));
        }
    }
}