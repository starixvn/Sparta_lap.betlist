namespace Fanex.Athena.Excel.Base.UnitTests
{
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
            var report = Substitute.ForPartsOf<ReportBase>("ExportFileName", 0, new ShowingOption(), 0);
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

        [Test]
        public void ClearTemplate_WhenCall_CallReleventMethods()
        {
            // Arrange
            var sheet = Substitute.For<ISheet>();
            sheet.LastRowNum.Returns(1);

            // Action
            ReportBase.ClearTemplate(sheet, 0);

            // Assert
            Assert.Multiple(() =>
            {
                sheet.Received(1).GetRow(0);
                sheet.Received(1).GetRow(1);
                sheet.Received(2).RemoveRow(Arg.Any<IRow>());
            });
        }

        [Test]
        public void AssignData_TotalRowsIsZeno_CallReleventMethods()
        {
            // Arrange
            const int TotalRows = 0;
            var report = Substitute.ForPartsOf<ReportBase>(string.Empty, TotalRows, new ShowingOption(), 0);
            report.Workbook.Returns(Substitute.For<IWorkbook>());

            // Action
            report.AssignData();

            // Assert
            AssertNoInfoAssignSheet(report);
        }

        [Test]
        public void AssignData_TotalRowsGreaterThanZeno_CallAssignSheetData()
        {
            // Arrange
            const int TotalRows = 1;
            var report = Substitute.ForPartsOf<ReportBase>(string.Empty, TotalRows, new ShowingOption(), 0);
            report.Workbook.Returns(Substitute.For<IWorkbook>());

            // Action
            report.AssignData();

            // Assert
            Assert.Multiple(() =>
            {
                report.DidNotReceive().AssignNoInfoRow(Arg.Any<ISheet>());
                report.Received(1).AssignSheetData(Arg.Any<ISheet>());
            });
        }

        [Test]
        public void AssignDataMultiSheet_TotalRowsIsZeno_CallReleventMethods()
        {
            // Arrange
            const int TotalRows = 0;
            var report = Substitute.ForPartsOf<ReportBase>(string.Empty, TotalRows, new ShowingOption(), 0);
            report.Workbook.Returns(Substitute.For<IWorkbook>());

            // Action
            report.AssignDataMultiSheet();

            // Assert
            AssertNoInfoAssignSheet(report);
        }

        [Test]
        public void AssignDataMultiSheet_TotalRowsGreaterThanZero_CallReleventMethods()
        {
            // Arrange
            const int TotalRows = 3;
            var report = Substitute.ForPartsOf<ReportBase>(string.Empty, TotalRows, new ShowingOption(), 0);
            report.PageSize = 2;
            report.Workbook.Returns(Substitute.For<IWorkbook>());

            // Action
            report.AssignDataMultiSheet();

            // Assert
            Assert.Multiple(() =>
            {
                report.Received(2).AssignHeader(Arg.Any<ISheet>());
                report.Received(2).AssignTitle(Arg.Any<ISheet>());
                report.Received(2).ProcessUI(Arg.Any<ISheet>());
                report.Received(1).AssignSheetData(Arg.Any<ISheet>(), 0);
                report.Received(1).AssignSheetData(Arg.Any<ISheet>(), 1);
            });
        }

        private static void AssertNoInfoAssignSheet(ReportBase report)
        {
            Assert.Multiple(() =>
            {
                report.Received(1).AssignHeader(Arg.Any<ISheet>());
                report.Received(1).AssignTitle(Arg.Any<ISheet>());
                report.Received(1).AssignNoInfoRow(Arg.Any<ISheet>());
                report.Received(1).ProcessUI(Arg.Any<ISheet>());
                report.DidNotReceive().AssignSheetData(Arg.Any<ISheet>());
            });
        }
    }
}