using Fanex.Athena.Common.Utility.Services;
using Fanex.Athena.Common.Utility.Services.Interfaces;
using Fanex.RemotingRef;
using NSubstitute;
using NUnit.Framework;
namespace Fanex.Athena.Common.Utility.UnitTests.Services
{
    public class CacheRefAdapterTest
    {
        private ICacheRefAdapter _cacheRefAdapter;
        private IRemotingRefService _remotingRefService;
        private const string ExpectedSportName = "TestSport";
        private const string ExpectedLeagueName = "TestLeagueName";

        [SetUp]
        public void Setup()
        {
            _remotingRefService = Substitute.For<IRemotingRefService>();
            _remotingRefService.GetSportName(Arg.Any<string>(),
                Arg.Any<string>(), string.Empty)
                .Returns(ExpectedSportName);

            _remotingRefService.GetLeagueName(Arg.Any<string>(),
               Arg.Any<string>(), string.Empty)
               .Returns(ExpectedLeagueName);

            _remotingRefService.GetCombinationTypeCombo(Arg.Any<string>(), Arg.Any<string>(), Arg.Any<int>(),
                Arg.Any<int>(), Arg.Any<string>()).Returns(new[] { "call1", "call2" });
            _cacheRefAdapter = new CacheRefAdapter(_remotingRefService);

        }

        [TearDown]
        public void TearDown()
        {
            _cacheRefAdapter = null;
            _remotingRefService = null;
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase("")]
        public void GetSportName_WithInvalidSport_ReturnEmpty(object sportId)
        {
            // Arrange

            // Action
            var result = _cacheRefAdapter.GetSportName(sportId, "en-US");

            // Assert
            Assert.True(string.IsNullOrEmpty(result));
        }
        [Test]
        public void GetSportName_ValidSportId_ReturnSportName()
        {
            // Arrange
            const int sportId = 1;
            // Action
            var result = _cacheRefAdapter.GetSportName(sportId, "en-US");

            // Assert
            Assert.AreEqual(ExpectedSportName, result);

        }
        [Test]
        public void GetLeagueName_ValidLeagueId_ReturnLegueName()
        {
            // Arrange
            const int leagueId = 1;
            // Action
            var result = _cacheRefAdapter.GetLeagueName(leagueId);

            // Assert
            Assert.AreEqual(ExpectedLeagueName, result);
        }

        [TestCase(null)]
        [TestCase(0)]
        [TestCase("")]
        public void GetLeagueName_InValidLeagueId_ReturnEmpty(object leagueId)
        {
            // Arrange

            // Action
            var result = _cacheRefAdapter.GetLeagueName(leagueId);

            // Assert
            Assert.True(string.IsNullOrEmpty(result));
        }

        [Test]
        public void GetCombinationTypeCombo_WhenCall_ReturnCombinationTypeCombo()
        {
            // Arrange
            const string betTeam = "betteam";
            const string betCheck = "betCheck";
            const decimal handicap1 = 1;
            const decimal handicap2 = 2;
            // Action
            var result = _cacheRefAdapter.GetCombinationTypeCombo(betTeam,
                betCheck, handicap1, handicap2);

            // Assert
            Assert.True(result.Length == 2);
            Assert.True(result[0] == "call1");
            Assert.True(result[1] == "call2");

        }
    }
}
