namespace Fanex.Athena.DataAccess.Customer.Tests
{
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class GroupComDataAccessTest
    {
        private IObjectDb _objectDb;
        private GroupComDataAccess _dataAccess;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<GroupComDataAccess>();
        }

        [Test]
        public void GetAll_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeGroupCommSel")).Returns(_objectDb);
            var licenseeId = 0;

            // Action
            _dataAccess.GetAll(licenseeId);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, licenseeId);
        }

        [Test]
        public void GetDataForExcel_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeGroupCommSel")).Returns(_objectDb);
            _objectDb.ExecuteDataSet().Returns(new DataSet());
            var licenseeId = 0;

            // Action
            _dataAccess.GetDataForExcel(licenseeId);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, licenseeId);
        }

        [Test]
        public void GetById_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_GroupCommGetById")).Returns(_objectDb);
            var groupCommId = 0;

            // Action
            _dataAccess.GetById(groupCommId);

            // Assert
            _objectDb.Received().AddInParameter("GroupCommID", DbType.Int32, groupCommId);
        }

        [Test]
        public void InsertGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeGroupCommIns")).Returns(_objectDb);
            var item = new GroupCommMetaModel();
            var request = new Request();
            var userId = 0;
            IResultInfo resultInfo = new ResultInfo();

            // Action
            _dataAccess.InsertGroup(item, request, userId, out resultInfo);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, item.LicenseeId);
            _objectDb.Received().AddInParameter("GroupName", DbType.String, item.GroupName);
            _objectDb.Received().AddInParameter("SBComm", DbType.Decimal, item.SBComm);
            _objectDb.Received().AddInParameter("RacingComm", DbType.Decimal, item.RCComm);
            _objectDb.Received().AddInParameter("NGComm", DbType.Decimal, item.NGComm);
            _objectDb.Received().AddInParameter("LCComm", DbType.Decimal, item.LCComm);
            _objectDb.Received().AddInParameter("RNGComm", DbType.Decimal, item.CSComm);
            _objectDb.Received().AddInParameter("GroupColor", DbType.String, item.GroupColor);
            _objectDb.Received().AddInParameter("UserID", DbType.Int32, userId);
            _objectDb.Received().AddInParameter("IP", DbType.String, request.Ip);
            _objectDb.Received().AddInParameter("langkey", DbType.String, request.Language);
            _objectDb.Received().AddInParameter("IsDefault", DbType.Boolean, item.IsDefaultGroup);
            _objectDb.Received().AddInParameter("IsResetDefault", DbType.Boolean, item.IsResetDefault);
            _objectDb.Received().AddOutParameter("GroupCommID", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void UpdateGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeGroupCommUpd")).Returns(_objectDb);
            var newItem = new GroupCommMetaModel();
            var request = new Request();
            var userId = 0;
            IResultInfo resultInfo = new ResultInfo();

            // Action
            _dataAccess.UpdateGroup(newItem, request, userId, out resultInfo);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, newItem.LicenseeId);
            _objectDb.Received().AddInParameter("GroupCommId", DbType.Int32, newItem.GroupCommId);
            _objectDb.Received().AddInParameter("GroupName", DbType.String, newItem.GroupName);
            _objectDb.Received().AddInParameter("SbComm", DbType.Decimal, newItem.SBComm);
            _objectDb.Received().AddInParameter("NGComm", DbType.Decimal, newItem.NGComm);
            _objectDb.Received().AddInParameter("RacingComm", DbType.Decimal, newItem.RCComm);
            _objectDb.Received().AddInParameter("LCComm", DbType.Decimal, newItem.LCComm);
            _objectDb.Received().AddInParameter("RNGComm", DbType.Decimal, newItem.CSComm);
            _objectDb.Received().AddInParameter("IsUpdateNG", DbType.Boolean, newItem.IsUpdateNG);
            _objectDb.Received().AddInParameter("IsUpdateRacing", DbType.Boolean, newItem.IsUpdateRacing);
            _objectDb.Received().AddInParameter("IsUpdateLC", DbType.Boolean, newItem.IsUpdateLC);
            _objectDb.Received().AddInParameter("IsUpdateRNG", DbType.Boolean, newItem.IsUpdateCS);
            _objectDb.Received().AddInParameter("GroupColor", DbType.String, newItem.GroupColor);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, userId);
            _objectDb.Received().AddInParameter("Ip", DbType.String, request.Ip);
            _objectDb.Received().AddInParameter("langkey", DbType.String, request.Language);
            _objectDb.Received().AddInParameter("IsDefault", DbType.Boolean, newItem.IsDefaultGroup);
            _objectDb.Received().AddInParameter("IsResetDefault", DbType.Boolean, newItem.IsResetDefault);
            _objectDb.Received().AddOutParameter("StrCustID", DbType.String, int.MaxValue);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void CheckByName_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_GroupCommCheckByName")).Returns(_objectDb);
            var licenseeId = 0;
            var groupName = string.Empty;

            // Action
            _dataAccess.CheckByName(licenseeId, groupName);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("GroupName", DbType.String, groupName);
        }
    }
}
