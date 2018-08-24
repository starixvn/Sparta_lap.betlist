namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System;
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.Customer.DTO.CustomerLog;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.NUnitExtensions;

    public class LogInLogDataAccessTest : BaseDataAccessHelperTest<LogInLogDataAccess>
    {
        private const string UspName = "Lrf_UserLog_LoginSel";

        [TestCase("Name ", "Name")]
        [TestCase(" ", "")]
        public void GetDataList_WhenCall_ParametersPassedCorrectly(string userName, string expectdUserName)
        {
            // Arrange
            const int LicenseeId = 0;
            const string AgentList = "xml";

            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);

            // Action
            DataAccess.GetDataList(LicenseeId, userName, AgentList);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("UserName", DbType.String, expectdUserName);
                ObjectDb.Received(1).AddInParameter("LicenseeId", DbType.Int32, LicenseeId);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, AgentList);
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
                LastTxn = new DateTime(2011, 2, 2)
            };
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
            MapReaderData(new
            {
                expectedResult.UserName,
                expectedResult.CreatedDate,
                expectedResult.LastTxn
            });
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetDataList(0, null, null);
            var firstItem = result[0];

            // Assert
            Assert.That(firstItem, Tobe.SameProperties(expectedResult));
        }
    }
}