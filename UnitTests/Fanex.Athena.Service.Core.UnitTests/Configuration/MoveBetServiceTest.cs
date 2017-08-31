namespace Fanex.Athena.Service.Core.UnitTests.Configuration
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
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
            const string ExpectedMessage = "Sample Message";
            var expectedResult = new MoveBetNotification
            {
                ContentByLanguage = new Dictionary<string, string>
                {
                    { "en-US", ExpectedMessage }
                }
            };
            _moveBetDataAccess.GetMoveBetNotification().Returns(expectedResult);

            // Action
            var result = _moveBetService.GetMoveBetNotification("en-US");

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedMessage));
        }
    }
}