namespace Fanex.Athena.Service.Core.UnitTests.Common
{
    using System;
    using Core.Common;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ExChangeRateServiceTest
    {
        private IExChangeRateDataAccess _exchangeRateDataAccess;
        private IExChangeRateService _exchangeRateService;

        [SetUp]
        public void SetUp()
        {
            _exchangeRateDataAccess = Substitute.For<IExChangeRateDataAccess>();
            _exchangeRateService = new ExChangeRateService(_exchangeRateDataAccess);
        }

        [Test]
        public void GetExChangeRate_WhenCall_ReturnResultFromDataAccess()
        {
            // Arrange
            var expectedResult = new ExChangeRate()
            {
                USD = 1,
                EUR = 2,
                GBP = 3
            };

            var currencyCode = "UUS";
            var date = new DateTime(2016, 1, 1);
            _exchangeRateDataAccess.GetExChangeRate(currencyCode, date).Returns(expectedResult);

            // Action
            var result = _exchangeRateService.GetExChangeRate(currencyCode, date);

            // Assert
            Assert.That(result.USD, Is.EqualTo(expectedResult.USD));
            Assert.That(result.EUR, Is.EqualTo(expectedResult.EUR));
            Assert.That(result.GBP, Is.EqualTo(expectedResult.GBP));
        }
    }
}