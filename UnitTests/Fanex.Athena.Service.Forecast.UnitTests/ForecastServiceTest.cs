namespace Fanex.Athena.Service.Forecast.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using DataAccess.Forecast.Interfaces;
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ForecastServiceTest
    {
        private IForecastDataAccess _forecastDataAccess;
        private ForecastService _forecastService;

        [SetUp]
        public void SetUp()
        {
            _forecastDataAccess = Substitute.For<IForecastDataAccess>();
            _forecastService = new ForecastService(_forecastDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _forecastDataAccess = null;
            _forecastService = null;
        }

        [Test]
        public void GetForecastTopOddsByMatch_WhenCall_ReturnCorrectData()
        {
            // Arrange
            const int MatchId = 1, BetType = 2;
            var expectedResult = new ForecastAH();
            _forecastDataAccess.GetForecastTopOddsByMatch(MatchId, BetType).Returns(expectedResult);

            // Action
            var result = _forecastService.GetForecastTopOddsByMatch(MatchId, BetType);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetForecastHandicap_ReturnEmptyDataTableWithCallingDataAccess_ReturnHasDataValueIsFalse()
        {
            // Arrange
            var metaModel = new ForecastAHMetaModel();
            const int UserId = 1;
            var forecastHandicapTable = new DataTable();
            _forecastDataAccess.GetForecastHandicap(metaModel, UserId).Returns(forecastHandicapTable);

            // Action
            var result = _forecastService.GetForecastHandicap(metaModel, UserId);

            // Assert
            Assert.That(result.HasData, Is.False);
        }

        [Test]
        public void GetForecastHandicap_HasDataWithForecastScoreIsZero_ForeCastValue0IsZero()
        {
            // Arrange
            var metaModel = new ForecastAHMetaModel { CurrentScore = 3, BaseCurrency = "EUR" };
            const int UserId = 1;

            var fakeData = new Dictionary<string, object>();
            SetFakeForecastHandicapValue(fakeData, 0, 1m, 12m, 14m);
            var forecastHandicapTable = GetFakeDataTable(fakeData);

            _forecastDataAccess.GetForecastHandicap(metaModel, UserId).Returns(forecastHandicapTable);
            var expectedResult = new ForecastValue { ForeCastValue0 = 0 };

            // Action
            var result = _forecastService.GetForecastHandicap(metaModel, UserId);

            // Assert
            Assert.That(result.HasData, Is.True);
            Assert.That(result.ForeCastValue0, Is.EqualTo(expectedResult.ForeCastValue0));
        }

        [Test]
        public void GetForecastHandicap_HasDataWithForecastScoreGreaterThanZeroAndLessThanOneHalf_ForeCastValue1HasCorrectValue()
        {
            // Arrange
            var metaModel = new ForecastAHMetaModel { CurrentScore = 2, BaseCurrency = "EUR" };
            const int UserId = 1;

            var fakeData = new Dictionary<string, object>();
            SetFakeForecastHandicapValue(fakeData, 0, 0.6m, 12m, 14m);
            var forecastHandicapTable = GetFakeDataTable(fakeData);

            _forecastDataAccess.GetForecastHandicap(metaModel, UserId).Returns(forecastHandicapTable);
            var expectedResult = new ForecastValue { ForeCastValue1 = 6 };

            // Action
            var result = _forecastService.GetForecastHandicap(metaModel, UserId);

            // Assert
            Assert.That(result.HasData, Is.True);
            Assert.That(result.ForeCastValue1, Is.EqualTo(expectedResult.ForeCastValue1));
        }

        [Test]
        public void GetForecastHandicap_HasDataWithForecastScoreGreaterThanOneHalf_ForeCastValue2HasCorrectValue()
        {
            // Arrange
            var metaModel = new ForecastAHMetaModel { CurrentScore = 1, BaseCurrency = "EUR" };
            const int UserId = 1;

            var fakeData = new Dictionary<string, object>();
            SetFakeForecastHandicapValue(fakeData, 0, -4.6m, 12m, 14m);
            var forecastHandicapTable = GetFakeDataTable(fakeData);

            _forecastDataAccess.GetForecastHandicap(metaModel, UserId).Returns(forecastHandicapTable);
            var expectedResult = new ForecastValue { ForeCastValue2 = 12 };

            // Action
            var result = _forecastService.GetForecastHandicap(metaModel, UserId);

            // Assert
            Assert.That(result.HasData, Is.True);
            Assert.That(result.ForeCastValue2, Is.EqualTo(expectedResult.ForeCastValue2));
        }

        [Test]
        public void GetForecastHandicap_HasDataWithForecastScoreLessThanZeroAndGreaterThanOneHalfNegative_ForeCastValue3HasCorrectValue()
        {
            // Arrange
            var metaModel = new ForecastAHMetaModel { CurrentScore = 1, BaseCurrency = "EUR" };
            const int UserId = 1;

            var fakeData = new Dictionary<string, object>();
            SetFakeForecastHandicapValue(fakeData, 0, 2.4m, 12m, 14m);
            var forecastHandicapTable = GetFakeDataTable(fakeData);

            _forecastDataAccess.GetForecastHandicap(metaModel, UserId).Returns(forecastHandicapTable);
            var expectedResult = new ForecastValue { ForeCastValue3 = 7 };

            // Action
            var result = _forecastService.GetForecastHandicap(metaModel, UserId);

            // Assert
            Assert.That(result.HasData, Is.True);
            Assert.That(result.ForeCastValue3, Is.EqualTo(expectedResult.ForeCastValue3));
        }

        [Test]
        public void GetForecastHandicap_HasDataWithForecastScoreLessThanOneHalf_ForeCastValue4HasCorrectValue()
        {
            // Arrange
            var metaModel = new ForecastAHMetaModel { CurrentScore = 1, BaseCurrency = "EUR" };
            const int UserId = 1;

            var fakeData = new Dictionary<string, object>();
            SetFakeForecastHandicapValue(fakeData, 0, 3.6m, 12m, 14m);
            var forecastHandicapTable = GetFakeDataTable(fakeData);

            _forecastDataAccess.GetForecastHandicap(metaModel, UserId).Returns(forecastHandicapTable);
            var expectedResult = new ForecastValue { ForeCastValue4 = 14 };

            // Action
            var result = _forecastService.GetForecastHandicap(metaModel, UserId);

            // Assert
            Assert.That(result.HasData, Is.True);
            Assert.That(result.ForeCastValue4, Is.EqualTo(expectedResult.ForeCastValue4));
        }

        private static DataTable GetFakeDataTable(Dictionary<string, object> fakeData)
        {
            var dt = new DataTable();

            dt.Columns.Add("offset");
            dt.Columns.Add("aleur");
            dt.Columns.Add("hleur");

            var index = 0;
            DataRow dataRow = dt.NewRow();
            foreach (var item in fakeData)
            {
                dataRow[index] = item.Value;

                index++;
                if (index == 3)
                {
                    dt.Rows.Add(dataRow);
                    index = 0;
                }
            }

            return dt;
        }

        private static void SetFakeForecastHandicapValue(Dictionary<string, object> data, int index, decimal offsetValue, decimal alvalue, decimal hlvalue)
        {
            data.Add("offset" + index, offsetValue);
            data.Add("alEUR" + index, alvalue);
            data.Add("hlEUR" + index, hlvalue);
        }
    }
}