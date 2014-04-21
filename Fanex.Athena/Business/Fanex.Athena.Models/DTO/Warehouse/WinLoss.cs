// -----------------------------------------------------------------------
// <copyright file="WinLoss.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Warehouse
{
    using System;
    using Fanex.Athena.Models.Cache;
    using Fanex.Athena.Models.ViewModel;

    /// <summary>
    /// WinLoss class.
    /// </summary>
    [Serializable]
    public class WinLoss
    {
        private int _bettypeId;
        private bool _liveindicator;
        private object _groupByvalue;
        private int _reportBy;

        public WinLoss()
        {
            MemberWinLoss = 0;
            MemberWinLossCom = 0;
            BetCount = 0;
            Turnover = 0;
            BaComm = 0;
        }

        public bool Liveindicator
        {
            get { return _liveindicator; }
            set { _liveindicator = value; }
        }

        public int BettypeId
        {
            get { return _bettypeId; }
            set { _bettypeId = value; }
        }

        public int BetCount { get; set; }

        public decimal Turnover { get; set; }

        public decimal MemberWinLoss { get; set; }

        public decimal MemberWinLossCom { get; set; }

        public decimal LicenseeWinloss { get; set; }

        public decimal BaComm { get; set; }

        public decimal TotalGrossProfit
        {
            get { return LicenseeWinloss + BaComm; }
        }

        public decimal Margin
        {
            get { return Turnover > 0 ? MemberWinLoss / Turnover * 100 : 0; }
        }

        public object GroupByValue
        {
            get { return _groupByvalue; }
            set { _groupByvalue = value; }
        }

        public int ReportBy
        {
            get { return _reportBy; }
            set { _reportBy = value; }
        }

        public object GroupByString
        {
            get
            {
                switch (_reportBy)
                {
                    case (int)WarehouseReportBy.Day:
                        return Convert.ToDateTime(_groupByvalue).ToString("MM/dd/yyyy");

                    case (int)WarehouseReportBy.Month:
                    case (int)WarehouseReportBy.Currency:
                        return _groupByvalue.ToString();

                    case (int)WarehouseReportBy.League:
                        switch (Convert.ToInt32(_groupByvalue))
                        {
                            case -9:
                                return ViewResources.SystemParlay;
                            case -1:
                                return ViewResources.Outright + " " + ViewResources.Horse + " " + ViewResources.Racing;
                            default:
                                return CacheRef.GetLeagueName(_groupByvalue);
                        }

                    case (int)WarehouseReportBy.Bettype:
                        return _bettypeId >= 81 && _bettypeId < 120
                            ? CacheRef.GetBettype(_bettypeId, _liveindicator) : CacheRef.GetBettype(_groupByvalue);

                    default:
                        return string.Empty;
                }
            }
        }
    }
}