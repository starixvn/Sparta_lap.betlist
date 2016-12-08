namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Data;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;

    [TestFixture]
    public class ActiveCustomerSummaryDataAccessTest
    {
        private IObjectDb _objectDb;
        private ActiveCustomerSummaryDataAccess _dataAccess;
        private ActiveCustomerMetaModel _metaModel;

        [OneTimeSetUp]
        public void Setup()
        {
            _objectDb = Substitute.For<IObjectDb>();
            _dataAccess = Substitute.ForPartsOf<ActiveCustomerSummaryDataAccess>();
            _dataAccess.GetObjectDb(Arg.Is<string>("Lrf_Rpt_ActiveMember")).Returns(_objectDb);

            var productOptions = new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = true }
            };
            _metaModel = new ActiveCustomerMetaModel { ProductOptions = productOptions };
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _objectDb = null;
            _dataAccess = null;
            _metaModel = null;
        }

        [Test]
        public void GetActiveCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var totalCustomer = 0;
            _objectDb.GetParameterValue("TotalActive").Returns(5);

            // Action
            _dataAccess.GetActiveCustomers(_metaModel, out totalCustomer);

            // Assert
            _objectDb.AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
            _objectDb.AddInParameter("userid", DbType.Int32, _metaModel.UserId);
            _objectDb.AddInParameter("currencyid", DbType.Int32, _metaModel.CurrencyId);
            _objectDb.AddInParameter("fromdate", DbType.Date, _metaModel.FromDate);
            _objectDb.AddInParameter("todate", DbType.Date, _metaModel.ToDate);
            _objectDb.AddInParameter("products", DbType.String, "SB, NG");
            _objectDb.AddInParameter("excludetest", DbType.Boolean, _metaModel.IsExcludeTest);
            _objectDb.AddOutParameter("TotalActive", DbType.Int32, 10);
        }

        [Test]
        public void GetActiveCustomers_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var reader = Substitute.For<IDataReader>();
            _objectDb.ExecuteReader().Returns(reader);

            var totalCustomer = 0;
            _objectDb.GetParameterValue("TotalActive").Returns(5);
            var expected = new ActiveCustomer
            {
                TransactionDate = new DateTime(2016, 8, 29),
                TotalCustomer = 100
            };
            ConfigureReaderForGetActiveCustomersMethod(reader, expected);

            // Action
            var result = _dataAccess.GetActiveCustomers(_metaModel, out totalCustomer)[0];

            // Assert
            Assert.That(result.TransactionDate, Is.EqualTo(expected.TransactionDate));
            Assert.That(result.TotalCustomer, Is.EqualTo(expected.TotalCustomer));
            Assert.That(totalCustomer, Is.EqualTo(5));
        }

        private void ConfigureReaderForGetActiveCustomersMethod(IDataReader reader, ActiveCustomer expected)
        {
            reader.Read().Returns(true, false);

            reader["TransDate"].Returns(expected.TransactionDate);
            reader["MemCountPerMonth"].Returns(expected.TotalCustomer);
        }
    }
}