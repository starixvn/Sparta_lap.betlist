namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using System.Data;
    using System.Threading.Tasks;
    using Core.Common;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    public class FunctionDataAccessTest : BaseDataAccessHelperTest<FunctionDataAccess>
    {
        private const string GetFunctionListSp = "Lrf_FunctionsSel";
        private const string GetLicenseeFunctionList = "Lrf_LicenseeFunctionsSel";
        private const string GetUserFunctionList = "Lrf_LicenseeUserFunctionsSel";
        private const string AssignUserFunction = "Lrf_LicenseeUserFunctionsIns";
        private const string RemoveUserFunction = "Lrf_LicenseeUserFunctionsDel";
        private const int LicenseeId = 1;
        private const int CustId = 2;
        private const int FunctionId = 3;
        private const int UserId = 4;
        private const string Ip = "IP";
        private const string LangKey = "en-US";

        private IAppSettings _appSetting;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            _appSetting = Substitute.For<IAppSettings>();
            DataAccess = Substitute.ForPartsOf<FunctionDataAccess>(_appSetting);
            ObjectDb = Substitute.For<Data.IObjectDb>();
        }

        [OneTimeTearDown]
        public override void FixtureTearDown()
        {
            base.FixtureTearDown();

            _appSetting = null;
        }

        [TestCase("PRO")]
        [TestCase("TEST")]
        public void GetFunctionList_WhenCall_DataReturnedCorrectly(string environment)
        {
            // Arrange
            _appSetting.Environment.Returns(environment);

            DataAccess.GetObjectDb(Arg.Is(GetFunctionListSp)).Returns(ObjectDb);
            MapReaderData(new
            {
                FunctionId = 1,
                Environment = "LIVE"
            });
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetFunctionList();

            // Assert
            Assert.That(result[0].FunctionId, Is.EqualTo(1));
        }

        [Test]
        public void GetFunctionList_OnProduction_FunctionIsNotLive_DataReturnedCorrectly()
        {
            // Arrange
            _appSetting.Environment.Returns("PRO");

            DataAccess.GetObjectDb(Arg.Is(GetFunctionListSp)).Returns(ObjectDb);
            MapReaderData(new
            {
                FunctionId = 1,
                Environment = "TEST"
            });
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetFunctionList();

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void GetLicenseeFunctionList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLicenseeFunctionList)).Returns(ObjectDb);

            // Action
            DataAccess.GetLicenseeFunctionList(LicenseeId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("licenseeid", LicenseeId);
        }

        [Test]
        public void GetLicenseeFunctionList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var expected = new List<Function> { new Function { FunctionId = 1 }, new Function { FunctionId = 2 } };

            DataAccess.GetObjectDb(Arg.Is(GetLicenseeFunctionList)).Returns(ObjectDb);
            Reader["FunctionId"].Returns(1, 2);
            Reader.Read().Returns(true, true, false);

            // Action
            var result = DataAccess.GetLicenseeFunctionList(LicenseeId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].FunctionId, Is.EqualTo(expected[0].FunctionId));
                Assert.That(result[1].FunctionId, Is.EqualTo(expected[1].FunctionId));
            });
        }

        [Test]
        public void GetUserFunctionList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetUserFunctionList)).Returns(ObjectDb);

            // Action
            DataAccess.GetUserFunctionList(LicenseeId, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeId", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("UserId", UserId);
            });
        }

        [Test]
        public void GetUserFunctionList_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var expected = new List<Function> { new Function { FunctionId = 1 }, new Function { FunctionId = 2 } };

            DataAccess.GetObjectDb(Arg.Is(GetUserFunctionList)).Returns(ObjectDb);
            Reader["FunctionId"].Returns(1, 2);
            Reader.Read().Returns(true, true, false);

            // Action
            var result = DataAccess.GetUserFunctionList(LicenseeId, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].FunctionId, Is.EqualTo(expected[0].FunctionId));
                Assert.That(result[1].FunctionId, Is.EqualTo(expected[1].FunctionId));
            });
        }

        [Test]
        public void AssignUserFunction_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(AssignUserFunction)).Returns(ObjectDb);

            // Action
            DataAccess.AssignUserFunctionAsync(LicenseeId, CustId, FunctionId, UserId, Ip, LangKey);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("UserID", UserId);
                ObjectDb.Received(1).SetParameterValue("LicenseeID", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("FunctionID", FunctionId);
                ObjectDb.Received(1).SetParameterValue("CustID", CustId);
                ObjectDb.Received(1).SetParameterValue("LangKey", LangKey);
                ObjectDb.Received(1).SetParameterValue("IP", Ip);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }

        [Test]
        public async Task AssignUserFunction_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(AssignUserFunction)).Returns(ObjectDb);
            ResultInfoHelper.MockResultInfo(ObjectDb);

            // Action
            var result = await DataAccess.AssignUserFunctionAsync(LicenseeId, CustId, FunctionId, UserId, Ip, LangKey);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
                Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
            });
        }

        [Test]
        public void RemoveUserFunction_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RemoveUserFunction)).Returns(ObjectDb);

            // Action
            DataAccess.RemoveUserFunctionAsync(LicenseeId, UserId, CustId, Ip, FunctionId, LangKey);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("UserID", UserId);
                ObjectDb.Received(1).SetParameterValue("LicenseeID", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("FunctionID", FunctionId);
                ObjectDb.Received(1).SetParameterValue("CustID", CustId);
                ObjectDb.Received(1).SetParameterValue("LangKey", LangKey);
                ObjectDb.Received(1).SetParameterValue("IP", Ip);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }

        [Test]
        public async Task RemoveUserFunction_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(RemoveUserFunction)).Returns(ObjectDb);
            ResultInfoHelper.MockResultInfo(ObjectDb);

            // Action
            var result = await DataAccess.RemoveUserFunctionAsync(LicenseeId, UserId, CustId, Ip, FunctionId, LangKey);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Code, Is.EqualTo(ResultInfoHelper.ResultCode));
                Assert.That(result.Message, Is.EqualTo(ResultInfoHelper.ResultMessage));
            });
        }
    }
}