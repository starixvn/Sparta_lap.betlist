namespace Fanex.Athena.DataAccess.Dashboard.UnitTests
{
    using System.Collections;
    using Model.Dashboard.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class SingleValueDataAccessTest
    {
        private SingleValueDataAccess _dataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _dataAccess = Substitute.ForPartsOf<SingleValueDataAccess>();
            _dataAccess.When(x => x.GetObjectDb(Arg.Any<string>())).DoNotCallBase();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _dataAccess = null;
        }

        [Test]
        public void GetPendingFunds_WhenCall_PassCorrectParam()
        {
            // Arrange
            var metaModel = new DashboardBaseModel
            {
                LicenseeId = 1,
                CurrencyIDList = "1,2,3",
                BaseCurrency = "EUR"
            };
            Hashtable passedParam = null;
            var reader = _dataAccess.GetObjectDb(Arg.Is("Lrf_GetLicenseePendingFunds"))
                .ExecuteReader(Arg.Do<Hashtable>(x => passedParam = x));

            // Action
            _dataAccess.GetPendingFunds(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(passedParam.Count, Is.EqualTo(2));
                Assert.That(passedParam["LicenseeID"], Is.EqualTo(metaModel.LicenseeId));
                Assert.That(passedParam["CurrencyIDList"], Is.EqualTo(metaModel.CurrencyIDList));
            });
        }

        [Test]
        public void GetTodayWithdrawal_WhenCall_PassCorrectParam()
        {
            // Arrange
            var metaModel = new DashboardBaseModel
            {
                LicenseeId = 1,
                CurrencyIDList = "1,2,3",
                BaseCurrency = "EUR",
                FromDate = new System.DateTime(2017, 1, 1)
            };
            Hashtable passedParam = null;
            var reader = _dataAccess.GetObjectDb(Arg.Is("Lrf_GetLicenseeTodayWithdrawal"))
                .ExecuteReader(Arg.Do<Hashtable>(x => passedParam = x));

            // Action
            _dataAccess.GetTodayWithdrawal(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(passedParam.Count, Is.EqualTo(3));
                Assert.That(passedParam["LicenseeID"], Is.EqualTo(metaModel.LicenseeId));
                Assert.That(passedParam["CurrencyIDList"], Is.EqualTo(metaModel.CurrencyIDList));
                Assert.That(passedParam["ReportDate"], Is.EqualTo(metaModel.FromDate));
            });
        }

        [Test]
        public void GetTodayWithdrawal_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = 500000M;
            var reader = _dataAccess.GetObjectDb(Arg.Is("Lrf_GetLicenseeTodayWithdrawal"))
                .ExecuteReader(Arg.Any<object>());
            reader.Read().Returns(true, false);
            reader["WithdrawalEUR"].Returns(expected);

            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };

            // Action
            var result = _dataAccess.GetTodayWithdrawal(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}