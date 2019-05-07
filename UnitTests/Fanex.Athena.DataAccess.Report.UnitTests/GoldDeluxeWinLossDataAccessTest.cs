namespace Fanex.Athena.DataAccess.Report.UnitTests
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Core.UnitTests.TestHelpers;
    using Model.Report.DTO;
    using Model.Report.MetaModel;
    using NSubstitute;
    using NUnit.Framework;
    using Testable;

    [TestFixture]
    public class GoldDeluxeWinLosDataAccessTest : BaseDataAccessHelperTest<GoldDeluxeWinLossDataAccessTestable>
    {
        [Test]
        public void ReadSummaryTotal_WhenCall_ReadDataCorrectly()
        {
            // Arrange
            MapReaderData(new
            {
                winlostUSD = 1,
                winlostEUR = 2,
                winlostGBP = 3
            });
            var summaryTotal = new Dictionary<string, GoldDeluxeWinLoss>
            {
                { "USD", new GoldDeluxeWinLoss() },
                { "EUR", new GoldDeluxeWinLoss() },
                { "GBP", new GoldDeluxeWinLoss() }
            };

            // Action
            DataAccess.Public_ReadSummaryTotal(Reader, summaryTotal);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(summaryTotal["USD"].CustomerWinLoss, Is.EqualTo(1));
                Assert.That(summaryTotal["EUR"].CustomerWinLoss, Is.EqualTo(2));
                Assert.That(summaryTotal["GBP"].CustomerWinLoss, Is.EqualTo(3));
            });
        }

        [Test]
        public void UpdateUspParams_WhenCall_ParametersPassedCorrectly()
        {
            // Arrange
            var metaModel = new ThirdPartyWinLossMetaModel
            {
                LicenseeId = 123
            };

            // Action
            DataAccess.Public_UpdateUspParams(ObjectDb, metaModel, true);

            // Assert
            ObjectDb.Received(1).SetParameterValue("licenseeId", metaModel.LicenseeId);
        }

        [Test]
        public void PostReadItem_WhenCall_ReadDataCorrectly()
        {
            // Arrange
            const string BaseCurrency = "ABC";
            MapReaderData(new
            {
                tipsamountABC = 1.5m,
                tipssharingABC = 2.4m,
                amountdueABC = 3.3m
            });
            var item = new GoldDeluxeWinLoss();

            // Action
            DataAccess.Public_PostReadItem(Reader, item, BaseCurrency);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(item.TipAmount, Is.EqualTo(1.5m));
                Assert.That(item.TipSharing, Is.EqualTo(2.4m));
                Assert.That(item.AmountDue, Is.EqualTo(3.3m));
            });
        }

        [Test]
        public void CalculateTotal_NoData_ReturnEmptyList()
        {
            // Arrange
            var dataList = new List<GoldDeluxeWinLoss>();

            // Action
            var result = DataAccess.Public_CalculateTotal(dataList);

            // Assert
            Assert.That(result, Is.Empty);
        }

        [Test]
        public void CalculateTotal_WhenCall_ReturnCorrectData()
        {
            // Arrange
            var dataList = new List<GoldDeluxeWinLoss>
            {
                new GoldDeluxeWinLoss { CurrencyName = "1", TipAmount = 2 },
                new GoldDeluxeWinLoss { CurrencyName = "1", TipAmount = 3 },
                new GoldDeluxeWinLoss { CurrencyName = "2", TipAmount = 4 }
            };
            
            // Action
            var result = DataAccess.Public_CalculateTotal(dataList);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(result.Count(), Is.EqualTo(2));
                Assert.That(result.ElementAt(0).TipAmount, Is.EqualTo(5));
                Assert.That(result.ElementAt(1).TipAmount, Is.EqualTo(4));
            });
        }
    }
}