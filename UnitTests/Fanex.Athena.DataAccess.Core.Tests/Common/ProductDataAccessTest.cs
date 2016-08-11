namespace Fanex.Athena.DataAccess.Core.Tests.Common
{
    using System.Data;
    using Core.Common;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ProductDataAccessTest
    {
        private IObjectDb _objectDb;
        private ProductDataAccess _dataAccess;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<ProductDataAccess>();
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetProductAccessList_WhenCall_ParametersPassedCorrectly(bool isActiveCustomerSummaryReport)
        {
            // Arrange
            var uspName = Arg.Is<string>(name => name == "Lrf_LicenseeProduct3rdSel_active" || name == "Lrf_LicenseeProduct3rdSel");
            _dataAccess.GetObjectDb(uspName).Returns(_objectDb);
            var licenseeId = 1;

            // Action
            _dataAccess.GetProductAccessList(licenseeId, isActiveCustomerSummaryReport);

            // Assert
            _objectDb.Received().AddInParameter("licenseeid", DbType.Int32, licenseeId);
        }

        [Test]
        public void IsAllowedSaToteCurrency_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CheckHRAllowedCurrency")).Returns(_objectDb);
            var custId = 1;

            // Action
            _dataAccess.IsAllowedSaToteCurrency(custId);

            // Assert
            _objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            _objectDb.Received().AddOutParameter("allowed", DbType.Boolean, sizeof(bool));
        }
    }
}