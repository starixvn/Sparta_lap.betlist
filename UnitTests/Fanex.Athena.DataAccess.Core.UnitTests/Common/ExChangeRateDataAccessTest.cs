namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Data;
    using Core.Common;
    using Data;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ExChangeRateDataAccessTest
    {
        [Test]
        public void GetExChangeRate_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            var currencyCode = "USD";
            var datetime = new DateTime(2016, 8, 10);
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<ExChangeRateDataAccess>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetExchangeRateByDate")).Returns(objectDb);

            // Action
            dataAccess.GetExChangeRate(currencyCode, datetime);

            // Assert
            objectDb.AddInParameter("CurrencyCode", DbType.String, currencyCode);
            objectDb.AddInParameter("Date", DbType.DateTime, datetime);
        }

        [Test]
        public void GetExChangeRate_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            var objectDb = Substitute.For<IObjectDb>();
            var dataAccess = Substitute.ForPartsOf<ExChangeRateDataAccess>();
            var reader = Substitute.For<IDataReader>();
            dataAccess.GetObjectDb(Arg.Is<string>("Lrf_GetExchangeRateByDate")).Returns(objectDb);
            objectDb.ExecuteReader().ReturnsForAnyArgs(reader);
            reader.Read().Returns(true, false);
            reader["USD"].Returns(1);
            reader["EUR"].Returns(2);
            reader["GBP"].Returns(3);

            // Action
            var result = dataAccess.GetExChangeRate(Arg.Any<string>(), Arg.Any<DateTime>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.USD, Is.EqualTo(1));
                Assert.That(result.EUR, Is.EqualTo(2));
                Assert.That(result.GBP, Is.EqualTo(3));
            });
        }
    }
}