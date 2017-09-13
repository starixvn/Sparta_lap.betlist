namespace Fanex.Athena.Excel.Base.UnitTests
{
    using System.Collections.Generic;
    using Entities;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class ReportBaseTest
    {
        [Test]
        public void Export_WhenCall_AssignDataMethodIsCalled()
        {
            // Arrange
            var report = Substitute.ForPartsOf<ReportBase>("ExportFileName", new ShowingOption(), new ConvertingOption(), 0);
            report.When(x => x.AssignData()).DoNotCallBase();
            report.When(x => x.LoadTemplate()).DoNotCallBase();
            report.When(x => x.RemoveTemplate()).DoNotCallBase();

            // Action
            report.Export();

            // Assert
            report.Received().LoadTemplate();
            report.Received().AssignData();
            report.Received().RemoveTemplate();
        }

        [TestCase(0, "All Currencies")]
        [TestCase(-1, "View in EUR")]
        [TestCase(-2, "View in USD")]
        [TestCase(-3, "View in GBP")]
        public void GetExactTitleCurrencyName_CurrencyIdInRange_ReturnCorrectValue(int currencyId, string expected)
        {
            // Arrange
            var report = Substitute.ForPartsOf<ReportBase>("ExportFileName", new ShowingOption(), new ConvertingOption(), 0);

            // Action
            var result = report.GetExactTitleCurrencyName(currencyId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(-10, true, "ABC", "View in EUR")]
        [TestCase(10, true, "ABC", "View in EUR")]
        [TestCase(10, false, "ABC", "View in ABC")]
        public void GetExactTitleCurrencyName_CurrencyIdNotInRange_ReturnCorrectValue(int currencyId, bool convertOption, string currencyName, string expected)
        {
            // Arrange
            var convertingOption = new ConvertingOption { IsConvert = convertOption, BaseCurrency = "EUR", CurrencyName = currencyName };
            var report = Substitute.ForPartsOf<ReportBase>("ExportFileName", new ShowingOption(), convertingOption, 0);

            // Action
            var result = report.GetExactTitleCurrencyName(currencyId);

            // Assert
            Assert.That(result, Is.EqualTo(expected));
        }

        [TestCase(0, 2)]
        [TestCase(2, 0)]
        public void SetTextRowValue_WhenCall_SetCellValueMethodIsCalled(int sourceRowIndex, int destRowIndex)
        {
            // Arrange
            var report = Substitute.ForPartsOf<ReportBase>("ExportFileName", new ShowingOption(), new ConvertingOption(), 0);
            report.TemplateSheet = Substitute.For<ISheet>();
            var sheet = Substitute.For<ISheet>();
            var row = Substitute.For<IRow>();

            sheet.GetRow(destRowIndex).Returns(row);

            var cell = Substitute.For<ICell>();
            row.Cells.Returns(new List<ICell>() { cell });

            // Action
            report.SetTextRowValue(sheet, sourceRowIndex, destRowIndex, 1, "Test");

            // Assert
            cell.Received().SetCellValue("Test");
        }
    }
}