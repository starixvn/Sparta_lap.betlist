namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Data;
    using Core.Common;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;
    using VendorAdapter.Interfaces;

    public class BetTypeDataAccessTest_GetBettypesByProductByProduct : BaseDataAccessHelperTest<BetTypeDataAccess>
    {
        private const string UspName = "Lrf_SelBetTypesByProduct";
        private const int LicenseeId = 1;
        private const string ProductCode = "VS";
        private const string LanguageCode = "en-US";

        private ICacheRefAdapter _cacheRefAdapter;

        protected override void SetUpDataAccess()
        {
            _cacheRefAdapter = Substitute.For<ICacheRefAdapter>();
            DataAccess = Substitute.ForPartsOf<BetTypeDataAccess>(_cacheRefAdapter);
        }

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();

            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
        }

        [Test]
        public void GetBettypesByProduct_WhenCall_PassCorrectParameters()
        {
            // Action
            DataAccess.GetBettypesByProduct(LicenseeId, ProductCode, LanguageCode);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("licenseeid", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("productcode", ProductCode);
            });
        }

        [Test]
        public void GetBettypesByProduct_WhenCall_DataReturnedCorrectly()
        {
            // Arrange
            var expected = new CheckedItem { Checked = true, ID = 1, Name = "BettypeNameTest" };
            _cacheRefAdapter.GetBettype(expected.ID).Returns(expected.Name);
            MapReaderData(new
            {
                Bettype = expected.ID
            });
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetBettypesByProduct(LicenseeId, ProductCode, LanguageCode);

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