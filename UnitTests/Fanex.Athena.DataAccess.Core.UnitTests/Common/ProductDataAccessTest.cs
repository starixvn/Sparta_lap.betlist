namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System.Data;
    using System.Threading.Tasks;
    using Core.Common;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    public class ProductDataAccessTest : BaseDataAccessHelperTest<ProductDataAccess>
    {
        private const string SpName = "Lrf_LicenseeProduct3rdSel";
        private const string SpNameActive = "Lrf_LicenseeProduct3rdSel_active";
        private const int LicenseeId = 1;

        [TestCase(true)]
        [TestCase(false)]
        public void GetProductAccessList_WhenCall_ParametersPassedCorrectly(bool isActiveCustomerSummaryReport)
        {
            // Arrange
            var uspName = Arg.Is<string>(name => name == SpNameActive || name == SpName);
            DataAccess.GetObjectDb(uspName).Returns(ObjectDb);

            // Action
            DataAccess.GetProductAccessList(LicenseeId, isActiveCustomerSummaryReport);

            // Assert
            ObjectDb.Received(1).SetParameterValue("licenseeid", LicenseeId);
        }

        [TestCase(true)]
        [TestCase(false)]
        public void GetProductAccessList_WhenCall_DataReturnedCorrectly(bool isActiveCustomerSummaryReport)
        {
            // Arrange
            var uspName = Arg.Is<string>(name => name == SpNameActive || name == SpName);
            var expectedProductAccess = GetFakeProductAccess();

            DataAccess.GetObjectDb(uspName).Returns(ObjectDb);
            MapReaderData(new
            {
                ProductID = expectedProductAccess.ProductID,
                productprefix = "NG",
                comm = expectedProductAccess.AllowCommission,
                winloss = expectedProductAccess.AllowWinloss
            });
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetProductAccessList(LicenseeId, isActiveCustomerSummaryReport);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result[0].ProductID, Is.EqualTo(0));
                Assert.That(result[0].ProductCode, Is.EqualTo("SB"));

                Assert.That(result[1].ProductID, Is.EqualTo(expectedProductAccess.ProductID));
                Assert.That(result[1].ProductCode, Is.EqualTo(expectedProductAccess.ProductCode));
                Assert.That(result[1].ProductName, Is.EqualTo(expectedProductAccess.ProductName));
                Assert.That(result[1].AllowCommission, Is.EqualTo(expectedProductAccess.AllowCommission));
                Assert.That(result[1].AllowWinloss, Is.EqualTo(expectedProductAccess.AllowWinloss));
            });
        }

        private Product GetFakeProductAccess()
        {
            return new Product
            {
                ProductID = 1,
                ProductName = "Number Game",
                ProductCode = "NG",
                AllowCommission = true,
                AllowWinloss = true
            };
        }
    }
}