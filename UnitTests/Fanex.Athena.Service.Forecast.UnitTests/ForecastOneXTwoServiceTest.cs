namespace Fanex.Athena.Service.Forecast.UnitTests
{
    using System.Collections.Generic;
    using DataAccess.Forecast.Interfaces;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.MetaModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ForecastOneXTwoServiceTest
    {
        private IForecastOneXTwoDataAccess _forecastOneXTwoDataAccess;
        private ForecastOneXTwoService _forecastOneXTwoService;

        [SetUp]
        public void SetUp()
        {
            _forecastOneXTwoDataAccess = Substitute.For<IForecastOneXTwoDataAccess>();
            _forecastOneXTwoService = new ForecastOneXTwoService(_forecastOneXTwoDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _forecastOneXTwoDataAccess = null;
            _forecastOneXTwoService = null;
        }

        [Test]
        public void GetForecastOneXTwoByMatch_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new ForecastOneXTwoMetaModel();
            var expectedResult = new List<ForecastOneXTwo>();
            _forecastOneXTwoDataAccess.GetForecastOneXTwoByMatch(metaModel).Returns(expectedResult);

            // Action
            var result = _forecastOneXTwoService.GetForecastOneXTwoByMatch(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }

        [Test]
        public void GetForecastOneXTwoByLeague_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var metaModel = new ForecastOneXTwoMetaModel();
            var expectedResult = new List<ForecastOneXTwo>();
            _forecastOneXTwoDataAccess.GetForecastOneXTwoByLeague(metaModel).Returns(expectedResult);

            // Action
            var result = _forecastOneXTwoService.GetForecastOneXTwoByLeague(metaModel);

            // Assert
            Assert.That(result, Is.EqualTo(expectedResult));
        }
    }
}