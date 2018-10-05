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
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerMoveGroupComm")).Returns(ObjectDb);
            var metaModel = new MoveToGroupComMetaModel();

            // Action
            DataAccess.MoveCustomersToGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                ObjectDb.Received(1).AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                ObjectDb.Received(1).AddInParameter("GroupCommId", DbType.Int32, metaModel.GroupComId);
                ObjectDb.Received(1).AddInParameter("IsUpdateNG", DbType.Boolean, metaModel.IsAllowedNG);
                ObjectDb.Received(1).AddInParameter("IsUpdateRNG", DbType.Boolean, metaModel.IsAllowedRNG);
                ObjectDb.Received(1).AddInParameter("Ip", DbType.String, metaModel.Ip);
                ObjectDb.Received(1).AddInParameter("langkey", DbType.String, metaModel.Language);
                ObjectDb.Received(1).AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);
                ObjectDb.Received(1).AddOutParameter("strCustID", DbType.String, int.MaxValue);
                ObjectDb.Received(1).AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void RemoveCustomersFromGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerRemoveGroupComm")).Returns(ObjectDb);
            var metaModel = new RemoveFromGroupComMetaModel();

            // Action
            DataAccess.RemoveCustomersFromGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                ObjectDb.Received(1).AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                ObjectDb.Received(1).AddInParameter("Ip", DbType.String, metaModel.Ip);
                ObjectDb.Received(1).AddInParameter("IsRemovePlayercomm", DbType.Boolean, metaModel.IsRemovePlayercomm);
                ObjectDb.Received(1).AddInParameter("langkey", DbType.String, metaModel.Language);
                ObjectDb.Received(1).AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);
                ObjectDb.Received(1).AddOutParameter("strCustID", DbType.String, int.MaxValue);
                ObjectDb.Received(1).AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }

        [Test]
        public void UpdateCustomersCommission_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeCustCommUpd")).Returns(ObjectDb);
            var metaModel = new UpdatedCustomerComMetaModel();

            // Action
            DataAccess.UpdateCustomersCommission(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
                ObjectDb.Received(1).AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                ObjectDb.Received(1).AddInParameter("SBComm", DbType.Decimal, metaModel.SBComm);
                ObjectDb.Received(1).AddInParameter("NGComm", DbType.Decimal, metaModel.NGComm);
                ObjectDb.Received(1).AddInParameter("RNGComm", DbType.Decimal, metaModel.CSComm);
                ObjectDb.Received(1).AddInParameter("IsUpdateNG", DbType.Boolean, metaModel.IsAllowedNG);
                ObjectDb.Received(1).AddInParameter("IsUpdateRNG", DbType.Boolean, metaModel.IsAllowedCS);
                ObjectDb.Received(1).AddInParameter("Ip", DbType.String, metaModel.Ip);
                ObjectDb.Received(1).AddInParameter("langkey", DbType.String, metaModel.Language);
                ObjectDb.Received(1).AddInParameter("strCustIDOriginal", DbType.String, metaModel.CustomerToSQLParam);
                ObjectDb.Received(1).AddOutParameter("strCustID", DbType.String, int.MaxValue);
                ObjectDb.Received(1).AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received(1).AddOutParameter("ErrMsg", DbType.String, 4000);
            });
        }
    }
}