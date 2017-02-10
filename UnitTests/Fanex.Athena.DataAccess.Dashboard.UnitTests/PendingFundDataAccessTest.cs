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