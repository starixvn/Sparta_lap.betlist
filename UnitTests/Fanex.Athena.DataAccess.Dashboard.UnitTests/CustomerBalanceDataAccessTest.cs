namespace Fanex.Athena.DataAccess.Dashboard.UnitTests
{
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    public class CustomerBalanceDataAccessTest
    {
        private CustomerBalanceDataAccess _customerBalanceDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _customerBalanceDataAccess = Substitute.ForPartsOf<CustomerBalanceDataAccess>();
            _customerBalanceDataAccess.When(x => x.GetObjectDb(Arg.Any<string>())).DoNotCallBase();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _customerBalanceDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR", LicenseeId = 1, CurrencyId = 2, FromDate = new System.DateTime(2017, 1, 1) };
            object passedParam = null;
            var reader = _customerBalanceDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetCustomerBalance"))
                .ExecuteReader(Arg.Do<object>(x => passedParam = x));
            reader.Read().Returns(true, false);
            reader["ClosingBalanceEUR"].Returns(0M);

            // Action
            _customerBalanceDataAccess.GetData(metaModel);
            var type = passedParam.GetType();

            // Assert
            Assert.That(type.GetProperty("LicenseeID").GetValue(passedParam, null), Is.EqualTo(metaModel.LicenseeId));
            Assert.That(type.GetProperty("CurrencyID").GetValue(passedParam, null), Is.EqualTo(metaModel.CurrencyId));
            Assert.That(type.GetProperty("ReportDate").GetValue(passedParam, null), Is.EqualTo(metaModel.FromDate));
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const decimal Expected = 1570000M;
            var reader = _customerBalanceDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetCustomerBalance"))
                .ExecuteReader(Arg.Any<object>());
            reader.Read().Returns(true, false);
            reader["ClosingBalanceEUR"].Returns(Expected);

            // Action
            var result = _customerBalanceDataAccess.GetData(new DashboardBaseModel
            {
                BaseCurrency = "EUR"
            });

            // Assert
            Assert.That(result, Is.EqualTo(Expected));
        }
    }
}
