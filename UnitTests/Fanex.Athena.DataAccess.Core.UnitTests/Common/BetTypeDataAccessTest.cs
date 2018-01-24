namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Data;
    using Core.Common;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    public class BetTypeDataAccessTest : BaseDataAccessHelperTest<BetTypeDataAccess>
    {
        private const string UspName = "Lrf_SelBetTypeBySport";
        private const int LicenseeId = 1;
        private const string Sports = "1,2,3";
        private const string LanguageCode = "en-US";

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
                ObjectDb.Received().AddInParameter("licenseeid", DbType.Int32, LicenseeId);
                ObjectDb.Received().AddInParameter("sportlist", DbType.String, Sports);
                ObjectDb.Received().AddInParameter("langkey", DbType.String, LanguageCode);
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

        [Test]
        public void GetBettypes_WhenObjectDbThrowsException_ThrowDataAccessException()
        {
            TestException_WhenObjectDbThrowsException_ThrowDataAccessException(
                UspName,
                () => DataAccess.GetBettypes(0, string.Empty, string.Empty));
        }
    }
}