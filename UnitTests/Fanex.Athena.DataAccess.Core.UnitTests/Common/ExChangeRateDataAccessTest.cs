namespace Fanex.Athena.DataAccess.Core.UnitTests.Common
{
    using System;
    using System.Data;
    using Core.Common;
    using Core.Exception;
    using Data;
    using NSubstitute;
    using NUnit.Framework;
    using TestHelpers;

    public class ExChangeRateDataAccessTest : BaseDataAccessHelperTest<ExChangeRateDataAccess>
    {
        private const string UspName = "Lrf_GetExchangeRateByDate";

        [Test]
        public void GetExChangeRate_WhenCall_ParameterPassedCorrectly()
        {
            // Arrange
            const string CurrencyCode = "USD";
            var datetime = new DateTime(2016, 8, 10);
            DataAccess.GetObjectDb(Arg.Is<string>(UspName)).Returns(ObjectDb);

            // Action
            DataAccess.GetExChangeRate(CurrencyCode, datetime);

            // Assert
            ObjectDb.AddInParameter("CurrencyCode", DbType.String, CurrencyCode);
            ObjectDb.AddInParameter("Date", DbType.DateTime, datetime);
        }

        [Test]
        public void GetExChangeRate_WhenCall_DataReturnCorrectly()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is<string>(UspName)).Returns(ObjectDb);
            MapReaderData(new
            {
                Usd = 1,
                Eur = 2,
                Gbp = 3
            });
            Reader.Read().Returns(true, false);

            // Action
            var result = DataAccess.GetExChangeRate(Arg.Any<string>(), Arg.Any<DateTime>());

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.USD, Is.EqualTo(1));
                Assert.That(result.EUR, Is.EqualTo(2));
                Assert.That(result.GBP, Is.EqualTo(3));
            });
        }

        [Test]
        public void GetExChangeRate_WhenThrowObjectDbException_ThrowDataAccessException()
        {
            // Arrange
            DataAccess.GetObjectDb(Arg.Is(UspName)).Returns(x => { throw new ObjectDbException(); });

            // Action
            TestDelegate testDelegate = () => DataAccess.GetExChangeRate(string.Empty, DateTime.Today);

            // Assert
            Assert.Throws<DataAccessException>(testDelegate, Arg.Is<string>(x => x.Contains(UspName)));
        }
    }
}