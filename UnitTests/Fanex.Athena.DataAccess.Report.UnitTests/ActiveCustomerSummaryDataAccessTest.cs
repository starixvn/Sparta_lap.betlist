namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
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
            DataAccess = Substitute.ForPartsOf<ActiveCustomerSummaryDataAccess>();
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
                ObjectDb.SetParameterValue("licenseeid", _metaModel.LicenseeId);
                ObjectDb.SetParameterValue("userid", _metaModel.UserId);
                ObjectDb.SetParameterValue("currencyid", _metaModel.CurrencyId);
                ObjectDb.SetParameterValue("fromdate", _metaModel.FromDate);
                ObjectDb.SetParameterValue("todate", _metaModel.ToDate);
                ObjectDb.SetParameterValue("products", "SB, NG");
                ObjectDb.SetParameterValue("excludetest", _metaModel.IsExcludeTest);
                ObjectDb.SetParameterValue("TotalActive", null);
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

        private void ConfigureReaderForGetActiveCustomersMethod(ActiveCustomer expected)
        {
            Reader.Read().Returns(true, false);

            Reader["TransDate"].Returns(expected.TransactionDate);
            Reader["MemCountPerMonth"].Returns(expected.TotalCustomer);
        }
    }
}