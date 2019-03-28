namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Data;
    using System.Threading.Tasks;
    using Constant.DataDomain;
    using Core.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;
    using VendorAdapter.Interfaces;

    public class SportTypeDataAccessTest : BaseDataAccessHelperTest<SportTypeDataAccess>
    {
        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<SportTypeDataAccess>(
                Substitute.For<ICacheRefAdapter>());
        }

        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Any<string>()).Returns(ObjectDb);
        }

        [Test]
        public async Task GetSportsAndProducts_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            const int LicenseeId = 1;
            const string LanguageCode = "en-US";

            // Action
            await DataAccess.GetSportsAndProducts(LicenseeId, LanguageCode);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("licenseeId", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("langkey", LanguageCode);
            });
        }

        [TestCase((int)SportType.UnknowThaiSport)]
        [TestCase((int)SportType.Cricket)]
        public async Task GetSportsAndProducts_HiddenSport_NotAddToResult(int sporttype)
        {
            // Arrange
            MapReaderData(new { ID = sporttype });
            Reader.Read().Returns(true, false);

            // Action
            var result = await DataAccess.GetSportsAndProducts(0, string.Empty);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public async Task GetSportsAndProducts_NotHiddenSport_AddToResult()
        {
            // Arrange
            MapReaderData(new { ID = SportType.AllBet });
            Reader.Read().Returns(true, false);

            // Action
            var result = await DataAccess.GetSportsAndProducts(0, string.Empty);

            // Assert
            Assert.That(result, Is.Not.Empty);
        }

        [Test]
        public async Task GetAllSports_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int SportType = 100;

            MapReaderData(new { Sporttype = SportType });
            Reader.Read().Returns(true, false);

            // Action
            var result = await DataAccess.GetAllSports().ConfigureAwait(false);

            // Assert
            Assert.That(result.ContainsKey(SportType), Is.True);
        }
    }
}