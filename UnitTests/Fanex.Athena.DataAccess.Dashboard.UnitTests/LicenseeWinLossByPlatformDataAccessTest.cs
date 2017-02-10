namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    using System.Collections.Generic;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LicenseeWinLossByPlatformDataAccessTest
    {
        private LicenseeWinLossByPlatformDataAccess _licenseeWinLossByPlatformDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _licenseeWinLossByPlatformDataAccess = Substitute.ForPartsOf<LicenseeWinLossByPlatformDataAccess>();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _licenseeWinLossByPlatformDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var reader = _licenseeWinLossByPlatformDataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetLicenseeWLByPlatform"))
                .ExecuteReader(Arg.Any<object>());

            var expected = new WidgetByPlatformViewModel
            {
                Data = new List<KeyValuePair<string, decimal>>
                {
                    new KeyValuePair<string, decimal>("Asia", 100M),
                    new KeyValuePair<string, decimal>("China", 300M),
                    new KeyValuePair<string, decimal>("WAP", 800M),
                    new KeyValuePair<string, decimal>("Smartsite", 700M),
                    new KeyValuePair<string, decimal>("Others", 1700M)
                }
            };
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };
            reader.Read().Returns(true, false);
            reader["PlatformType"].Returns(1);
            reader["LicenseeWLEUR"].Returns(100M);

            // Action
            var result = _licenseeWinLossByPlatformDataAccess.GetData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(1));
                Assert.That(result.Total, Is.EqualTo(100M));
                Assert.That(result.Data[0].Key, Is.EqualTo(expected.Data[0].Key));
                Assert.That(result.Data[0].Value, Is.EqualTo(expected.Data[0].Value));
            });
        }
    }
}