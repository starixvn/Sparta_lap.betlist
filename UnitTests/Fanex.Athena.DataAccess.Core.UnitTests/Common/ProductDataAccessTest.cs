namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Core.Common;
    using Data;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ProductDataAccessTest
    {
        private IObjectDb _objectDb;
        private ProductDataAccess _dataAccess;
        private IDataReader _reader;

        [SetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<ProductDataAccess>();
            _reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(_reader);
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

        [TestCase(true)]
        [TestCase(false)]
        public void GetProductAccessList_WhenCall_DataReturnedCorrectly(bool isActiveCustomerSummaryReport)
        {
            // Arrange
            var uspName = Arg.Is<string>(name => name == "Lrf_LicenseeProduct3rdSel_active" || name == "Lrf_LicenseeProduct3rdSel");
            _dataAccess.GetObjectDb(uspName).Returns(_objectDb);
            ConfigureReader();
            var licenseeId = 1;
            var expected = GetFakeProductAccessList();

            // Action
            var result = _dataAccess.GetProductAccessList(licenseeId, isActiveCustomerSummaryReport);

            // Assert
            Assert.That(result[0].ProductID, Is.EqualTo(0));
            Assert.That(result[0].ProductCode, Is.EqualTo("SB"));

            Assert.That(result[1].ProductID, Is.EqualTo(expected[0].ProductID));
            Assert.That(result[1].ProductCode, Is.EqualTo(expected[0].ProductCode));
            Assert.That(result[1].ProductName, Is.EqualTo(expected[0].ProductName));
            Assert.That(result[1].AllowCommission, Is.EqualTo(expected[0].AllowCommission));
            Assert.That(result[1].AllowWinloss, Is.EqualTo(expected[0].AllowWinloss));
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

        [TestCase(true)]
        [TestCase(false)]
        public void IsAllowedSaToteCurrency_WhenCall_ParametersPassedCorrectly(bool expected)
        {
            // Arrange
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CheckHRAllowedCurrency")).Returns(_objectDb);
            _objectDb.GetParameterValue("allowed").Returns(expected);
            var custId = 1;

            // Action
            var result = _dataAccess.IsAllowedSaToteCurrency(custId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        private void ConfigureReader()
        {
            _reader.Read().Returns(true, false);
            _reader["ProductID"].Returns(1);
            _reader["productprefix"].Returns("NG");
            _reader["comm"].Returns(true);
            _reader["winloss"].Returns(true);
        }

        private IList<Product> GetFakeProductAccessList()
        {
            return new List<Product>
            {
                new Product
                {
                    ProductID = 1,
                    ProductName = "Number Game",
                    ProductCode = "NG",
                    AllowCommission = true,
                    AllowWinloss = true
                }
            };
        }
    }
}