namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Customer;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerCommDataAccessTest : BaseDataAccessHelperTest<CustomerCommDataAccess>
    {
        [Test]
        public void MoveCustomersToGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerMoveGroupComm")).Returns(ObjectDb);
            var metaModel = new MoveToGroupComMetaModel();

            // Action
            DataAccess.MoveCustomersToGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                ObjectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                ObjectDb.Received().AddInParameter("GroupCommId", DbType.Int32, metaModel.GroupComId);
                ObjectDb.Received().AddInParameter("IsUpdateNG", DbType.Boolean, metaModel.IsAllowedNG);
                ObjectDb.Received().AddInParameter("IsUpdateRacing", DbType.Boolean, metaModel.IsAllowedRC);
                ObjectDb.Received().AddInParameter("IsUpdateLC", DbType.Boolean, metaModel.IsAllowedLC);
                ObjectDb.Received().AddInParameter("IsUpdateRNG", DbType.Boolean, metaModel.IsAllowedRNG);
                ObjectDb.Received().AddInParameter("IsUpdateAllbet", DbType.Boolean, metaModel.IsAllowedAB);
                ObjectDb.Received().AddInParameter("Ip", DbType.String, metaModel.Ip);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, metaModel.Language);
                ObjectDb.Received().AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);
                ObjectDb.Received().AddOutParameter("strCustID", DbType.String, int.MaxValue);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void RemoveCustomersFromGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerRemoveGroupComm")).Returns(ObjectDb);
            var metaModel = new RemoveFromGroupComMetaModel();

            // Action
            DataAccess.RemoveCustomersFromGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                ObjectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                ObjectDb.Received().AddInParameter("Ip", DbType.String, metaModel.Ip);
                ObjectDb.Received().AddInParameter("IsRemovePlayercomm", DbType.Boolean, metaModel.IsRemovePlayercomm);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, metaModel.Language);
                ObjectDb.Received().AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);
                ObjectDb.Received().AddOutParameter("strCustID", DbType.String, int.MaxValue);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void UpdateCustomersCommission_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is<string>("Lrf_LicenseeCustCommUpd")).Returns(ObjectDb);
            var metaModel = new UpdatedCustomerComMetaModel();

            // Action
            DataAccess.UpdateCustomersCommission(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                ObjectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                ObjectDb.Received().AddInParameter("SBComm", DbType.Decimal, metaModel.SBComm);
                ObjectDb.Received().AddInParameter("NGComm", DbType.Decimal, metaModel.NGComm);
                ObjectDb.Received().AddInParameter("RacingComm", DbType.Decimal, metaModel.RCComm);
                ObjectDb.Received().AddInParameter("LCComm", DbType.Decimal, metaModel.LCComm);
                ObjectDb.Received().AddInParameter("RNGComm", DbType.Decimal, metaModel.CSComm);
                ObjectDb.Received().AddInParameter("AllbetComm", DbType.Decimal, metaModel.ABComm);
                ObjectDb.Received().AddInParameter("IsUpdateNG", DbType.Boolean, metaModel.IsAllowedNG);
                ObjectDb.Received().AddInParameter("IsUpdateRacing", DbType.Boolean, metaModel.IsAllowedRC);
                ObjectDb.Received().AddInParameter("IsUpdateLC", DbType.Boolean, metaModel.IsAllowedLC);
                ObjectDb.Received().AddInParameter("IsUpdateRNG", DbType.Boolean, metaModel.IsAllowedCS);
                ObjectDb.Received().AddInParameter("IsUpdateAllbet", DbType.Boolean, metaModel.IsAllowedAB);
                ObjectDb.Received().AddInParameter("Ip", DbType.String, metaModel.Ip);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, metaModel.Language);
                ObjectDb.Received().AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);
                ObjectDb.Received().AddOutParameter("strCustID", DbType.String, int.MaxValue);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }
    }
}