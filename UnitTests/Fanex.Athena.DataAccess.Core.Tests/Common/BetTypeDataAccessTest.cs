namespace Fanex.Athena.DataAccess.Core.Tests.Common
{
    using System.Collections.Generic;
    using System.Data;
    using Core.Common;
    using Data;
    using Model.Core.MetaModel.Product;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BetTypeDataAccessTest
    {
        [Test]
        public void GetBettypes_WhenCall_PassCorrectParameters()
        {
            // Arrange
            int licenseeId = 1;
            var sports = "1,2,3";
            var languageCode = "en-US";

            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<BetTypeDataAccess>();
            dataAccess.GetObjectDb(Arg.Is("Lrf_SelBetTypeBySport")).Returns(objectDb);

            // Action
            dataAccess.GetBettypes(licenseeId, sports, languageCode);

            // Assert
            objectDb.Received().AddInParameter("licenseeid", DbType.Int32, licenseeId);
            objectDb.Received().AddInParameter("sportlist", DbType.String, sports);
            objectDb.Received().AddInParameter("langkey", DbType.String, languageCode);
        }

        [Test]
        public void GetAllBettypes_WhenCall_PassCorrectParameters()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<BetTypeDataAccess>();
            dataAccess.GetObjectDb(Arg.Is("Lrf_BetTypesGetAll")).Returns(objectDb);
            var productOptions = new List<ProductOption>
            {
                new ProductOption() { Code = "SB", Status = false },
                new ProductOption() { Code = "VB", Status = true }
            };

            // Action
            dataAccess.GetAllBettypes(productOptions);

            // Assert
            objectDb.Received().AddInParameter("showSB", DbType.Boolean, false);
            objectDb.Received().AddInParameter("showVB", DbType.Boolean, true);
        }
    }
}
