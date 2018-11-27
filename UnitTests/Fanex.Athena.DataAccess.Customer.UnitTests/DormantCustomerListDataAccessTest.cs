namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Customer;
    using Model.Customer.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Helpers;

    public class DormantCustomerListDataAccessTest : BaseDataAccessHelperTest<DormantCustomerListDataAccess>
    {
        [Test]
        public void GetDormantCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_DormantCustomerSel")).Returns(ObjectDb);
            var metaModel = new CustomerListMetaModel
            {
                Username = "abc",
                PageIndex = 12,
                PageSize = 2323,
                AgentListXml = "sdflskdjflksjdlfkjsdf"
            };

            // Action
            DataAccess.GetDormantCustomers(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("username", DbType.String, metaModel.Username);
                ObjectDb.Received(1).AddInParameter("PageSize", DbType.Int32, metaModel.PageSize);
                ObjectDb.Received(1).AddInParameter("PageIndex ", DbType.Int32, metaModel.PageIndex);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                ObjectDb.Received(1).AddOutParameter("TotalRows", DbType.Int32, 4);
            });
        }

        [Test]
        public void GetDormantCustomers_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int ExpectedCustId = 999;
            const string ExpectedUserName = "BillDate";
            const string ExpectedNickName = "Bill";
            const decimal ExpectedBalance = 123.5m;
            var expectedArchivedDate = new DateTime(2018, 4, 5);
            DataAccess.GetObjectDb(Arg.Is("Lrf_DormantCustomerSel")).Returns(ObjectDb);
            var reader = Substitute.For<IDataReader>();
            reader.Read().Returns(true, false);
            ObjectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            DataAccessTestHelper.MapReaderData(
                reader,
                new
                {
                    custid = ExpectedCustId,
                    username = ExpectedUserName,
                    nickname = ExpectedNickName,
                    BalanceGBP = ExpectedBalance,
                    ArchivedDate = expectedArchivedDate
                });

            var metaModel = new CustomerListMetaModel { BaseCurrency = "GBP" };

            // Action
            var result = DataAccess.GetDormantCustomers(metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.CustId, Is.EqualTo(ExpectedCustId));
                Assert.That(firstItem.SystemId, Is.EqualTo(ExpectedUserName));
                Assert.That(firstItem.ExternalId, Is.EqualTo(ExpectedNickName));
                Assert.That(firstItem.Balance, Is.EqualTo(ExpectedBalance));
                Assert.That(firstItem.ArchivedDate, Is.EqualTo(expectedArchivedDate));
            });
        }
    }
}