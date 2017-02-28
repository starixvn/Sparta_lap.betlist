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
            var userId = 0;
            var userName = string.Empty;

            // Action
            _dataAccess.SearchCustomers(userId, userName, string.Empty);

            // Assert
            _objectDb.Received().AddInParameter("userid", DbType.Int32, userId);
            _objectDb.Received().AddInParameter("username", DbType.String, userName);
            _objectDb.Received().AddInParameter("agentlist", DbType.Int32, string.Empty);
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
            _objectDb.Received().AddInParameter("UserID", DbType.Int32, metaModel.UserId);
            _objectDb.Received().AddInParameter("CurrencyId", DbType.Int32, metaModel.CurrencyId);
            _objectDb.Received().AddInParameter("LicenseeType", DbType.String, metaModel.LicenseeType);
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
            var custId = 0;

            // Action
            _dataAccess.GetCustomerInfo(custId);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
        }

        [Test]
        public void GetCustomerLoginInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerLoginInfo")).Returns(_objectDb);
            var custId = 0;
            var licenseeId = 0;

            // Action
            _dataAccess.GetCustomerLoginInfo(custId, licenseeId);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            _objectDb.Received().AddInParameter("licenseeId", DbType.Int32, licenseeId);
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
            var licenseeId = 0;
            var custId = 0;
            var modifiedBy = 0;
            var allowBA = false;
            var request = new Request();

            // Action
            _dataAccess.UpdateCustomerBAStatus(licenseeId, custId, modifiedBy, allowBA, request);

            // Assert
            _objectDb.Received().AddInParameter("LicenseeID", DbType.Int32, licenseeId);
            _objectDb.Received().AddInParameter("CustID", DbType.String, custId);
            _objectDb.Received().AddInParameter("ModifyBy", DbType.String, modifiedBy);
            _objectDb.Received().AddInParameter("IsBA", DbType.Boolean, allowBA);
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
            var userId = 0;
            var custId = 0;
            var level = 0;
            var ip = string.Empty;
            var language = string.Empty;

            // Action
            _dataAccess.UpdateSpecialLevel(userId, custId, level, ip, language);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, userId);
            _objectDb.Received().AddInParameter("speciallevel", DbType.Int32, level);
            _objectDb.Received().AddInParameter("ip", DbType.String, ip);
            _objectDb.Received().AddInParameter("langkey", DbType.String, language);
            _objectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
            _objectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
        }

        [Test]
        public void UpdateOddsGroup_WhenCallWithFiveParameters_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerUpdOddsGroup")).Returns(_objectDb);
            var userId = 0;
            var custId = 0;
            var group = string.Empty;
            var ip = string.Empty;
            var language = string.Empty;

            // Action
            _dataAccess.UpdateOddsGroup(userId, custId, group, ip, language);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            _objectDb.Received().AddInParameter("userid", DbType.Int32, userId);
            _objectDb.Received().AddInParameter("oddsgroup", DbType.String, group);
            _objectDb.Received().AddInParameter("ip", DbType.String, ip);
            _objectDb.Received().AddInParameter("langkey", DbType.String, language);
        }
    }
}