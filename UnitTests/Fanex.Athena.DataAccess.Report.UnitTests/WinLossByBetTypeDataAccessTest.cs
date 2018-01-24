namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Data;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.NUnitExtensions;

    [TestFixture]
    public class WinLossByBetTypeDataAccessTest : BaseDataAccessHelperTest<WinLossByBetTypeDataAccess>
    {
        private const string SpName = "Lrf_Rpt_WinLossByBettype";

        private BalanceUp _balanceUp;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balanceUp = new BalanceUp
            {
                BalanceUpDay = new DateTime(2018, 1, 1),
                MoveBetsDay = new DateTime(2018, 2, 2),
            };
            balanceUpDataAccess.GetBalanceUp().Returns(_balanceUp);

            DataAccess = Substitute.ForPartsOf<WinLossByBetTypeDataAccess>(balanceUpDataAccess);
            ObjectDb = Substitute.For<IObjectDb>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            _balanceUp = null;

            base.FixtureTearDown();
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = new WinLossByBetTypeMetaModel
            {
                AgentListXml = "xml",
                FromDate = new DateTime(2017, 1, 1),
                ToDate = new DateTime(2017, 2, 2)
            };
            var expectedResult = new
            {
                AgentList = metaModel.AgentListXml,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                MoveBetsDay = _balanceUp.MoveBetsDay,
                BalanceUpDay = _balanceUp.BalanceUpDay
            };
            dynamic uspParam = null;
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(arg => uspParam = arg));

            // Action
            DataAccess.GetDataList(metaModel);

            // Assert
            Assert.That(uspParam, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
            ObjectDb.ExecuteReader(Arg.Any<object>()).ReturnsForAnyArgs(Reader);
            Reader.Read().Returns(true, false);
            var readerDataMapping = new
            {
                TurnoverUSD = 999
            };

            MapReaderData(readerDataMapping);

            // Action
            var result = DataAccess.GetDataList(new WinLossByBetTypeMetaModel
            {
                BaseCurrency = "USD"
            });

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].Turnover, Is.EqualTo(999));
            });
        }

        [Test]
        public void GetDataList_WhenReceiveObjectDbException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                SpName,
                () => DataAccess.GetDataList(new WinLossByBetTypeMetaModel()));
        }
    }
}