namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using Customer;
    using Data;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerDataAccessTest
    {
        private IObjectDb _objectDb;
        private CustomerDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<CustomerDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void SearchCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelUserSearch")).Returns(_objectDb);
            const int UserId = 0;
            var userName = string.Empty;

            // Action
            _dataAccess.SearchCustomers(UserId, userName, string.Empty);

            // Assert
            _objectDb.Received().AddInParameter("userid", DbType.Int32, UserId);
            _objectDb.Received().AddInParameter("username", DbType.String, userName);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, string.Empty);
        }

        [Test]
        public void Search_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSel")).Returns(_objectDb);
            var metaModel = new CustomerListMetaModel();
            var totalRows = 0;

            // Action
            _dataAccess.Search(metaModel, ref totalRows);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
            _objectDb.Received().AddInParameter("username", DbType.String, metaModel.Username);
            _objectDb.Received().AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
            _objectDb.Received().AddInParameter("PageIndex ", DbType.Int32, metaModel.PageIndex);
            _objectDb.Received().AddInParameter("BAFilter", DbType.Int32, metaModel.BAFilter);
            _objectDb.Received().AddInParameter("CustStatus", DbType.String, metaModel.CustStatus ?? string.Empty);
            _objectDb.Received().AddInParameter("SpecialLevel", DbType.Int32, metaModel.SpecialLevel);
            _objectDb.Received().AddInParameter("GroupCommId", DbType.Int32, metaModel.GroupCommId);
            _objectDb.Received().AddOutParameter("TotalRows", DbType.Int32, 4);
        }

        [Test]
        public void GetCustomerInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelStatus")).Returns(_objectDb);
            const int CustId = 0;

            // Action
            _dataAccess.GetCustomerInfo(CustId);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, CustId);
        }

        [Test]
        public void GetCustomerLoginInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerLoginInfo")).Returns(_objectDb);
            const int CustId = 0;
            const int LicenseeId = 0;

            // Action
            _dataAccess.GetCustomerLoginInfo(CustId, LicenseeId);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, CustId);
            _objectDb.Received().AddInParameter("licenseeId", DbType.Int32, LicenseeId);
        }

        [Test]
        public void MoveCustomersToGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerMoveGroupComm")).Returns(_objectDb);
            var metaModel = new MoveToGroupComMetaModel();

            // Action
            _dataAccess.MoveCustomersToGroup(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("GroupCommId", DbType.Int32, metaModel.GroupComId);
            _objectDb.Received().AddInParameter("IsUpdateNG", DbType.Boolean, metaModel.IsAllowedNG);
            _objectDb.Received().AddInParameter("IsUpdateRacing", DbType.Boolean, metaModel.IsAllowedRC);
            _objectDb.Received().AddInParameter("IsUpdateLC", DbType.Boolean, metaModel.IsAllowedLC);
            _objectDb.Received().AddInParameter("IsUpdateRNG", DbType.Boolean, metaModel.IsAllowedRNG);
            _objectDb.Received().AddInParameter("Ip", DbType.String, metaModel.Ip);
            _objectDb.Received().AddInParameter("langkey", DbType.String, metaModel.Language);
            _objectDb.Received().AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);
            _objectDb.Received().AddOutParameter("strCustID", DbType.String, int.MaxValue);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void RemoveCustomersFromGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerRemoveGroupComm")).Returns(_objectDb);
            var metaModel = new RemoveFromGroupComMetaModel();

            // Action
            _dataAccess.RemoveCustomersFromGroup(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("Ip", DbType.String, metaModel.Ip);
            _objectDb.Received().AddInParameter("IsRemovePlayercomm", DbType.Boolean, metaModel.IsRemovePlayercomm);
            _objectDb.Received().AddInParameter("langkey", DbType.String, metaModel.Language);
            _objectDb.Received().AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);
            _objectDb.Received().AddOutParameter("strCustID", DbType.String, int.MaxValue);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void UpdateOddsGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdUgroup")).Returns(_objectDb);
            var metaModel = new CustomerOddsGroupMetaModel();

            // Action
            _dataAccess.UpdateOddsGroup(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("CustId", DbType.Int32, metaModel.CustId);
            _objectDb.Received().AddInParameter("OddsGroup", DbType.String, metaModel.OddsGroup);
            _objectDb.Received().AddInParameter("IP", DbType.String, metaModel.Ip);
            _objectDb.Received().AddInParameter("Langkey", DbType.String, metaModel.Language);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void UpdateCustClosed_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdClosed")).Returns(_objectDb);
            var metaModel = new CustomerStatusMetaModel();

            // Action
            _dataAccess.UpdateCustClosed(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeId", DbType.String, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("UserId", DbType.String, metaModel.ModifyById);
            _objectDb.Received().AddInParameter("CustId", DbType.String, metaModel.CustId);
            _objectDb.Received().AddInParameter("Closed", DbType.String, metaModel.IsOn);
            _objectDb.Received().AddInParameter("IP", DbType.String, metaModel.Ip);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void UpdateCustSuspend_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdSuspended")).Returns(_objectDb);
            var metaModel = new CustomerStatusMetaModel();

            // Action
            _dataAccess.UpdateCustSuspend(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeId", DbType.String, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("LicenseeId", DbType.String, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("UserId", DbType.String, metaModel.ModifyById);
            _objectDb.Received().AddInParameter("CustId", DbType.String, metaModel.CustId);
            _objectDb.Received().AddInParameter("Suspended", DbType.String, metaModel.IsOn);
            _objectDb.Received().AddInParameter("IP", DbType.String, metaModel.Ip);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void UpdateCustomerBAStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdMemberBAStatus")).Returns(_objectDb);
            _objectDb.GetParameterValue("OddsGroup").Returns(string.Empty);
            const int LicenseeId = 0;
            const int CustId = 0;
            const int ModifiedBy = 0;
            const bool AllowBA = false;
            var request = new Request();

            // Action
            _dataAccess.UpdateCustomerBAStatus(LicenseeId, CustId, ModifiedBy, AllowBA, request);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, LicenseeId);
            _objectDb.Received().AddInParameter("CustID", DbType.String, CustId);
            _objectDb.Received().AddInParameter("ModifyBy", DbType.String, ModifiedBy);
            _objectDb.Received().AddInParameter("IsBA", DbType.Boolean, AllowBA);
            _objectDb.Received().AddInParameter("langkey", DbType.String, request.Language);
            _objectDb.Received().AddInParameter("IP", DbType.String, request.Ip);
            _objectDb.Received().AddOutParameter("OddsGroup", DbType.String, 10);
        }

        [Test]
        public void UpdateCustomersCommission_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeCustCommUpd")).Returns(_objectDb);
            var metaModel = new UpdatedCustomerComMetaModel();

            // Action
            _dataAccess.UpdateCustomersCommission(metaModel);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            _objectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("SBComm", DbType.Decimal, metaModel.SBComm);
            _objectDb.Received().AddInParameter("NGComm", DbType.Decimal, metaModel.NGComm);
            _objectDb.Received().AddInParameter("RacingComm", DbType.Decimal, metaModel.RCComm);
            _objectDb.Received().AddInParameter("LCComm", DbType.Decimal, metaModel.LCComm);
            _objectDb.Received().AddInParameter("RNGComm", DbType.Decimal, metaModel.CSComm);
            _objectDb.Received().AddInParameter("IsUpdateNG", DbType.Boolean, metaModel.IsAllowedNG);
            _objectDb.Received().AddInParameter("IsUpdateRacing", DbType.Boolean, metaModel.IsAllowedRC);
            _objectDb.Received().AddInParameter("IsUpdateLC", DbType.Boolean, metaModel.IsAllowedLC);
            _objectDb.Received().AddInParameter("IsUpdateRNG", DbType.Boolean, metaModel.IsAllowedCS);
            _objectDb.Received().AddInParameter("Ip", DbType.String, metaModel.Ip);
            _objectDb.Received().AddInParameter("langkey", DbType.String, metaModel.Language);
            _objectDb.Received().AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);
            _objectDb.Received().AddOutParameter("strCustID", DbType.String, int.MaxValue);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void UpdateSpecialLevel_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdSpecialLevel")).Returns(_objectDb);
            const int UserId = 0;
            const int CustId = 0;
            const int Level = 0;
            const string Ip = "";
            const string Language = "";

            // Action
            _dataAccess.UpdateSpecialLevel(UserId, CustId, Level, Ip, Language);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, CustId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, UserId);
            _objectDb.Received().AddInParameter("speciallevel", DbType.Int32, Level);
            _objectDb.Received().AddInParameter("ip", DbType.String, Ip);
            _objectDb.Received().AddInParameter("langkey", DbType.String, Language);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void UpdateOddsGroup_WhenCallWithFiveParameters_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdOddsGroup")).Returns(_objectDb);
            const int UserId = 0;
            const int CustId = 0;
            const string Group = "", Ip = "", Language = "";

            // Action
            _dataAccess.UpdateOddsGroup(UserId, CustId, Group, Ip, Language);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, CustId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, UserId);
            _objectDb.Received().AddInParameter("oddsgroup", DbType.String, Group);
            _objectDb.Received().AddInParameter("ip", DbType.String, Ip);
            _objectDb.Received().AddInParameter("langkey", DbType.String, Language);
        }
    }
}