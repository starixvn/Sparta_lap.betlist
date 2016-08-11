namespace Fanex.Athena.DataAccess.Core.Tests.Configuration
{
    using Core.Configuration;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BalanceUpDataAccessTest
    {
        [Test]
        public void GetBalanceUpDay_WhenCall_ExecuteReaderMethodIsCalled()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<BalanceUpDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelBalanceUpDay")).Returns(objectDb);

            // Action
            dataAccess.GetBalanceUpDay();

            // Assert
            objectDb.Received().ExecuteReader();
        }
    }
}