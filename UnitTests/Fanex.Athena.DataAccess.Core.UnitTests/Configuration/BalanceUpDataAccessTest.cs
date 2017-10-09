namespace Fanex.Athena.DataAccess.Core.UnitTests.Configuration
{
    using System;
    using System.Data;
    using Core.Configuration;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class BalanceUpDataAccessTest
    {
        [Test]
        public void GetBalanceUpDay_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var reader = Substitute.For<IDataReader>();
            var dataAccess = Substitute.ForPartsOf<BalanceUpDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_SelBalanceUpDay")).Returns(objectDb);
            objectDb.ExecuteReader().Returns(reader);
            reader.Read().Returns(true);
            reader["balanceupday"].Returns("1/1/2017");
            reader["movebetsday"].Returns("2/2/2017");

            // Action
            var result = dataAccess.GetBalanceUp();

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.BalanceUpDay, Is.EqualTo(new DateTime(2017, 1, 1)));
                Assert.That(result.MoveBetsDay, Is.EqualTo(new DateTime(2017, 2, 2)));
            });
        }
    }
}