namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Model.Customer.MetaModel.CustomerLog;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerLogDataAccessTest : BaseDataAccessHelperTest<CustomerLogDataAccess>
    {
        [Test]
        public void GetCustomerSettingList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSettingLog_Sel")).Returns(ObjectDb);
            var metaModel = new CustomerLogMetaModel();

            // Action
            DataAccess.GetCustomerSettingList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("UserName", DbType.String, metaModel.UserName);
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("FilterBy", DbType.Int16, metaModel.ViewById);
                ObjectDb.Received(1).AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            });
        }

        [Test]
        public void GetGroupCommList_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_GroupCommLog_Sel")).Returns(ObjectDb);
            var metaModel = new CustomerLogMetaModel();

            // Action
            DataAccess.GetGroupCommList(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                ObjectDb.Received(1).AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                ObjectDb.Received(1).AddInParameter("LicenseeId", DbType.Int32, metaModel.LicenseeId);
            });
        }
    }
}