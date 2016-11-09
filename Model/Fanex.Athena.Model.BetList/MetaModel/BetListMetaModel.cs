// -----------------------------------------------------------------------
// <copyright file="BetListMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.MetaModel
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using Fanex.Athena.Model.Core.MetaModel.Product;
    using Model.Core.MetaModel;

    /// <summary>
    /// BetListMetaModel class.
    /// </summary>
    public class BetListMetaModel : BaseMetaModel
    {
        public BetListMetaModel()
        {
            ProductOptions = new HashSet<ProductOption>();
            FromDate = DateTime.Today;
            Date = DateTime.Today;
            ToDate = DateTime.Today;
            PageIndex = 1;
            PageSize = int.MaxValue - 1;
            IsLastBetMonitoring = false;
        }

        public string ExcelFileName { get; set; }

        public bool IsShowSumStake { get; set; }

        public IEnumerable<ProductOption> ProductOptions { get; set; }

        public bool IsServerPaging { get; set; }

        public string SortBy { get; set; }

        public string SortOrder { get; set; }

        #region Currency chosen on filter

        public int OriginalCurrenceId { get; set; }

        public string OriginalCurrencyName { get; set; }

        #endregion Currency chosen on filter

        public bool ShowWin { get; set; }

        public bool ShowLoss { get; set; }

        public bool ShowDraw { get; set; }

        public decimal Odds { get; set; }

        public bool IsExportExcel { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public DateTime FromDate { get; set; }

        public string FromDateString
        {
            get
            {
                return FromDate.ToString(CultureInfo.InvariantCulture);
            }
        }

        public DateTime ToDate { get; set; }

        public string ToDateString
        {
            get
            {
                return ToDate.ToString(CultureInfo.InvariantCulture);
            }
        }

        public DateTime Date { get; set; }

        public string DateString
        {
            get
            {
                return Date.ToString(CultureInfo.InvariantCulture);
            }
        }

        public int CustId { get; set; }

        public string SystemId { get; set; }

        public string ExternalId { get; set; }

        public string BetListName { get; set; }

        public DateTime MoveBetsDay { get; set; }

        public DateTime BalanceUpDay { get; set; }

        public int MatchId { get; set; }

        public int HomeId { get; set; }

        public int AwayId { get; set; }

        public int LeagueId { get; set; }

        public int BetType { get; set; }

        public string MultipleBettype { get; set; }

        public string BetTeam { get; set; }

        public int? LiveIndicator { get; set; }

        public string RaceNo { get; set; }

        public int SportType { get; set; }

        public int DrawFilter { get; set; }

        public string CurrencyIdList { get; set; }

        public string BetTypeLastBets { get; set; }

        public int LimitRowCount { get; set; }

        public string SpecialLevel { get; set; }

        public long TransId { get; set; }

        public long TransType { get; set; }

        public bool IsLastBetMonitoring { get; set; }

        public int PlatformType { get; set; }

        public string BetChoice { get; set; }

        public string Handicap { get; set; }

        public string ProductCode { get; set; }

        public string DailyBetListStatus { get; set; }

        public virtual bool IsValid
        {
            get
            {
                return FromDate <= ToDate
                    && ProductOptions.Any(p => p.Status);
            }
        }
    }
}