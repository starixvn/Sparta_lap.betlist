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
            ObjectDb.Received(1).AddInParameter("LicenseeID", DbType.Int32, LicenseeId);
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
            ObjectDb.Received(1).AddInParameter("LicenseeID", DbType.Int32, LicenseeId);
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
            DataAccess.GetObjectDb(Arg.Is("Lrf_GroupCommGetById")).Returns(ObjectDb);

            // Action
            DataAccess.GetById(GroupCommId);

            // Assert
            ObjectDb.Received(1).AddInParameter("GroupCommID", DbType.Int32, GroupCommId);
        }

        [Test]
        public void InsertGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int UserId = 1;

            DataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeGroupCommIns")).Returns(ObjectDb);
            var item = new GroupCommMetaModel();
            var request = new Request();

            // Action
            DataAccess.InsertGroup(item, request, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("LicenseeID", DbType.Int32, item.LicenseeId);
                ObjectDb.Received(1).AddInParameter("GroupName", DbType.String, item.GroupName);
                ObjectDb.Received(1).AddInParameter("SBComm", DbType.Decimal, item.SBComm);
                ObjectDb.Received(1).AddInParameter("NGComm", DbType.Decimal, item.NGComm);
                ObjectDb.Received(1).AddInParameter("RNGComm", DbType.Decimal, item.CSComm);
                ObjectDb.Received(1).AddInParameter("GroupColor", DbType.String, item.GroupColor);
                ObjectDb.Received(1).AddInParameter("UserID", DbType.Int32, UserId);
                ObjectDb.Received(1).AddInParameter("IP", DbType.String, request.Ip);
                ObjectDb.Received(1).AddInParameter("langkey", DbType.String, request.Language);
                ObjectDb.Received(1).AddInParameter("IsDefault", DbType.Boolean, item.IsDefaultGroup);
                ObjectDb.Received(1).AddInParameter("IsResetDefault", DbType.Boolean, item.IsResetDefault);
                ObjectDb.Received(1).AddOutParameter("GroupCommID", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void UpdateGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int UserId = 0;

            DataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeGroupCommUpd")).Returns(ObjectDb);

            var newItem = new GroupCommMetaModel();
            var request = new Request();

            // Action
            DataAccess.UpdateGroup(newItem, request, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("LicenseeID", DbType.Int32, newItem.LicenseeId);
                ObjectDb.Received(1).AddInParameter("GroupCommId", DbType.Int32, newItem.GroupCommId);
                ObjectDb.Received(1).AddInParameter("GroupName", DbType.String, newItem.GroupName);
                ObjectDb.Received(1).AddInParameter("SbComm", DbType.Decimal, newItem.SBComm);
                ObjectDb.Received(1).AddInParameter("NGComm", DbType.Decimal, newItem.NGComm);
                ObjectDb.Received(1).AddInParameter("RNGComm", DbType.Decimal, newItem.CSComm);
                ObjectDb.Received(1).AddInParameter("IsUpdateNG", DbType.Boolean, newItem.IsUpdateNG);
                ObjectDb.Received(1).AddInParameter("IsUpdateRNG", DbType.Boolean, newItem.IsUpdateCS);
                ObjectDb.Received(1).AddInParameter("GroupColor", DbType.String, newItem.GroupColor);
                ObjectDb.Received(1).AddInParameter("UserId", DbType.Int32, UserId);
                ObjectDb.Received(1).AddInParameter("Ip", DbType.String, request.Ip);
                ObjectDb.Received(1).AddInParameter("langkey", DbType.String, request.Language);
                ObjectDb.Received(1).AddInParameter("IsDefault", DbType.Boolean, newItem.IsDefaultGroup);
                ObjectDb.Received(1).AddInParameter("IsResetDefault", DbType.Boolean, newItem.IsResetDefault);
                ObjectDb.Received(1).AddOutParameter("StrCustID", DbType.String, int.MaxValue);
                ObjectDb.Received(1).AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 4000);
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
                ObjectDb.Received(1).AddInParameter("LicenseeId", DbType.Int32, LicenseeId);
                ObjectDb.Received(1).AddInParameter("GroupName", DbType.String, GroupName);
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
                ObjectDb.Received(1).AddInParameter("LicenseeId", DbType.Int32, LicenseeId);
                ObjectDb.Received(1).AddInParameter("GroupCommId", DbType.Int32, GroupId);
                ObjectDb.Received(1).AddInParameter("UserId", DbType.Int32, UserId);
                ObjectDb.Received(1).AddInParameter("Ip", DbType.String, Ip);
                ObjectDb.Received(1).AddInParameter("langkey", DbType.String, Language);
                ObjectDb.Received(1).AddInParameter("IsRemovePlayercomm", DbType.Boolean, IsRemoveCommSetting);
                ObjectDb.Received(1).AddInParameter("IsResetDefault", DbType.Boolean, IsResetDefault);
                ObjectDb.Received(1).AddOutParameter("StrCustID", DbType.String, int.MaxValue);
                ObjectDb.Received(1).AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }
    }
}