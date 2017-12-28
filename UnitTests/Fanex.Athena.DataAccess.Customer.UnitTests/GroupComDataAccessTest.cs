namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class GroupComDataAccessTest : BaseDataAccessHelperTest<GroupComDataAccess>
    {
        private const string GetLicenseeGroupCommSp = "Lrf_LicenseeGroupCommSel";
        private const int LicenseeId = 0;

        [Test]
        public void GetAll_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLicenseeGroupCommSp)).Returns(ObjectDb);

            // Action
            DataAccess.GetAll(LicenseeId);

            // Assert
            ObjectDb.Received().AddInParameter("LicenseeID", DbType.Int32, LicenseeId);
        }

        [Test]
        public void GetDataForExcel_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLicenseeGroupCommSp)).Returns(ObjectDb);
            ObjectDb.ExecuteDataSet().Returns(new DataSet());

            // Action
            DataAccess.GetDataForExcel(LicenseeId);

            // Assert
            ObjectDb.Received().AddInParameter("LicenseeID", DbType.Int32, LicenseeId);
        }

        [Test]
        public void GetDataForExcel_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var dataTable = new DataTable();
            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);

            DataAccess.GetObjectDb(Arg.Is(GetLicenseeGroupCommSp)).Returns(ObjectDb);
            ObjectDb.ExecuteDataSet().Returns(dataSet);

            // Action
            var result = DataAccess.GetDataForExcel(LicenseeId);

            // Assert
            Assert.That(result, Is.EqualTo(dataTable));
        }

        [Test]
        public void GetById_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int GroupCommId = 0;
            DataAccess.GetObjectDb(Arg.Is<string>("Lrf_GroupCommGetById")).Returns(ObjectDb);

            // Action
            DataAccess.GetById(GroupCommId);

            // Assert
            ObjectDb.Received().AddInParameter("GroupCommID", DbType.Int32, GroupCommId);
        }

        [Test]
        public void InsertGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int UserId = 1;

            DataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeGroupCommIns")).Returns(ObjectDb);
            var item = new GroupCommMetaModel();
            var request = new Request();

            // Action
            DataAccess.InsertGroup(item, request, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("LicenseeID", DbType.Int32, item.LicenseeId);
                ObjectDb.Received().AddInParameter("GroupName", DbType.String, item.GroupName);
                ObjectDb.Received().AddInParameter("SBComm", DbType.Decimal, item.SBComm);
                ObjectDb.Received().AddInParameter("RacingComm", DbType.Decimal, item.RCComm);
                ObjectDb.Received().AddInParameter("NGComm", DbType.Decimal, item.NGComm);
                ObjectDb.Received().AddInParameter("LCComm", DbType.Decimal, item.LCComm);
                ObjectDb.Received().AddInParameter("RNGComm", DbType.Decimal, item.CSComm);
                ObjectDb.Received().AddInParameter("GroupColor", DbType.String, item.GroupColor);
                ObjectDb.Received().AddInParameter("UserID", DbType.Int32, UserId);
                ObjectDb.Received().AddInParameter("IP", DbType.String, request.Ip);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, request.Language);
                ObjectDb.Received().AddInParameter("IsDefault", DbType.Boolean, item.IsDefaultGroup);
                ObjectDb.Received().AddInParameter("IsResetDefault", DbType.Boolean, item.IsResetDefault);
                ObjectDb.Received().AddOutParameter("GroupCommID", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void UpdateGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int UserId = 0;

            DataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeGroupCommUpd")).Returns(ObjectDb);

            var newItem = new GroupCommMetaModel();
            var request = new Request();

            // Action
            DataAccess.UpdateGroup(newItem, request, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("LicenseeID", DbType.Int32, newItem.LicenseeId);
                ObjectDb.Received().AddInParameter("GroupCommId", DbType.Int32, newItem.GroupCommId);
                ObjectDb.Received().AddInParameter("GroupName", DbType.String, newItem.GroupName);
                ObjectDb.Received().AddInParameter("SbComm", DbType.Decimal, newItem.SBComm);
                ObjectDb.Received().AddInParameter("NGComm", DbType.Decimal, newItem.NGComm);
                ObjectDb.Received().AddInParameter("RacingComm", DbType.Decimal, newItem.RCComm);
                ObjectDb.Received().AddInParameter("LCComm", DbType.Decimal, newItem.LCComm);
                ObjectDb.Received().AddInParameter("RNGComm", DbType.Decimal, newItem.CSComm);
                ObjectDb.Received().AddInParameter("IsUpdateNG", DbType.Boolean, newItem.IsUpdateNG);
                ObjectDb.Received().AddInParameter("IsUpdateRacing", DbType.Boolean, newItem.IsUpdateRacing);
                ObjectDb.Received().AddInParameter("IsUpdateLC", DbType.Boolean, newItem.IsUpdateLC);
                ObjectDb.Received().AddInParameter("IsUpdateRNG", DbType.Boolean, newItem.IsUpdateCS);
                ObjectDb.Received().AddInParameter("GroupColor", DbType.String, newItem.GroupColor);
                ObjectDb.Received().AddInParameter("UserId", DbType.Int32, UserId);
                ObjectDb.Received().AddInParameter("Ip", DbType.String, request.Ip);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, request.Language);
                ObjectDb.Received().AddInParameter("IsDefault", DbType.Boolean, newItem.IsDefaultGroup);
                ObjectDb.Received().AddInParameter("IsResetDefault", DbType.Boolean, newItem.IsResetDefault);
                ObjectDb.Received().AddOutParameter("StrCustID", DbType.String, int.MaxValue);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void CheckByName_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const string GroupName = "";

            DataAccess.GetObjectDb(Arg.Is("Lrf_GroupCommCheckByName")).Returns(ObjectDb);

            // Action
            DataAccess.CheckByName(LicenseeId, GroupName);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("LicenseeId", DbType.Int32, LicenseeId);
                ObjectDb.Received().AddInParameter("GroupName", DbType.String, GroupName);
            });
        }

        [Test]
        public void DeleteGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int GroupId = 1;
            const int UserId = 3;
            const string Ip = "4";
            const string Language = "5";
            const bool IsRemoveCommSetting = true;
            const bool IsResetDefault = true;

            DataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeGroupCommDel")).Returns(ObjectDb);

            // Action
            DataAccess.DeleteGroup(GroupId, LicenseeId, UserId, Ip, Language, IsRemoveCommSetting, IsResetDefault);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("LicenseeId", DbType.Int32, LicenseeId);
                ObjectDb.Received().AddInParameter("GroupCommId", DbType.Int32, GroupId);
                ObjectDb.Received().AddInParameter("UserId", DbType.Int32, UserId);
                ObjectDb.Received().AddInParameter("Ip", DbType.String, Ip);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, Language);
                ObjectDb.Received().AddInParameter("IsRemovePlayercomm", DbType.Boolean, IsRemoveCommSetting);
                ObjectDb.Received().AddInParameter("IsResetDefault", DbType.Boolean, IsResetDefault);
                ObjectDb.Received().AddOutParameter("StrCustID", DbType.String, int.MaxValue);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }
    }
}