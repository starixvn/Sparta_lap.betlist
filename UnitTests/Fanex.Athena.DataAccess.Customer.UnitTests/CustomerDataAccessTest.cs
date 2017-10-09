namespace Fanex.Athena.DataAccess.Customer.UnitTests
{
    using System.Data;
    using Customer;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class CustomerDataAccessTest
    {
        private IObjectDb _objectDb;
        private CustomerDataAccess _dataAccess;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<CustomerDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
        }

        [Test]
        public void GetCustomerInfo_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelStatus")).Returns(_objectDb);
            const int CustId = 0;

            // Action
            _dataAccess.GetCustomerInfo(CustId);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, CustId);
        }
    }
}