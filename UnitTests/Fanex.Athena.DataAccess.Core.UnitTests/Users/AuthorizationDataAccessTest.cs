namespace Fanex.Athena.DataAccess.Core.UnitTests.Users
{
    using System.Data;
    using Core.Users;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    public class AuthorizationDataAccessTest : BaseDataAccessHelperTest<AuthorizationDataAccess>
    {
        private const string UspName = "Lrf_CustomerSelCheckBelonging";
        private const int LicenseeId = 1;
        private const int CustId = 2;
        private const long TransId = 3;

        [OneTimeSetUp]
        public override void FixtureSetUp()
        {
            base.FixtureSetUp();
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(ObjectDb);
        }

        [Test]
        public void IsCustomerBelonging_WhenCall_ParametersPassedCorrectly()
        {
            // Action
            DataAccess.IsCustomerBelonging(LicenseeId, CustId, TransId);

            // Assert
            Assert.Multiple(() =>
            {
                ObjectDb.Received(1).SetParameterValue("licenseeid", LicenseeId);
                ObjectDb.Received(1).SetParameterValue("transid", TransId);
                ObjectDb.Received(1).SetParameterValue("custid", CustId);
                ObjectDb.Received(1).SetParameterValue("status", null);
            });
        }

        [TestCase(true)]
        [TestCase(false)]
        public void IsCustomerBelonging_WhenCall_ValueReturnedCorrectly(bool expected)
        {
            // Arrange
            ObjectDb.GetParameterValue("status").Returns(expected);

            // Action
            var result = DataAccess.IsCustomerBelonging(LicenseeId, CustId, TransId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }
    }
}