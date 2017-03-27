namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Core.Common;
    using Data;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    [TestFixture]
    public class FunctionDataAccessTest
    {
        private IObjectDb _objectDb;
        private FunctionDataAccess _dataAccess;
        private IDataReader _reader;
        private IAppSettings _appSetting;

        [OneTimeSetUp]
        public void Setup()
        {
            _appSetting = Substitute.For<IAppSettings>();
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<FunctionDataAccess>(_appSetting);
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _reader = null;
        }

        [Test]
        public void GetFunctionList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_FunctionsSel")).Returns(_objectDb);
            var expected = new List<Function> { new Function { FunctionId = 1 }, new Function { FunctionId = 2 } };
            _reader.Read().Returns(true, false);

            var readerDataMapping = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase)
            {
                { "FunctionId", 1 }
            };

            MapReaderData(_reader, readerDataMapping);

            // Action
            var result = _dataAccess.GetFunctionList();

            // Assert
            Assert.That(result[0].FunctionId, Is.EqualTo(1));
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
        public void GetLicenseeFunctionList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeFunctionsSel")).Returns(_objectDb);
            _reader.Read().Returns(true, true, false);
            _reader["FunctionId"].Returns(1, 2);
            var expected = new List<Function> { new Function { FunctionId = 1 }, new Function { FunctionId = 2 } };

            // Action
            var result = _dataAccess.GetLicenseeFunctionList(licenseeId);

            // Assert
            Assert.That(result[0].FunctionId, Is.EqualTo(expected[0].FunctionId));
            Assert.That(result[1].FunctionId, Is.EqualTo(expected[1].FunctionId));
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
        public void GetUserFunctionList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            var userId = 2;
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeUserFunctionsSel")).Returns(_objectDb);
            _reader.Read().Returns(true, true, false);
            _reader["FunctionId"].Returns(1, 2);
            var expected = new List<Function> { new Function { FunctionId = 1 }, new Function { FunctionId = 2 } };

            // Action
            var result = _dataAccess.GetUserFunctionList(licenseeId, userId);

            // Assert
            Assert.That(result[0].FunctionId, Is.EqualTo(expected[0].FunctionId));
            Assert.That(result[1].FunctionId, Is.EqualTo(expected[1].FunctionId));
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
        public void AssignUserFunction_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            var custId = 2;
            var functionId = 3;
            var userId = 4;
            var ip = "::1";
            var langKey = "en-US";

            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeUserFunctionsIns")).Returns(_objectDb);
            ResultInfoHelper.MockResultInfo(_objectDb);

            // Action
            var result = _dataAccess.AssignUserFunction(licenseeId, custId, functionId, userId, ip, langKey);

            // Assert
            Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
            Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
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

        [Test]
        public void RemoveUserFunction_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var licenseeId = 1;
            var custId = 2;
            var functionId = 3;
            var userId = 4;
            var ip = "::1";
            var langKey = "en-US";

            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeUserFunctionsDel")).Returns(_objectDb);
            ResultInfoHelper.MockResultInfo(_objectDb);

            // Action
            var result = _dataAccess.RemoveUserFunction(licenseeId, userId, custId, ip, functionId, langKey);

            // Assert
            Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
            Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
        }

        private static void MapReaderData(IDataReader reader, Dictionary<string, object> readerDataMapping)
        {
            reader[Arg.Any<string>()].Returns(x =>
            {
                var paramName = x.ArgAt<string>(0);
                if (readerDataMapping.ContainsKey(paramName))
                {
                    return readerDataMapping[paramName];
                }

                return string.Empty;
            });
        }
    }
}