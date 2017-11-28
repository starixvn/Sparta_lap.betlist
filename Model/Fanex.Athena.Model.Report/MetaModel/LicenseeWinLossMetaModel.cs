// -----------------------------------------------------------------------
// <copyright file="LicenseeWinLossMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using Extension.Utility;

    public class LicenseeWinLossMetaModel : BaseDrilldownReportMetaModel
    {
        public LicenseeWinLossMetaModel()
            : this(new AppSettings())
        {
        }

        public LicenseeWinLossMetaModel(IAppSettings appSetting)
            : base(appSetting)
        {
        }

        public byte ReportBy { get; set; }

        public int DrawFilter { get; set; }

        public bool IsExcludedTestData { get; set; }

        public string DrillDownCurrencyName { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }
    }
}