namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
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
        [Test]
        public void SearchCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelUserSearch")).Returns(ObjectDb);
            const int UserId = 0;
            const string UserName = "";

            // Action
            DataAccess.SearchCustomers(UserId, UserName, string.Empty);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("userid", DbType.Int32, UserId);
                ObjectDb.Received().AddInParameter("username", DbType.String, UserName);
                ObjectDb.Received().AddInParameter("AgentList", DbType.String, string.Empty);
            });
        }

        [TestCase("New", "New")]
        [TestCase(null, "")]
        public void Search_WhenCall_ParametersPassedCorrectly(string custStatus, string expectedStatusParam)
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
            DataAccess.Search(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received().AddInParameter("LicenseeID", DbType.Int32, metaModel.LicenseeId);
                ObjectDb.Received().AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received().AddInParameter("username", DbType.String, metaModel.Username);
                ObjectDb.Received().AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received().AddInParameter("PageIndex ", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received().AddInParameter("BAFilter", DbType.Int32, metaModel.BAFilter);
                ObjectDb.Received().AddInParameter("CustStatus", DbType.String, expectedStatusParam);
                ObjectDb.Received().AddInParameter("SpecialLevel", DbType.Int32, metaModel.SpecialLevel);
                ObjectDb.Received().AddInParameter("GroupCommId", DbType.Int32, metaModel.GroupCommId);
                ObjectDb.Received().AddInParameter("OddsGroups", DbType.String, "A,C");
                ObjectDb.Received().AddOutParameter("TotalRows", DbType.Int32, 4);
            });
        }

        [TestCase(true, true)]
        [TestCase(true, false)]
        [TestCase(false, true)]
        [TestCase(false, false)]
        public void Search_WhenCall_ReturnCorrectData(bool isClosed, bool isSuspended)
        {
            // Arrange
            const int ExpectedCustId = 999;
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSel")).Returns(ObjectDb);
            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(true, false);
            ObjectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            DataAccessTestHelper.MapReaderData(
                reader,
                new
                {
                    custid = ExpectedCustId,
                    closed = isClosed,
                    suspended = isSuspended
                });
            var expectedStatus = isClosed ? ViewResources.Closed : isSuspended ? ViewResources.Suspended : ViewResources.Open;

            var metaModel = new CustomerListMetaModel();

            // Action
            var result = DataAccess.Search(metaModel);
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

            // Action
            DataAccess.GetCustomerLoginInfo(CustId, LicenseeId);

            // Assert
            ObjectDb.Received().AddInParameter("custid", DbType.Int32, CustId);
            ObjectDb.Received().AddInParameter("licenseeId", DbType.Int32, LicenseeId);
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
                ObjectDb.Received().AddInParameter("UserId", DbType.Int32, metaModel.UserId);
                ObjectDb.Received().AddInParameter("CustId", DbType.Int32, metaModel.CustId);
                ObjectDb.Received().AddInParameter("OddsGroup", DbType.String, metaModel.OddsGroup);
                ObjectDb.Received().AddInParameter("IP", DbType.String, metaModel.Ip);
                ObjectDb.Received().AddInParameter("Langkey", DbType.String, metaModel.Language);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
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
                ObjectDb.Received().AddInParameter("LicenseeId", DbType.String, metaModel.LicenseeId);
                ObjectDb.Received().AddInParameter("UserId", DbType.String, metaModel.ModifyById);
                ObjectDb.Received().AddInParameter("CustId", DbType.String, metaModel.CustId);
                ObjectDb.Received().AddInParameter("Closed", DbType.String, metaModel.IsOn);
                ObjectDb.Received().AddInParameter("IP", DbType.String, metaModel.Ip);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
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
                ObjectDb.Received().AddInParameter("LicenseeId", DbType.String, metaModel.LicenseeId);
                ObjectDb.Received().AddInParameter("LicenseeId", DbType.String, metaModel.LicenseeId);
                ObjectDb.Received().AddInParameter("UserId", DbType.String, metaModel.ModifyById);
                ObjectDb.Received().AddInParameter("CustId", DbType.String, metaModel.CustId);
                ObjectDb.Received().AddInParameter("Suspended", DbType.String, metaModel.IsOn);
                ObjectDb.Received().AddInParameter("IP", DbType.String, metaModel.Ip);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
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
                ObjectDb.Received().AddInParameter("LicenseeID", DbType.Int32, LicenseeId);
                ObjectDb.Received().AddInParameter("CustID", DbType.String, CustId);
                ObjectDb.Received().AddInParameter("ModifyBy", DbType.String, ModifiedBy);
                ObjectDb.Received().AddInParameter("IsBA", DbType.Boolean, AllowBA);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, request.Language);
                ObjectDb.Received().AddInParameter("IP", DbType.String, request.Ip);
                ObjectDb.Received().AddOutParameter("OddsGroup", DbType.String, 10);
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
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, CustId);
                ObjectDb.Received().AddInParameter("userid", DbType.Int32, UserId);
                ObjectDb.Received().AddInParameter("speciallevel", DbType.Int32, Level);
                ObjectDb.Received().AddInParameter("ip", DbType.String, Ip);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, Language);
                ObjectDb.Received().AddOutParameter("ErrCode", DbType.Int32, 4);
                ObjectDb.Received().AddOutParameter("ErrMsg", DbType.String, 4000);
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
                ObjectDb.Received().AddInParameter("custid", DbType.Int32, CustId);
                ObjectDb.Received().AddInParameter("userid", DbType.Int32, UserId);
                ObjectDb.Received().AddInParameter("oddsgroup", DbType.String, Group);
                ObjectDb.Received().AddInParameter("ip", DbType.String, Ip);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, Language);
            });
        }
    }
}