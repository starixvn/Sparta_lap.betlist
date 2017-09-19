namespace Fanex.Athena.Service.Core.UnitTests.Configuration
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility.Interfaces;
    using Model.Core.DTO.Configuration;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class MoveBetServiceTest
    {
        private IMoveBetDataAccess _moveBetDataAccess;
        private IMoveBetService _moveBetService;
        private IMemoryCacheService _cache;

        [OneTimeSetUp]
        public void SetUp()
        {
            _moveBetDataAccess = Substitute.For<IMoveBetDataAccess>();
            _cache = Substitute.For<IMemoryCacheService>();
            _moveBetService = new MoveBetService(_moveBetDataAccess, _cache);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
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

            _cache.Get<MoveBetNotification>(Arg.Any<string>()).Returns(x => null);
            _cache.When(x => x.Set(Arg.Any<string>(), Arg.Any<MoveBetNotification>(), Arg.Any<int>())).DoNotCallBase();

            // Action
            var result = _moveBetService.GetMoveBetNotification("en-US");

            // Assert
            Assert.That(result, Is.EqualTo(ExpectedMessage));
        }
    }
}