namespace Fanex.Athena.DataAccess.Core.Tests.Common
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
    }
}