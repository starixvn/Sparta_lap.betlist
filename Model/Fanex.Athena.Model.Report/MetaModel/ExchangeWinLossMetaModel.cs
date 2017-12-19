namespace Fanex.Athena.Model.Report.MetaModel
{
    public class ExchangeWinLossMetaModel : BaseReportMetaModel
    {
        public byte ReportBy { get; set; }

        public bool ExcludeTestData { get; set; }

        public int CustId { get; set; }

        public int Level { get; set; }

        public override bool IsValid
        {
            get
            {
                return FromDate <= ToDate;
            }
        }
    }
}