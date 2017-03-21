namespace Fanex.Athena.Service.Core.UnitTests.Common
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

        [OneTimeSetUp]
        public void SetUp()
        {
            _appSettings = Substitute.For<IAppSettings>();
            _moveBetDataAccess = Substitute.For<IMoveBetDataAccess>();
            _moveBetService = new MoveBetService(_appSettings, _moveBetDataAccess);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _appSettings = null;
            _moveBetDataAccess = null;
            _moveBetService = null;
        }

        [Test]
        public void GetMoveBet_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new MoveBet
            {
                AvailableWinlostDate = new DateTime(2016, 1, 1)
            };
            _moveBetDataAccess.GetMoveBet().Returns(expectedResult);

            // Action
            var result = _moveBetService.GetMoveBet();

            // Assert
            Assert.That(result.AvailableWinlostDate, Is.EqualTo(expectedResult.AvailableWinlostDate));
        }

        [Test]
        public void GetMoveBet_GetDataFromCache_ReturnCachedData()
        {
            // Arrange
            var expectedResult = new MoveBet
            {
                AvailableWinlostDate = new DateTime(2016, 1, 1)
            };
            _moveBetDataAccess.GetMoveBet().Returns(expectedResult);
            var firstGetting = _moveBetService.GetMoveBet();

            // Action
            var result = _moveBetService.GetMoveBet();

            // Assert
            Assert.That(result.AvailableWinlostDate, Is.EqualTo(expectedResult.AvailableWinlostDate));
        }

        [Test]
        public void GetMoveBet_WhenCall_ReturnResultFromAppSetting()
        {
            // Arrange
            var expectedResult = new MoveBet
            {
                AvailableWinlostDate = new DateTime(2016, 1, 1),
                EstimatedTime = new DateTime(2017, 1, 1)
            };
            _appSettings.MoveBetDayStart.Returns(new DateTime(2016, 1, 1));
            _appSettings.MoveBetDayEnd.Returns(new DateTime(2017, 1, 1));

            // Action
            var result = _moveBetService.GetMoveBet();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.AvailableWinlostDate, Is.EqualTo(expectedResult.AvailableWinlostDate));
                Assert.That(result.EstimatedTime, Is.EqualTo(expectedResult.EstimatedTime));
            });
        }
    }
}