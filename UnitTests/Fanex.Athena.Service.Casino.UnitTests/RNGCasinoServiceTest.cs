namespace Fanex.Athena.Service.Customer.UnitTests
{
    using System;
    using System.Data;
    using Casino;
    using Casino.Wrappers;
    using Core.Common.Interfaces;
    using DataAccess.Core.Common.Interfaces;
    using Model.Casino.MetaModel;
    using Model.Core.DTO.Common;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class RNGCasinoServiceTest
    {
        private IReportApiWrapper _reportApiWrapper;

        private ILanguageService _languageService;

        private IExChangeRateDataAccess _exchangeRateDataAccess;

        private RNGCasinoService _rngCasinoService;

        [SetUp]
        public void SetUp()
        {
            _reportApiWrapper = Substitute.For<IReportApiWrapper>();
            _languageService = Substitute.For<ILanguageService>();
            _exchangeRateDataAccess = Substitute.For<IExChangeRateDataAccess>();

            _rngCasinoService = new RNGCasinoService(_reportApiWrapper, _languageService, _exchangeRateDataAccess);
        }

        [TearDown]
        public void TearDown()
        {
            _languageService = null;
        }

        [Test]
        public void GetRNGCasinoResult_WhenCall_ReturnDataFromApi()
        {
            // Arrange
            const string ExpectedResult = "<br /><img abc";
            var metaModel = new RNGCasinoResultMetaModel
            {
                IsShowPic = true,
                BaseCurrency = "abc"
            };
            var table = new DataTable();
            table.Columns.Add("Result_En", typeof(string));
            table.Rows.Add("<img abc");
            _reportApiWrapper.RealTimeWinLossHandHistory(DateTime.Now, DateTime.Now, 0, null, false)
                .ReturnsForAnyArgs(table);

            // Action
            var result = _rngCasinoService.GetRNGCasinoResult(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count, Is.EqualTo(1));
                Assert.That(result[0].Result, Is.EqualTo(ExpectedResult));
            });
        }

        [TestCase("EUR", 200)]
        [TestCase("usd", 300)]
        [TestCase("gbp", 400)]
        public void GetRNGCasinoResult_BaseCurrencyIsEUR_ReturnDataWithCorrectExchangeRate(
            string baseCurrency,
            decimal expectedBeginBalance)
        {
            // Arrange
            var metaModel = new RNGCasinoResultMetaModel
            {
                BaseCurrency = baseCurrency
            };
            _exchangeRateDataAccess.GetExChangeRate(null, DateTime.Now)
                .ReturnsForAnyArgs(new ExChangeRate
                {
                    EUR = 2,
                    USD = 3,
                    GBP = 4
                });
            var table = new DataTable();
            table.Columns.Add("BeginBalance", typeof(decimal));
            table.Rows.Add(100);
            _reportApiWrapper.RealTimeWinLossHandHistory(DateTime.Now, DateTime.Now, 0, null, false)
                .ReturnsForAnyArgs(table);

            // Action
            var result = _rngCasinoService.GetRNGCasinoResult(metaModel);

            // Assert
            Assert.That(result[0].BeginBalance, Is.EqualTo(expectedBeginBalance));
        }

        [Test]
        public void GetRNGCasinoResultDetail_WhenCall_ReturnDataFromApi()
        {
            // Arrange
            const string ExpectedUserName = "NoName";
            const decimal ExpectedBet = 100;
            var metaModel = new RNGCasinoResultDetailMetaModel
            {
                CurrencyName = "UUS"
            };
            var headTable = new DataTable();
            headTable.Columns.Add("UserName", typeof(string));
            headTable.Rows.Add(ExpectedUserName);
            var detailTable = new DataTable();
            detailTable.Columns.Add("Bet", typeof(decimal));
            detailTable.Rows.Add(ExpectedBet);
            _reportApiWrapper.WinLossDetail(0).ReturnsForAnyArgs(new[] { headTable, detailTable });

            // Action
            var result = _rngCasinoService.GetRNGCasinoResultDetail(metaModel);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.CasinoDetailHeaderList.Count, Is.EqualTo(1));
                Assert.That(result.CasinoDetailHeaderList[0].UserName, Is.EqualTo(ExpectedUserName));
                Assert.That(result.CasinoDetailList.Count, Is.EqualTo(1));
                Assert.That(result.CasinoDetailList[0].Bet, Is.EqualTo(ExpectedBet));
            });
        }
    }
}