namespace Fanex.Athena.Excel.Base.UnitTests.Utility
{
    using System;
    using System.Data;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Excel.Base.Utility;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class DataHelperTest
    {
        [TestCase("LicenseeWL", "USD", "LicenseeWLUSD")]
        [TestCase("Commission", "EUR", "CommissionEUR")]
        [TestCase("", "", "")]
        public void GetCurrencyColumnName_IsConvertEqualTrue_ReturnColumnNameWithCurrencySuffix(string columnName, string baseCurrency, string expected)
        {
            // Arrange
            var convertOption = Substitute.For<ConvertingOption>();
            convertOption.IsConvert = true;
            convertOption.BaseCurrency = baseCurrency;

            // Action
            var result = DataHelper.GetCurrencyColumnName(columnName, convertOption);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("LicenseeWL", "LicenseeWL")]
        [TestCase("Commission", "Commission")]
        [TestCase("", "")]
        public void GetCurrencyColumnName_IsConvertEqualFalse_ReturnColumnNameWithoutCurrencySuffix(string columnName, string expected)
        {
            // Arrange
            var convertOption = Substitute.For<ConvertingOption>();
            convertOption.IsConvert = false;
            convertOption.BaseCurrency = "USD";

            // Action
            var result = DataHelper.GetCurrencyColumnName(columnName, convertOption);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase("LicenseeWL", 5.2)]
        [TestCase("Commission", 1)]
        public void GetCurrencyValue_ContainColumn_ReturnCorrectValueOfColumn(string columnName, object value)
        {
            // Arrange
            value = Convert.ToDecimal(value);
            var columnNameWithCurrency = columnName + "USD";
            var convertOption = new ConvertingOption()
            {
                IsConvert = true,
                BaseCurrency = "USD",
            };

            var row = GetDataRow(columnNameWithCurrency, value);

            // Action
            var result = DataHelper.GetCurrencyValue(row, columnName, convertOption);

            // Assert
            Assert.That(result, Is.EqualTo(value));
        }

        [Test]
        public void GetCurrencyValue_NotContainColumn_ReturnZero()
        {
            // Arrange
            var columnNameWithCurrency = "ColumnUSD";
            var convertOption = new ConvertingOption()
            {
                IsConvert = true,
                BaseCurrency = "USD",
            };

            var row = GetDataRow(columnNameWithCurrency, 10);

            // Action
            var result = DataHelper.GetCurrencyValue(row, "ColumnEUR", convertOption);

            // Assert
            Assert.That(result, Is.EqualTo(0));
        }

        private DataRow GetDataRow(string columnName, object value)
        {
            var dataTable = new DataTable();
            dataTable.Columns.Add(columnName);

            var dataRow = dataTable.NewRow();
            dataRow[columnName] = value;

            return dataRow;
        }
    }
}