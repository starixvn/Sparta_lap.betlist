namespace Fanex.Athena.DataAccess.Customer.Tests
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

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<LogInLogDataAccess>();
        }

        [Test]
        public void GetDataList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_UserLog_LoginSel")).Returns(_objectDb);
            var licenseeId = 0;
            var username = string.Empty;

            // Action
            _dataAccess.GetDataList(licenseeId, username);

            // Assert
            _objectDb.Received().AddInParameter("UserName", DbType.String, string.IsNullOrEmpty(username) ? string.Empty : username.Trim());
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, licenseeId);
        }
    }
}