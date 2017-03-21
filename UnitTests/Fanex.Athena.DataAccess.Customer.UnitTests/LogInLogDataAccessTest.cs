namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LogInLogDataAccessTest
    {
        private IObjectDb _objectDb;
        private LogInLogDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<LogInLogDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserLog_LoginSel")).Returns(_objectDb);
            var licenseeId = 0;
            var username = string.Empty;
            var agentList = "xml";

            // Action
            _dataAccess.GetDataList(licenseeId, username, agentList);

            // Assert
            _objectDb.Received().AddInParameter("UserName", DbType.String, string.IsNullOrEmpty(username) ? string.Empty : username.Trim());
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, agentList);
        }
    }
}