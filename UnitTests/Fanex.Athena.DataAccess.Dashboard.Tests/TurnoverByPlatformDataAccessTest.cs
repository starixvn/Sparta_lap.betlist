namespace Fanex.Athena.DataAccess.Dashboard.Tests
{
    using System.Collections.Generic;
    using Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NUnit.Framework;

    [TestFixture]
    public class TurnoverByPlatformDataAccessTest
    {
        private IWidgetByPlatformDataAccess _turnoverByPlatformDataAccess;

        [OneTimeSetUp]
        public void SetUp()
        {
            _turnoverByPlatformDataAccess = new TurnoverByPlatformDataAccess();
        }

        [OneTimeTearDown]
        public void TearDown()
        {
            _turnoverByPlatformDataAccess = null;
        }

        [Test]
        public void GetData_WhenCall_ReturnCorrectData()
        {
            // Arrange
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
            var metaModel = new DashboardBaseModel();

            // Action
            var result = _turnoverByPlatformDataAccess.GetData(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(5));
                Assert.That(result.Data[0].Key, Is.EqualTo(expected.Data[0].Key));
                Assert.That(result.Data[0].Value, Is.EqualTo(expected.Data[0].Value));
                Assert.That(result.Data[2].Key, Is.EqualTo(expected.Data[2].Key));
                Assert.That(result.Data[2].Value, Is.EqualTo(expected.Data[2].Value));
                Assert.That(result.Data[4].Key, Is.EqualTo(expected.Data[4].Key));
                Assert.That(result.Data[4].Value, Is.EqualTo(expected.Data[4].Value));
            });
        }
    }
}