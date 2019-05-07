namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using Core.UnitTests.TestHelpers;
    using Customer;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerDataAccessTest : BaseDataAccessHelperTest<CustomerDataAccess>
    {
        [Test]
        public void GetCustomerInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int CustId = 0;

            DataAccess.GetObjectDb(Arg.Is("Lrf_CustomerSelStatus")).Returns(ObjectDb);

            // Action
            DataAccess.GetCustomerInfo(CustId);

            // Assert
            ObjectDb.Received(1).SetParameterValue("custid", CustId);
        }
    }
}