namespace Fanex.Athena.Excel.Report.UnitTests.TestableClasses
{
    using System.Collections.Generic;
    using Fanex.Athena.Excel.Base.Entities;
    using Fanex.Athena.Model.Report.DTO.LiveReports;
    using Fanex.Athena.Model.Report.ViewModel.LiveReports;
    using NPOI.SS.UserModel;

    public class LiveReportExcelTestable : LiveReportExcel
    {
        public LiveReportExcelTestable(
            string templatePath,
            ConvertingOption convertingOption,
            ShowingOption showingOption,
            IList<LiveReport> liveReportList,
            LiveReportDetailViewModel liveReportDetailList)
            : base(templatePath, convertingOption, showingOption, liveReportList, liveReportDetailList)
        {
        }

        public void Public_AssignHeader(ISheet sheet)
        {
            AssignHeader(sheet);
        }

        public void Public_AssignNoInfoRow(ISheet sheet)
        {
            AssignNoInfoRow(sheet);
        }

        public void Public_AssignSheetData(ISheet sheet)
        {
            AssignSheetData(sheet);
        }

        public void Public_AssignTitle(ISheet sheet)
        {
            AssignTitle(sheet);
        }

        public void Public_ProcessUI(ISheet sheet)
        {
            ProcessUI(sheet);
        }
    }
}