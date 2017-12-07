namespace Fanex.Athena.Model.Report.MetaModel
{
    public class ExchangeWinLossMetaModel : BaseDrilldownReportMetaModel
    {
        public byte ReportBy { get; set; }

        public bool ExcludeTestData { get; set; }
    }
}
