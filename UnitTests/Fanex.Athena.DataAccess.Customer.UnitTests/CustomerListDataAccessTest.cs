namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using System.Threading.Tasks;
    using Core.UnitTests.TestHelpers;
    using Customer;
    using Model.Core.DTO.Common;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Resources;
    using UnitTest.Common.Helpers;

    public class CustomerListDataAccessTest : BaseDataAccessHelperTest<CustomerListDataAccess>
    {
        [TestCase("New", "New")]
        [TestCase(null, "")]
        public async Task GetActiveCustomers_WhenCall_ParametersPassedCorrectly(string custStatus, string expectedStatusParam)
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSel")).Returns(ObjectDb);
            var metaModel = new CustomerListMetaModel
            {
                OddsGroups = new List<CheckedItem>
                {
                    new CheckedItem { Checked = true, Name = "A" },
                    new CheckedItem { Checked = false, Name = "B" },
                    new CheckedItem { Checked = true, Name = "C" }
                },
                CustStatus = custStatus
            };

            // Action
            await DataAccess.GetActiveCustomers(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeID", metaModel.LicenseeId);
                ObjectDb.Received(1).SetParameterValue("AgentList", metaModel.AgentListXml);
                ObjectDb.Received(1).SetParameterValue("username", metaModel.Username);
                ObjectDb.Received(1).SetParameterValue("PageSize", metaModel.PageSize);
                ObjectDb.Received(1).SetParameterValue("PageIndex", metaModel.PageIndex);
                ObjectDb.Received(1).SetParameterValue("BAFilter", metaModel.BAFilter);
                ObjectDb.Received(1).SetParameterValue("CustStatus", expectedStatusParam);
                ObjectDb.Received(1).SetParameterValue("SpecialLevel", metaModel.SpecialLevel);
                ObjectDb.Received(1).SetParameterValue("GroupCommId", metaModel.GroupCommId);
                ObjectDb.Received(1).SetParameterValue("OddsGroups", "A,C");
                ObjectDb.Received(1).SetParameterValue("TotalRows", null);
            });
        }

        [TestCase(true, true, "Closed")]
        [TestCase(true, false, "Closed")]
        [TestCase(false, true, "Suspended")]
        [TestCase(false, false, "Open")]
        public async Task GetActiveCustomers_WhenCall_ReturnCorrectData(bool isClosed, bool isSuspended, string expectedStatusRes)
        {
            // Arrange
            const int ExpectedCustId = 999;
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSel")).Returns(ObjectDb);
            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(true, false);
            ObjectDb.ExecuteReaderAsync().ReturnsForAnyArgs(reader);
            DataAccessTestHelper.MapReaderData(
                reader,
                new
                {
                    custid = ExpectedCustId,
                    closed = isClosed,
                    suspended = isSuspended
                });
            var expectedStatus = ViewResources.ResourceManager.GetString(expectedStatusRes);

            var metaModel = new CustomerListMetaModel();

            // Action
            var result = await DataAccess.GetActiveCustomers(metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.CustId, Is.EqualTo(ExpectedCustId));
                Assert.That(firstItem.Status, Is.EqualTo(expectedStatus));
            });
        }

        [Test]
        public void GetCustomerLoginInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerLoginInfo")).Returns(ObjectDb);
            const int CustId = 0;
            const int LicenseeId = 0;
            const string LicenseeName = "Name";

            // Action
            DataAccess.GetCustomerLoginInfo(CustId, LicenseeId, LicenseeName);

            // Assert
            ObjectDb.Received(1).SetParameterValue("custid", CustId);
            ObjectDb.Received(1).SetParameterValue("licenseeId", LicenseeId);
            ObjectDb.Received(1).SetParameterValue("siteName", LicenseeName);
        }

        [Test]
        public void UpdateOddsGroup_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdUgroup")).Returns(ObjectDb);
            var metaModel = new CustomerOddsGroupMetaModel();

            // Action
            DataAccess.UpdateOddsGroup(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("UserId", metaModel.UserId);
                ObjectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
                ObjectDb.Received(1).SetParameterValue("OddsGroup", metaModel.OddsGroup);
                ObjectDb.Received(1).SetParameterValue("IP", metaModel.Ip);
                ObjectDb.Received(1).SetParameterValue("Langkey", metaModel.Language);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }

        [Test]
        public void UpdateCustClosed_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdClosed")).Returns(ObjectDb);
            var metaModel = new CustomerStatusMetaModel();

            // Action
            DataAccess.UpdateCustClosed(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeId", metaModel.LicenseeId);
                ObjectDb.Received(1).SetParameterValue("UserId", metaModel.ModifyById);
                ObjectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
                ObjectDb.Received(1).SetParameterValue("Closed", metaModel.IsOn);
                ObjectDb.Received(1).SetParameterValue("IP", metaModel.Ip);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }

        [Test]
        public void UpdateCustSuspend_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdSuspended")).Returns(ObjectDb);
            var metaModel = new CustomerStatusMetaModel();

            // Action
            DataAccess.UpdateCustSuspend(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeId", metaModel.LicenseeId);
                ObjectDb.Received(1).SetParameterValue("LicenseeId", metaModel.LicenseeId);
                ObjectDb.Received(1).SetParameterValue("UserId", metaModel.ModifyById);
                ObjectDb.Received(1).SetParameterValue("CustId", metaModel.CustId);
                ObjectDb.Received(1).SetParameterValue("Suspended", metaModel.IsOn);
                ObjectDb.Received(1).SetParameterValue("IP", metaModel.Ip);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }

        [Test]
        public void UpdateCustomerBAStatus_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdMemberBAStatus")).Returns(ObjectDb);
            ObjectDb.GetParameterValue("OddsGroup").Returns(string.Empty);
            const int LicenseeId = 0;
            const int CustId = 0;
            const int ModifiedBy = 0;
            const bool AllowBA = false;
            var request = new Request();

            // Action
            DataAccess.UpdateCustomerBAStatus(LicenseeId, CustId, ModifiedBy, AllowBA, request);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("LicenseeID", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("CustID", CustId);
                ObjectDb.Received(1).SetParameterValue("ModifyBy", ModifiedBy);
                ObjectDb.Received(1).SetParameterValue("IsBA", AllowBA);
                ObjectDb.Received(1).SetParameterValue("langkey", request.Language);
                ObjectDb.Received(1).SetParameterValue("IP", request.Ip);
                ObjectDb.Received(1).SetParameterValue("OddsGroup", null);
            });
        }

        [Test]
        public void UpdateSpecialLevel_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdSpecialLevel")).Returns(ObjectDb);
            const int UserId = 0;
            const int CustId = 0;
            const int Level = 0;
            const string Ip = "";
            const string Language = "";

            // Action
            DataAccess.UpdateSpecialLevel(UserId, CustId, Level, Ip, Language);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("custid", CustId);
                ObjectDb.Received(1).SetParameterValue("userid", UserId);
                ObjectDb.Received(1).SetParameterValue("speciallevel", Level);
                ObjectDb.Received(1).SetParameterValue("ip", Ip);
                ObjectDb.Received(1).SetParameterValue("langkey", Language);
                ObjectDb.Received(1).SetParameterValue("ErrCode", null);
                ObjectDb.Received(1).SetParameterValue("ErrMsg", null);
            });
        }

        [Test]
        public void UpdateOddsGroup_WhenCallWithFiveParameters_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerUpdOddsGroup")).Returns(ObjectDb);
            const int UserId = 0;
            const int CustId = 0;
            const string Group = "";
            const string Ip = "";
            const string Language = "";

            // Action
            DataAccess.UpdateOddsGroup(UserId, CustId, Group, Ip, Language);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("custid", CustId);
                ObjectDb.Received(1).SetParameterValue("userid", UserId);
                ObjectDb.Received(1).SetParameterValue("oddsgroup", Group);
                ObjectDb.Received(1).SetParameterValue("ip", Ip);
                ObjectDb.Received(1).SetParameterValue("langkey", Language);
            });
        }
    }
}