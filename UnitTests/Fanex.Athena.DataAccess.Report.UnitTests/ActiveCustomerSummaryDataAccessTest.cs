namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Extension.Utility;
    using Model.Core.MetaModel.Product;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Report;

    [TestFixture]
    public class ActiveCustomerSummaryDataAccessTest : BaseDataAccessHelperTest<ActiveCustomerSummaryDataAccess>
    {
        private const string SpName = "Lrf_Rpt_ActiveMember";

        private ActiveCustomerMetaModel _metaModel;

        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<ActiveCustomerSummaryDataAccess>(Substitute.For<IAppSettings>(), CreateSessionStorage());
        }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is(SpName)).Returns(ObjectDb);

            var productOptions = new List<ProductOption>
            {
                new ProductOption { Code = "SB", Status = true },
                new ProductOption { Code = "NG", Status = true }
            };
            _metaModel = new ActiveCustomerMetaModel { ProductOptions = productOptions };
        }

        [Test]
        public void GetActiveCustomers_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            DataAccess.GetActiveCustomers(_metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.AddInParameter("licenseeid", DbType.Int32, _metaModel.LicenseeId);
                ObjectDb.AddInParameter("userid", DbType.Int32, _metaModel.UserId);
                ObjectDb.AddInParameter("currencyid", DbType.Int32, _metaModel.CurrencyId);
                ObjectDb.AddInParameter("fromdate", DbType.Date, _metaModel.FromDate);
                ObjectDb.AddInParameter("todate", DbType.Date, _metaModel.ToDate);
                ObjectDb.AddInParameter("products", DbType.String, "SB, NG");
                ObjectDb.AddInParameter("excludetest", DbType.Boolean, _metaModel.IsExcludeTest);
                ObjectDb.AddOutParameter("TotalActive", DbType.Int32, 10);
            });
        }

        [Test]
        public void GetActiveCustomers_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            const int TotalActive = 5;
            ObjectDb.GetParameterValue("TotalActive").Returns(TotalActive);

            var expected = new ActiveCustomer
            {
                TransactionDate = new DateTime(2016, 8, 29),
                TotalCustomer = 100
            };
            ConfigureReaderForGetActiveCustomersMethod(expected);

            var productCodeHasData = _metaModel.ProductOptions[0].Code;
            const int CustomerCount = 9;
            Reader.FieldCount.Returns(1);
            Reader.GetName(Arg.Any<int>()).Returns(productCodeHasData);
            Reader[productCodeHasData].Returns(CustomerCount);

            // Action
            var result = DataAccess.GetActiveCustomers(_metaModel);
            var firstItem = result.DataList.First();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(firstItem.TransactionDate, Is.EqualTo(expected.TransactionDate));
                Assert.That(firstItem.TotalCustomer, Is.EqualTo(expected.TotalCustomer));
                Assert.That(firstItem.CustomerCount[productCodeHasData], Is.EqualTo(CustomerCount));
                Assert.That(result.TotalCustomer, Is.EqualTo(TotalActive));
            });
        }

        [Test]
        public void GetActiveCustomers_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                SpName,
                () => DataAccess.GetActiveCustomers(_metaModel));
        }

        private void ConfigureReaderForGetActiveCustomersMethod(ActiveCustomer expected)
        {
            Reader.Read().Returns(true, false);

            Reader["TransDate"].Returns(expected.TransactionDate);
            Reader["MemCountPerMonth"].Returns(expected.TotalCustomer);
        }
    }
}