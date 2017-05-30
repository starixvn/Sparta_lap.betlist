namespace Fanex.Athena.Service.Core.UnitTests.Configuration
{
    using System;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BalanceUpServiceTest
    {
        private IBalanceUpDataAccess _balanceUpDataAccess;
        private IBalanceUpService _balanceUpService;

        [OneTimeSetUp]
        public void SetUp()
        {
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balanceUpService = new BalanceUpService(_balanceUpDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _balanceUpDataAccess = null;
            _balanceUpService = null;
        }

        [Test]
        public void GetAllSports_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new BalanceUp
            {
                BalanceUpDay = new DateTime(2016, 1, 1)
            };
            _balanceUpDataAccess.GetBalanceUpDay().Returns(expectedResult);

            // Action
            var result = _balanceUpService.BalanceUpInfo;

            // Assert
            Assert.That(result.BalanceUpDay, Is.EqualTo(expectedResult.BalanceUpDay));
        }

        [Test]
        public void BalanceUpInfo_GetDataFromCache_ReturnCachedData()
        {
            // Arrange
            var expectedResult = new BalanceUp
            {
                BalanceUpDay = new DateTime(2016, 1, 1)
            };
            _balanceUpDataAccess.GetBalanceUpDay().Returns(expectedResult);
            var firstGetting = _balanceUpService.BalanceUpInfo;

            // Action
            var result = _balanceUpService.BalanceUpInfo;

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}