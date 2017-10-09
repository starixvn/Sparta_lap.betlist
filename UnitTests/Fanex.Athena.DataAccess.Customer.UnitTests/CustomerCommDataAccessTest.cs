namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using Customer;
    using Data;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerCommDataAccessTest
    {
        private IObjectDb _objectDb;
        private CustomerCommDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<CustomerCommDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
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
    }
}