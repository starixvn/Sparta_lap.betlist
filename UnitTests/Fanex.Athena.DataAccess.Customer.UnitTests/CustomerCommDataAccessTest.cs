namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Customer;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerCommDataAccessTest : BaseDataAccessHelperTest<CustomerCommDataAccess>
    {
        [Test]
        public async Task MoveCustomersToGroupAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerMoveGroupComm")).Returns(ObjectDb);
            var metaModel = new MoveToGroupComMetaModel();

            // Action
            await DataAccess.MoveCustomersToGroupAsync(metaModel);

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
        public async Task RemoveCustomersFromGroupAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerRemoveGroupComm")).Returns(ObjectDb);
            var metaModel = new RemoveFromGroupComMetaModel();

            // Action
            await DataAccess.RemoveCustomersFromGroupAsync(metaModel);

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
        public async Task UpdateCustomersCommissionAsync_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_LicenseeCustCommUpd")).Returns(ObjectDb);
            var metaModel = new UpdatedCustomerComMetaModel();

            // Action
            await DataAccess.UpdateCustomersCommissionAsync(metaModel);

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