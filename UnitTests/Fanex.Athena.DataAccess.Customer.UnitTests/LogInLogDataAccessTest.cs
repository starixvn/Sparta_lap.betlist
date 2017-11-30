namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System;
    using System.Data;
    using Data;
    using Model.Customer.DTO.CustomerLog;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;
    using UnitTest.Common.NUnitExtensions;

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
            _dataAccess.GetObjectDb(Arg.Is("Lrf_UserLog_LoginSel")).Returns(_objectDb);
            var licenseeId = 0;
            var username = string.Empty;
            var agentList = "xml";

            // Action
            _dataAccess.GetDataList(licenseeId, username, agentList);

            // Assert
            Assert.Multiple(() =>
            {
                _objectDb.Received().AddInParameter("UserName", DbType.String, string.IsNullOrEmpty(username) ? string.Empty : username.Trim());
                _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, licenseeId);
                _objectDb.Received().AddInParameter("AgentList", DbType.String, agentList);
            });
        }

        [Test]
        public void GetDataList_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expectedResult = new LogInLog
            {
                UserName = "a",
                CreatedDate = new DateTime(2011, 1, 1),
                LastTxn = new DateTime(2011, 2, 2),
            };
            _dataAccess.GetObjectDb(Arg.Is("Lrf_UserLog_LoginSel")).Returns(_objectDb);
            var reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true, false);

            DataAccessTestHelper.MapReaderData(
                reader,
                new
                {
                    expectedResult.UserName,
                    expectedResult.CreatedDate,
                    expectedResult.LastTxn
                });

            // Action
            var result = _dataAccess.GetDataList(0, null, null);
            var firstItem = result[0];

            // Assert
            Assert.That(firstItem, Tobe.SameProperties(expectedResult));
        }
    }
}