namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System.Collections.Generic;
    using DataAccess.Customer.Interfaces;
    using Model.Customer.DTO.CustomerLog;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LogInLogServiceTest
    {
        private ILogInLogDataAccess _logInLogDataAccess;
        private LogInLogService _logInLogService;

        [SetUp]
        public void SetUp()
        {
            _logInLogDataAccess = Substitute.For<ILogInLogDataAccess>();
            _logInLogService = new LogInLogService(_logInLogDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _logInLogDataAccess = null;
            _logInLogService = null;
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectValue()
        {
            // Arrange
            var licenseeId = 1;
            var username = "tester";
            var agentListXml = string.Empty;
            var expectedResult = new List<LogInLog> { new LogInLog { LogId = 1 }, new LogInLog { LogId = 2 } };
            _logInLogDataAccess.GetDataList(licenseeId, username, agentListXml).Returns(expectedResult);

            // Action
            var result = _logInLogService.GetDataList(licenseeId, username, agentListXml);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}