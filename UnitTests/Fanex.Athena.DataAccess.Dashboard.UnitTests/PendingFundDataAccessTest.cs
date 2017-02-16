namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class PendingFundDataAccessTest
    {
        private PendingFundDataAccess _pendingFundDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _pendingFundDataAccess = Substitute.ForPartsOf<PendingFundDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _pendingFundDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_PassCorrectParam()
        {
            // Arrange
            var metaModel = new DashboardBaseModel { LicenseeId = 1, CurrencyId = 2, BaseCurrency = "EUR" };
            object passedParam = null;
            var reader = _pendingFundDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetLicenseePendingFunds"))
                .ExecuteReader(Arg.Do<object>(x => passedParam = x));
            reader.Read().Returns(true, false);
            reader["PendingFundEUR"].Returns(0M);

            // Action
            _pendingFundDataAccess.GetData(metaModel);
            var type = passedParam.GetType();

            // Assert
            Assert.That(type.GetProperty("LicenseeID").GetValue(passedParam, null), Is.EqualTo(metaModel.LicenseeId));
            Assert.That(type.GetProperty("CurrencyID").GetValue(passedParam, null), Is.EqualTo(metaModel.CurrencyId));
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 250000M;

            var reader = _pendingFundDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetLicenseePendingFunds"))
                .ExecuteReader(Arg.Any<object>());
            reader.Read().Returns(true, false);
            reader["PendingFundEUR"].Returns(expected);

            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR"
            };

            // Action
            var result = _pendingFundDataAccess.GetData(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}