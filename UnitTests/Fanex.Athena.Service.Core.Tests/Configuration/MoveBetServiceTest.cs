namespace Fanex.Athena.Service.Core.Tests.Common
{
    using System;
    using Configuration;
    using Configuration.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MoveBetServiceTest
    {
        private IAppSettings _appSettings;
        private IMoveBetDataAccess _moveBetDataAccess;
        private IMoveBetService _moveBetService;

        [SetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _moveBetDataAccess = Substitute.For<IMoveBetDataAccess>();
            _moveBetService = new MoveBetService(_appSettings, _moveBetDataAccess);
        }

        [Test]
        public void GetMoveBet_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new MoveBet()
            {
                AvailableWinlostDate = new DateTime(2016, 1, 1)
            };
            _moveBetDataAccess.GetMoveBet().Returns(expectedResult);

            // Action
            var result = _moveBetService.GetMoveBet();

            // Assert
            Assert.That(result.AvailableWinlostDate, Is.EqualTo(expectedResult.AvailableWinlostDate));
        }
    }
}