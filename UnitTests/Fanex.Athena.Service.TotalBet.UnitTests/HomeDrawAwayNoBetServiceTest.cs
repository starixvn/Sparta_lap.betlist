namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using DataAccess.TotalBet.Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using TotalBet;
    using TotalBet.Interfaces;

    [TestFixture]
    public class HomeDrawAwayNoBetServiceTest
    {
        private IHomeDrawAwayNoBetDataAccess _dataAccess;

        private IHomeDrawAwayNoBetService _service;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.For<IHomeDrawAwayNoBetDataAccess>();
            _service = new HomeDrawAwayNoBetService(_dataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
            _service = null;
        }

        [Test]
        public void GetHomeDrawAwayNoBetDataList_WhenCall_ReturnDataFromDataAccess()
        {
            // Arrange
            var expectedResult = new List<HomeDrawAwayNoBet>
            {
                new HomeDrawAwayNoBet()
            };
            _dataAccess.GetHomeDrawAwayNoBetDataList(Arg.Any<HomeDrawAwayNoBetMetaModel>())
                .Returns(expectedResult);

            // Action
            var result = _service.GetHomeDrawAwayNoBetDataList(new HomeDrawAwayNoBetMetaModel());

            // Assert
            _dataAccess.ReceivedWithAnyArgs().GetHomeDrawAwayNoBetDataList(Arg.Any<HomeDrawAwayNoBetMetaModel>());
            Assert.That(result.Count, Is.EqualTo(expectedResult.Count));
        }
    }
}