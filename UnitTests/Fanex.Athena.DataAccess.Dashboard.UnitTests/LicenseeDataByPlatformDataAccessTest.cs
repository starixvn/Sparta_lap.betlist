namespace Fanex.Athena.DataAccess.Dashboard.UnitTests
{
    using System.Collections.Generic;
    using Core.UnitTests.TestHelpers;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class LicenseeDataByPlatformDataAccessTest : BaseDataAccessHelperTest<LicenseeDataByPlatformDataAccess>
    {
        private const string GetLicenseeWLByPlatformSp = "Lrf_GetLicenseeWLByPlatform";

        private const string GetLicenseeTurnoverByPlatformSp = "Lrf_GetLicenseeTurnoverByPlatform";

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is(GetLicenseeWLByPlatformSp)).Returns(ObjectDb);
            DataAccess.GetObjectDb(Arg.Is(GetLicenseeTurnoverByPlatformSp)).Returns(ObjectDb);
        }

        [Test]
        public void GetWinLoss_WhenCall_PassCorrectParam()
        {
            // Arrange
            object passedParam = null;
            ObjectDb.When(x => x.ExecuteReader(Arg.Any<object>())).Do(x => passedParam = x[0]);
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR",
                LicenseeId = 1,
                CurrencyIDList = "1,2,3",
                FromDate = new System.DateTime(2017, 1, 1),
                ToDate = new System.DateTime(2017, 2, 15)
            };

            Reader.Read().Returns(true, false);
            Reader["PlatformType"].Returns(8);
            Reader["LicenseeWLEUR"].Returns(100M);

            // Action
            DataAccess.GetWinLoss(metaModel);
            var type = passedParam.GetType();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(type.GetProperty("LicenseeID").GetValue(passedParam, null), Is.EqualTo(metaModel.LicenseeId));
                Assert.That(type.GetProperty("CurrencyIDList").GetValue(passedParam, null), Is.EqualTo(metaModel.CurrencyIDList));
                Assert.That(type.GetProperty("FromDate").GetValue(passedParam, null), Is.EqualTo(metaModel.FromDate));
                Assert.That(type.GetProperty("ToDate").GetValue(passedParam, null), Is.EqualTo(metaModel.ToDate));
            });
        }

        [Test]
        public void GetWinLoss_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new WidgetByPlatformViewModel
            {
                Data = new List<KeyValuePair<string, decimal>>
                {
                    new KeyValuePair<string, decimal>(nameof(PlatformKey.Asia), 100M),
                }
            };
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };
            Reader.Read().Returns(true, false);
            Reader["PlatformType"].Returns(PlatformKey.Asia);
            Reader["LicenseeWLEUR"].Returns(100M);

            // Action
            var result = DataAccess.GetWinLoss(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(1));
                Assert.That(result.Total, Is.EqualTo(100M));
                Assert.That(result.Data[0].Key, Is.EqualTo(expected.Data[0].Key));
                Assert.That(result.Data[0].Value, Is.EqualTo(expected.Data[0].Value));
            });
        }

        [Test]
        public void GetWinLoss_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetLicenseeWLByPlatformSp,
                () => DataAccess.GetWinLoss(new DashboardBaseModel()));
        }

        [Test]
        public void GetTurnover_WhenCall_PassCorrectParam()
        {
            // Arrange
            object passedParam = null;
            ObjectDb.When(x => x.ExecuteReader(Arg.Any<object>())).Do(x => passedParam = x[0]);
            var metaModel = new DashboardBaseModel
            {
                BaseCurrency = "EUR",
                LicenseeId = 1,
                CurrencyIDList = "1,2,3",
                FromDate = new System.DateTime(2017, 1, 1),
                ToDate = new System.DateTime(2017, 2, 15)
            };

            Reader.Read().Returns(true, false);
            Reader["PlatformType"].Returns(8);
            Reader["TurnoverEUR"].Returns(100M);

            // Action
            DataAccess.GetTurnover(metaModel);
            var type = passedParam.GetType();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(type.GetProperty("LicenseeID").GetValue(passedParam, null), Is.EqualTo(metaModel.LicenseeId));
                Assert.That(type.GetProperty("CurrencyIDList").GetValue(passedParam, null), Is.EqualTo(metaModel.CurrencyIDList));
                Assert.That(type.GetProperty("FromDate").GetValue(passedParam, null), Is.EqualTo(metaModel.FromDate));
                Assert.That(type.GetProperty("ToDate").GetValue(passedParam, null), Is.EqualTo(metaModel.ToDate));
            });
        }

        [Test]
        public void GetTurnover_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new WidgetByPlatformViewModel
            {
                Data = new List<KeyValuePair<string, decimal>>
                {
                    new KeyValuePair<string, decimal>(nameof(PlatformKey.Asia), 100M),
                }
            };
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };
            Reader.Read().Returns(true, false);
            Reader["PlatformType"].Returns(PlatformKey.Asia);
            Reader["TurnoverEUR"].Returns(100M);

            // Action
            var result = DataAccess.GetTurnover(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(1));
                Assert.That(result.Total, Is.EqualTo(100M));
                Assert.That(result.Data[0].Key, Is.EqualTo(expected.Data[0].Key));
                Assert.That(result.Data[0].Value, Is.EqualTo(expected.Data[0].Value));
            });
        }

        [Test]
        public void GetTurnover_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                GetLicenseeTurnoverByPlatformSp,
                () => DataAccess.GetTurnover(new DashboardBaseModel()));
        }
    }
}