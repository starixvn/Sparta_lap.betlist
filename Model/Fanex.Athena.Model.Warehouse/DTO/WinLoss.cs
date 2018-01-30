// -----------------------------------------------------------------------
// <copyright file="WinLoss.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Warehouse.DTO
{
    using System;
    using Resources;
    using VendorAdapter;

    /// <summary>
    /// WinLoss class.
    /// </summary>
    [Serializable]
    public class WinLoss
    {
        public WinLoss()
        {
            MemberWinLoss = 0;
            MemberWinLossCom = 0;
            BetCount = 0;
            Turnover = 0;
            BAComm = 0;
        }

        public bool Liveindicator { get; set; }

        public int BettypeId { get; set; }

        public int BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberWinLossCom { get; set; }

        public decimal LicenseeWinloss { get; set; }

        public decimal BAComm { get; set; }

        public decimal TotalGrossProfit
        {
            get { return LicenseeWinloss + BAComm; }
        }

        public decimal Margin
        {
            get { return Turnover > 0 ? MemberWinLoss / Turnover * 100 : 0; }
        }

        public object GroupByValue { get; set; }

        public int ReportBy { get; set; }

        public object GroupByString
        {
            get
            {
                switch (ReportBy)
                {
                    case (int)WarehouseReportBy.Day:
                        return Convert.ToDateTime(GroupByValue).ToString("MM/dd/yyyy");

                    case (int)WarehouseReportBy.Month:
                    case (int)WarehouseReportBy.Currency:
                        return GroupByValue.ToString();

                    case (int)WarehouseReportBy.League:
                        switch (Convert.ToInt32(GroupByValue))
                        {
                            case -9:
                                return ViewResources.SystemParlay;

                            case -1:
                                return ViewResources.Outright + " " + ViewResources.Horse + " " + ViewResources.ProductRB;

                            default:
                                return CacheRefAdapter.Instance.GetLeagueName(GroupByValue);
                        }

                    case (int)WarehouseReportBy.Bettype:
                        return BettypeId >= 81 && BettypeId < 120 ?
                            CacheRefAdapter.Instance.GetBettype(BettypeId, Liveindicator) :
                            CacheRefAdapter.Instance.GetBettype(GroupByValue);

                    default:
                        return string.Empty;
                }
            }
        }
    }
}