namespace Fanex.Athena.Service.Core.Tests.Common
{
    using System.Collections.Generic;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LeagueServiceTest
    {
        private ILeagueDataAccess _leagueDataAccess;
        private ILeagueService _languageService;

        [SetUp]
        public void SetUp()
        {
            _leagueDataAccess = Substitute.For<ILeagueDataAccess>();
            _languageService = new LeagueService(_leagueDataAccess);
        }

        [Test]
        public void GetLeaguesBySport_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<League>()
            {
                new League() { LeagueId = 1 }
            };
            _leagueDataAccess.GetLeaguesBySport(Arg.Any<LeagueMetaModel>()).Returns(expectedResult);

            // Action
            var result = _languageService.GetLeaguesBySport(null);

            // Assert
            Assert.That(result[0].LeagueId, Is.EqualTo(expectedResult[0].LeagueId));
        }
    }
}