namespace Fanex.Athena.DataAccess.Core.Tests.Users
{
    using System.Data;
    using Core.Users;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class AuthorizationDataAccessTest
    {
        [Test]
        public void IsCustomerBelonging_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<AuthorizationDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_CustomerSelCheckBelonging")).Returns(objectDb);
            var licenseeId = 1;
            var custId = 2;
            long transId = 3;

            // Action
            dataAccess.IsCustomerBelonging(licenseeId, custId, transId);

            // Assert
            objectDb.Received().AddInParameter("licenseeid", DbType.Int32, licenseeId);
            objectDb.Received().AddInParameter("transid", DbType.Int64, transId);
            objectDb.Received().AddInParameter("custid", DbType.Int32, custId);
            objectDb.Received().AddOutParameter("status", DbType.Boolean, sizeof(bool));
        }
    }
}