namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class GroupComDataAccessTest : BaseDataAccessHelperTest<GroupComDataAccess>
    {
        private const string GetLicenseeGroupCommSp = "Lrf_LicenseeGroupCommSel";
        private const int LicenseeId = 23340;

        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<GroupComDataAccess>();
        }

        [Test]
        public async Task GetAll_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLicenseeGroupCommSp)).Returns(ObjectDb);

            // Action
            await DataAccess.GetAll(LicenseeId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("LicenseeID", LicenseeId);
        }

        [Test]
        public async Task GetDataForExcel_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(GetLicenseeGroupCommSp)).Returns(ObjectDb);
            ObjectDb.ExecuteDataSetAsync().Returns(new DataSet());

            // Action
            await DataAccess.GetDataForExcel(LicenseeId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("LicenseeID", LicenseeId);
        }

        [Test]
        public async Task GetDataForExcel_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var dataTable = new DataTable();
            var dataSet = new DataSet();
            dataSet.Tables.Add(dataTable);

            DataAccess.GetObjectDb(Arg.Is(GetLicenseeGroupCommSp)).Returns(ObjectDb);
            ObjectDb.ExecuteDataSetAsync().Returns(dataSet);

            // Action
            var result = await DataAccess.GetDataForExcel(LicenseeId);

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
            DataAccess.GetById(GroupCommId, LicenseeId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeId", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("GroupCommID", GroupCommId);
            });
        }

        [Test]
        public async Task InsertGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int UserId = 1;

            DataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeGroupCommIns")).Returns(ObjectDb);
            var item = new GroupCommMetaModel();
            var request = new Request();

            // Action
            await DataAccess.InsertGroup(item, request, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeID", item.LicenseeId);
                ObjectDb.Received(1).SetParameterValue("GroupName", item.GroupName);
                ObjectDb.Received(1).SetParameterValue("SBComm", item.SBComm);
                ObjectDb.Received(1).SetParameterValue("NGComm", item.NGComm);
                ObjectDb.Received(1).SetParameterValue("RNGComm", item.CSComm);
                ObjectDb.Received(1).SetParameterValue("GroupColor", item.GroupColor);
                ObjectDb.Received(1).SetParameterValue("UserID", UserId);
                ObjectDb.Received(1).SetParameterValue("IP", request.Ip);
                ObjectDb.Received(1).SetParameterValue("langkey", request.Language);
                ObjectDb.Received(1).SetParameterValue("IsDefault", item.IsDefaultGroup);
                ObjectDb.Received(1).SetParameterValue("IsResetDefault", item.IsResetDefault);
                ObjectDb.Received(1).SetParameterValue("GroupCommID", null);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }

        [Test]
        public async Task UpdateGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int UserId = 0;

            DataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeGroupCommUpd")).Returns(ObjectDb);

            var newItem = new GroupCommMetaModel();
            var request = new Request();

            // Action
            await DataAccess.UpdateGroup(newItem, request, UserId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeID", newItem.LicenseeId);
                ObjectDb.Received(1).SetParameterValue("GroupCommId", newItem.GroupCommId);
                ObjectDb.Received(1).SetParameterValue("GroupName", newItem.GroupName);
                ObjectDb.Received(1).SetParameterValue("SbComm", newItem.SBComm);
                ObjectDb.Received(1).SetParameterValue("NGComm", newItem.NGComm);
                ObjectDb.Received(1).SetParameterValue("RNGComm", newItem.CSComm);
                ObjectDb.Received(1).SetParameterValue("IsUpdateNG", newItem.IsUpdateNG);
                ObjectDb.Received(1).SetParameterValue("IsUpdateRNG", newItem.IsUpdateCS);
                ObjectDb.Received(1).SetParameterValue("GroupColor", newItem.GroupColor);
                ObjectDb.Received(1).SetParameterValue("UserId", UserId);
                ObjectDb.Received(1).SetParameterValue("Ip", request.Ip);
                ObjectDb.Received(1).SetParameterValue("langkey", request.Language);
                ObjectDb.Received(1).SetParameterValue("IsDefault", newItem.IsDefaultGroup);
                ObjectDb.Received(1).SetParameterValue("IsResetDefault", newItem.IsResetDefault);
                ObjectDb.Received(1).SetParameterValue("StrCustID", null);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
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
                ObjectDb.Received(1).SetParameterValue("LicenseeId", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("GroupName", GroupName);
            });
        }

        [Test]
        public async Task DeleteGroup_WhenCall_ParametersPassedCorrectly()
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
            await DataAccess.DeleteGroup(GroupId, LicenseeId, UserId, Ip, Language, IsRemoveCommSetting, IsResetDefault);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeId", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("GroupCommId", GroupId);
                ObjectDb.Received(1).SetParameterValue("UserId", UserId);
                ObjectDb.Received(1).SetParameterValue("Ip", Ip);
                ObjectDb.Received(1).SetParameterValue("langkey", Language);
                ObjectDb.Received(1).SetParameterValue("IsRemovePlayercomm", IsRemoveCommSetting);
                ObjectDb.Received(1).SetParameterValue("IsResetDefault", IsResetDefault);
                ObjectDb.Received(1).SetParameterValue("StrCustID", null);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }
    }
}