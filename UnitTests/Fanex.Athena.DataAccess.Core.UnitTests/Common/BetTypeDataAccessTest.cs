namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Data;
    using Core.Common;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;
    using VendorAdapter.Interfaces;

    public class BetTypeDataAccessTest : BaseDataAccessHelperTest<BetTypeDataAccess>
    {
        private const string UspName = "Lrf_SelBetTypeBySport";
        private const int LicenseeId = 1;
        private const string Sports = "1,2,3";
        private const string LanguageCode = "en-US";

        protected override void SetUpDataAccess()
        {
            DataAccess = Substitute.ForPartsOf<BetTypeDataAccess>(Substitute.For<ICacheRefAdapter>());
        }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
        }

        [Test]
        public void GetBettypes_WhenCall_PassCorrectParameters()
        {
            // Action
            DataAccess.GetBettypes(LicenseeId, Sports, LanguageCode);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("licenseeid", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("sportlist", Sports);
                ObjectDb.Received(1).SetParameterValue("langkey", LanguageCode);
            });
        }

        [Test]
        public void GetBettypes_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var expected = new CheckedItem { Checked = true, ID = 1, Name = "BettypeNameTest" };

            MapReaderData(new
            {
                BettypeID = expected.ID,
                BettypeName = expected.Name
            });
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetBettypes(LicenseeId, Sports, LanguageCode);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].Checked, Is.True);
                Assert.That(result[0].ID, Is.EqualTo(expected.ID));
                Assert.That(result[0].Name, Is.EqualTo(expected.Name));
            });
        }
    }
}