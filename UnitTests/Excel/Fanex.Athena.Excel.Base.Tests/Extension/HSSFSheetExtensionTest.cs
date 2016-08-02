namespace Fanex.Athena.Excel.Base.Tests.Extension
{
    using Excel.Base.Extension;
    using NPOI.SS.UserModel;
    using NSubstitute;
    using NUnit.Framework;

    [TestFixture]
    public class HSSFSheetExtensionTest
    {
        [TestCase(0, 2)]
        [TestCase(2, 0)]
        public void CopyRow(int sourceRowIndex, int destRowIndex)
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            var sourceSheet = Substitute.For<ISheet>();

            // Action
            var row = sheet.CopyRow(sourceSheet, sourceRowIndex, destRowIndex);

            // Assert
            Assert.That(row, Is.Not.Null);
        }
    }
}