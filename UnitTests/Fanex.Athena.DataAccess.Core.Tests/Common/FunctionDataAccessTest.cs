namespace Fanex.Athena.DataAccess.Core.Tests.Common
{
    using System.Data;
    using Core.Common;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class FunctionDataAccessTest
    {
        private IObjectDb _objectDb;
        private FunctionDataAccess _dataAccess;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<FunctionDataAccess>();
        }

        [Test]
        public void GetLicenseeFunctionList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeFunctionsSel")).Returns(_objectDb);

            // Action
            _dataAccess.GetLicenseeFunctionList(licenseeId);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, licenseeId);
        }

        [Test]
        public void GetUserFunctionList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeUserFunctionsSel")).Returns(_objectDb);

            // Action
            _dataAccess.GetUserFunctionList(licenseeId, userId);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, userId);
        }

        [Test]
        public void AssignUserFunction_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            var custId = 2;
            var functionId = 3;
            var userId = 4;
            var ip = "::1";
            var langKey = "en-US";

            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeUserFunctionsIns")).Returns(_objectDb);

            // Action
            _dataAccess.AssignUserFunction(licenseeId, custId, functionId, userId, ip, langKey);

            // Assert
            _objectDb.Received().AddInParameter("UserID", DbType.Int32, userId);
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("FunctionID", DbType.Int32, functionId);
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, custId);
            _objectDb.Received().AddInParameter("LangKey", DbType.String, langKey);
            _objectDb.Received().AddInParameter("IP", DbType.String, ip);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void RemoveUserFunction_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            var custId = 2;
            var functionId = 3;
            var userId = 4;
            var ip = "::1";
            var langKey = "en-US";

            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeUserFunctionsDel")).Returns(_objectDb);

            // Action
            _dataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, functionId, langKey);

            // Assert
            _objectDb.Received().AddInParameter("UserID", DbType.Int32, userId);
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("FunctionID", DbType.Int32, functionId);
            _objectDb.Received().AddInParameter("CustID", DbType.Int32, custId);
            _objectDb.Received().AddInParameter("LangKey", DbType.String, langKey);
            _objectDb.Received().AddInParameter("IP", DbType.String, ip);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }
    }
}