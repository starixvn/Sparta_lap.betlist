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
                ObjectDb.Received(1).SetParameterValue("LicenseeId", metaModel.LicenseeId);
                ObjectDb.Received(1).SetParameterValue("UserId", metaModel.UserId);
                ObjectDb.Received(1).SetParameterValue("GroupCommId", metaModel.GroupComId);
                ObjectDb.Received(1).SetParameterValue("IsUpdateNG", metaModel.IsAllowedNG);
                ObjectDb.Received(1).SetParameterValue("IsUpdateRNG", metaModel.IsAllowedRNG);
                ObjectDb.Received(1).SetParameterValue("Ip", metaModel.Ip);
                ObjectDb.Received(1).SetParameterValue("langkey", metaModel.Language);
                ObjectDb.Received(1).SetParameterValue("strCustIDOriginal", metaModel.CustomerToSQLParam);
                ObjectDb.Received(1).SetParameterValue("strCustID", null);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
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
                ObjectDb.Received(1).SetParameterValue("LicenseeId", metaModel.LicenseeId);
                ObjectDb.Received(1).SetParameterValue("UserId", metaModel.UserId);
                ObjectDb.Received(1).SetParameterValue("Ip", metaModel.Ip);
                ObjectDb.Received(1).SetParameterValue("IsRemovePlayercomm", metaModel.IsRemovePlayercomm);
                ObjectDb.Received(1).SetParameterValue("langkey", metaModel.Language);
                ObjectDb.Received(1).SetParameterValue("strCustIDOriginal", metaModel.CustomerToSQLParam);
                ObjectDb.Received(1).SetParameterValue("strCustID", null);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
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
                ObjectDb.Received(1).SetParameterValue("LicenseeId", metaModel.LicenseeId);
                ObjectDb.Received(1).SetParameterValue("UserId", metaModel.UserId);
                ObjectDb.Received(1).SetParameterValue("SBComm", metaModel.SBComm);
                ObjectDb.Received(1).SetParameterValue("NGComm", metaModel.NGComm);
                ObjectDb.Received(1).SetParameterValue("RNGComm", metaModel.CSComm);
                ObjectDb.Received(1).SetParameterValue("IsUpdateNG", metaModel.IsAllowedNG);
                ObjectDb.Received(1).SetParameterValue("IsUpdateRNG", metaModel.IsAllowedCS);
                ObjectDb.Received(1).SetParameterValue("Ip", metaModel.Ip);
                ObjectDb.Received(1).SetParameterValue("langkey", metaModel.Language);
                ObjectDb.Received(1).SetParameterValue("strCustIDOriginal", metaModel.CustomerToSQLParam);
                ObjectDb.Received(1).SetParameterValue("strCustID", null);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }
    }
}