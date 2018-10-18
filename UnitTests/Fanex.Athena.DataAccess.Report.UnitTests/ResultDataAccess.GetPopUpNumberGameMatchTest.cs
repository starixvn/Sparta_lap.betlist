namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using Fanex.Athena.UnitTest.Common.NUnitExtensions;
    using NSubstitute;
    using NUnit.Framework;
    using Result.Core.NumberGame.DTO;
    using UnitTest.Common.Helpers;

    public partial class ResultDataAccessTest
    {
        private readonly Dictionary<string, object> _readerDataMapping = new Dictionary<string, object>
            {
                { "leagueid", 111 }, { "matchid", 222 }, { "eventstatus", "win" }, { "eventdate", "03/16/2017" }, { "isrefund", "1" },
                { "kickofftime", "1/1/2017" }, { "matchcode", "test" }, { "seq", 3 }, { "number", 4 }
            };

        private readonly NumberGame _dataItem = new NumberGame
        {
            LeagueId = 111,
            MatchId = 222,
            EventStatus = "win",
            EventDate = new DateTime(2017, 3, 16),
            KickOffTime = new DateTime(2017, 1, 1),
            MatchCode = "test",
            Seg = new List<int> { 3, 3 },
            Ball = new List<int> { 4, 4 }
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

            MapReaderData(_readerDataMapping);
            var expectedResult = new List<NumberGame> { _dataItem };

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
            var expectedResult = new List<NumberGame> { _dataItem };

            // Action
            var result = DataAccess.GetPopUpNumberGameMatch(222);

            // Assert
            Assert.That(ObjectHelper.ToJson(result), Is.EqualTo(ObjectHelper.ToJson(expectedResult)));
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