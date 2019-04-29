namespace Fanex.Athena.Module.Dashboard.UnitTests.Data
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.DataAccess.Core.UnitTests.TestHelpers;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Module.Dashboard.Data.Dashboard;
    using Fanex.Athena.Module.Dashboard.MetaModel;
    using Fanex.Athena.Module.Dashboard.ViewModel;
    using Fanex.Athena.UnitTest.Common.Helpers;
    using NSubstitute;
    using NUnit.Framework;
    using UnitTest.Common.Extensions;
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
        public async Task GetWinLoss_WhenCall_PassCorrectParam()
        {
            // Arrange
            object passedParam = null;
            ObjectDb.When(x => x.ExecuteReaderAsync(Arg.Any<object>())).Do(x => passedParam = x[0]);
            var metaModel = FakeHelper.Create<DashboardBaseModel>(x => x.BaseCurrency, "EUR");

            Reader.Read().Returns(true, false);
            Reader["PlatformType"].Returns(8);
            Reader["LicenseeWLEUR"].Returns(100M);

            // Action
            await DataAccess.GetWinLoss(metaModel);

            // Assert
            AssertPassedParameter(passedParam, metaModel);
        }

        [Test]
        public async Task GetWinLoss_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new WidgetByPlatformViewModel
            {
                Data = new List<KeyValuePair<string, decimal>>
                {
                    new KeyValuePair<string, decimal>(nameof(PlatformType.Asia), 100M),
                }
            };
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };
            Reader.Read().Returns(true, false);
            Reader["PlatformType"].Returns(PlatformType.Asia);
            Reader["LicenseeWLEUR"].Returns(100M);

            // Action
            var result = await DataAccess.GetWinLoss(metaModel);

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
        public async Task GetTurnover_WhenCall_PassCorrectParam()
        {
            // Arrange
            object passedParam = null;
            ObjectDb.When(x => x.ExecuteReaderAsync(Arg.Any<object>())).Do(x => passedParam = x[0]);
            var metaModel = FakeHelper.Create<DashboardBaseModel>(x => x.BaseCurrency, "EUR");

            Reader.Read().Returns(true, false);
            Reader["PlatformType"].Returns(8);
            Reader["TurnoverEUR"].Returns(100M);

            // Action
            await DataAccess.GetTurnover(metaModel);

            // Assert
            AssertPassedParameter(passedParam, metaModel);
        }

        [Test]
        public async Task GetTurnover_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var expected = new WidgetByPlatformViewModel
            {
                Data = new List<KeyValuePair<string, decimal>>
                {
                    new KeyValuePair<string, decimal>(nameof(PlatformType.Asia), 100M),
                }
            };
            var metaModel = new DashboardBaseModel { BaseCurrency = "EUR" };
            Reader.Read().Returns(true, false);
            Reader["PlatformType"].Returns(PlatformType.Asia);
            Reader["TurnoverEUR"].Returns(100M);

            // Action
            var result = await DataAccess.GetTurnover(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Data.Count, Is.EqualTo(1));
                Assert.That(result.Total, Is.EqualTo(100M));
                Assert.That(result.Data[0].Key, Is.EqualTo(expected.Data[0].Key));
                Assert.That(result.Data[0].Value, Is.EqualTo(expected.Data[0].Value));
            });
        }

        private static void AssertPassedParameter(object passedParam, DashboardBaseModel metaModel)
        {
            Assert.Multiple(() =>
            {
                Assert.That(passedParam.GetValue("LicenseeID"), Is.EqualTo(metaModel.LicenseeId));
                Assert.That(passedParam.GetValue("CurrencyIDList"), Is.EqualTo(metaModel.CurrencyIDList));
                Assert.That(passedParam.GetValue("FromDate"), Is.EqualTo(metaModel.FromDate));
                Assert.That(passedParam.GetValue("ToDate"), Is.EqualTo(metaModel.ToDate));
            });
        }
    }
}