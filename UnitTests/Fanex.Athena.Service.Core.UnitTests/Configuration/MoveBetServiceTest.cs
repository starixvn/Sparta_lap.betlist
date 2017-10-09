namespace Fanex.Athena.Service.Core.UnitTests.Configuration
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility.Interfaces;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NSubstitute.ExceptionExtensions;
    using NUnit.Framework;

    [TestFixture]
    public class MoveBetServiceTest
    {
        private const string ExpectedMessage = "Sample Message";

        private IMoveBetDataAccess _moveBetDataAccess;
        private IMoveBetService _moveBetService;
        private IMemoryCacheService _cache;

        [SetUp]
        public void SetUp()
        {
            _moveBetDataAccess = Substitute.For<IMoveBetDataAccess>();
            _cache = Substitute.For<IMemoryCacheService>();
            _moveBetService = new MoveBetService(_moveBetDataAccess, _cache);
        }

        [TearDown]
        public void TearDown()
        {
            _moveBetDataAccess = null;
            _moveBetService = null;
        }

        [Test]
        public void GetMoveBet_CacheContainsData_ReturnResultFromCache()
        {
            // Arrange
            var expectedResult = new MoveBetNotification
            {
                ContentByLanguage = new Dictionary<string, string>
                {
                    { "vi-VN", ExpectedMessage }
                }
            };
            _cache.Get<MoveBetNotification>(Arg.Any<string>()).Returns(expectedResult);

            // Action
            var result = _moveBetService.GetMoveBetNotification("vi-VN");

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedMessage));
        }

        [Test]
        public void GetMoveBet_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new MoveBetNotification
            {
                ContentByLanguage = new Dictionary<string, string>
                {
                    { "vi-VN", ExpectedMessage }
                }
            };
            _moveBetDataAccess.GetMoveBetNotification().Returns(expectedResult);

            // Action
            var result = _moveBetService.GetMoveBetNotification("vi-VN");

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedMessage));
        }
        
        [Test]
        public void GetMoveBet_LanguageNotAvailable_ReturnContentWithDefaultLanguage()
        {
            // Arrange
            var expectedResult = new MoveBetNotification
            {
                ContentByLanguage = new Dictionary<string, string>
                {
                    { "en-US", ExpectedMessage }
                }
            };
            _cache.Get<MoveBetNotification>(Arg.Any<string>()).Returns(expectedResult);

            // Action
            var result = _moveBetService.GetMoveBetNotification("vi-VN");

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedMessage));
        }

        [Test]
        public void GetMoveBet_ResultIsEmpty_ReturnEmptyString()
        {
            // Arrange
            var expectedResult = new MoveBetNotification();
            _cache.Get<MoveBetNotification>(Arg.Any<string>()).Returns(expectedResult);

            // Action
            var result = _moveBetService.GetMoveBetNotification("vi-VN");

            // Assert
            Assert.That(result, Is.EqualTo(string.Empty));
        }
    }
}