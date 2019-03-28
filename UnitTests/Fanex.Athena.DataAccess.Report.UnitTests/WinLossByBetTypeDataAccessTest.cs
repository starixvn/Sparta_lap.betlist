namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Threading.Tasks;
    using Core.Configuration.Interfaces;
    using Core.UnitTests.TestHelpers;
    using Model.Core.DTO.Configuration;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.NUnitExtensions;
    using VendorAdapter.Interfaces;

    [TestFixture]
    public class WinLossByBetTypeDataAccessTest : BaseDataAccessHelperTest<WinLossByBetTypeDataAccess>
    {
        private const string SpName = "Lrf_Rpt_WinLossByBettype";

        private readonly BalanceUp _balanceUp = new BalanceUp
        {
            BalanceUpDay = new DateTime(2018, 1, 1),
            MoveBetsDay = new DateTime(2018, 2, 2),
        };

        private ICacheRefAdapter _cacheRefAdapter;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            base.FixtureSetUp();
        }

        protected override void SetUpDataAccess()
        {
            var balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            balanceUpDataAccess.GetBalanceUp().Returns(_balanceUp);
            DataAccess = Substitute.ForPartsOf<WinLossByBetTypeDataAccess>(
                _cacheRefAdapter,
                balanceUpDataAccess);
            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);
        }

        [Test]
        public async Task GetDataList_WhenCall_ParametersPassedCorrectly()
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
            ObjectDb.ExecuteReader(Arg.Do<dynamic>(arg => uspParam = arg));

            // Action
            await DataAccess.GetDataList(metaModel);

            // Assert
            Assert.That(uspParam, Tobe.SameProperties(expectedResult));
        }

        [Test]
        public async Task GetDataList_WhenCall_ReturnCorrectResult()
        {
            // Arrange
            Reader.Read().Returns(true, false);
            MapReaderData(new { TurnoverUSD = 999 });

            // Action
            var result = await DataAccess.GetDataList(new WinLossByBetTypeMetaModel
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

        [TestCase(1201, " (Virtual Sports)")]
        [TestCase(2700, " (Virtual Sports 2)")]
        public async Task GetDataList_WhenCall_ReturnCorrectBetTypeName(int bettype, string expectedBetTypeName)
        {
            // Arrange
            const string CacheRefBetTypeName = "ahihi";
            Reader.Read().Returns(true, false);
            MapReaderData(new { bettype });
            _cacheRefAdapter.GetBettype(bettype, false).Returns(CacheRefBetTypeName);

            // Action
            var result = await DataAccess.GetDataList(new WinLossByBetTypeMetaModel()).ConfigureAwait(false);

            // Assert
            Assert.That(result[0].BetTypeName, Is.EqualTo(CacheRefBetTypeName + expectedBetTypeName));
        }
    }
}