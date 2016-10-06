namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System;
    using Configuration;
    using Configuration.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BalanceUpServiceTest
    {
        private IBalanceUpDataAccess _balanceUpDataAccess;
        private IBalanceUpService _balanceUpService;

        [SetUp]
        public void SetUp()
        {
            _balanceUpDataAccess = Substitute.For<IBalanceUpDataAccess>();
            _balanceUpService = new BalanceUpService(_balanceUpDataAccess);
        }

        [Test]
        public void GetAllSports_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new BalanceUp()
            {
                BalanceUpDay = new DateTime(2016, 1, 1)
            };
            _balanceUpDataAccess.GetBalanceUpDay().Returns(expectedResult);

            // Action
            var result = _balanceUpService.BalanceUpInfo;

            // Assert
            Assert.That(result.BalanceUpDay, Is.EqualTo(expectedResult.BalanceUpDay));
        }
    }
}