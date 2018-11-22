namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Customer;
    using NSubstitute;
    using NUnit.Framework;

    public class SearchCustomerDataAccessTest : BaseDataAccessHelperTest<SearchCustomerDataAccess>
    {
        [Test]
        public void SearchCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelUserSearch")).Returns(ObjectDb);
            const int UserId = 0;
            const string UserName = "";

            // Action
            DataAccess.SearchCustomers(UserId, UserName, string.Empty, true);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).AddInParameter("userid", DbType.Int32, UserId);
                ObjectDb.Received(1).AddInParameter("username", DbType.String, UserName);
                ObjectDb.Received(1).AddInParameter("AgentList", DbType.String, string.Empty);
                ObjectDb.Received(1).AddInParameter("IsIncludeDormant", DbType.Boolean, true);
            });
        }
    }
}