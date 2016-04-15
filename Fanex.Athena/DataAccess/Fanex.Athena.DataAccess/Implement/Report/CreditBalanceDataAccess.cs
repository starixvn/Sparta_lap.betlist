// -----------------------------------------------------------------------
// <copyright file="CreditBalanceDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;

    /// <summary>
    ///     Credit Balance.
    /// </summary>
    public class CreditBalanceDataAccess : BaseDataAccess, IBalanceDataAccess
    {
        /// <summary>
        /// Get report data.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <returns>The result value.</returns>
        public Balance GetReportData(BalanceMetaModel metaModel)
        {
            var uspName = "Lrf_CustomerSelBalance";
            var uspParam = new
                {
                    metaModel.LicenseeId,
                    metaModel.UserId,
                    metaModel.CurrencyId,
                    metaModel.LicenseeType,
                    metaModel.MoveBetsDay,
                    metaModel.FromDate,
                    metaModel.ToDate,
                    metaModel.IsOld
                };

            using (var db = new ObjectDb(uspName))
            {
                using (var reader = db.ExecuteReader(uspParam))
                {
                    return BindData(reader, metaModel.BaseCurrency);
                }
            }
        }

        /// <summary>
        /// Bind data.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static Balance BindData(IDataReader reader, string baseCurrency)
        {
            // Selected Month Columns
            var selectedMonthLicColumn = "SelectedMonthLicenseeWL" + baseCurrency;
            var selectedMonthBetTradeCommColumn = "SelectedMonthBetTradeComm" + baseCurrency;
            var selectedMonthBaCommColumn = "SelectedMonthCommBA" + baseCurrency;
            var selectedMonthRbCommColumn = "SelectedMonthCommRB" + baseCurrency;
            var selectedMonthRacingRevenueColumn = "SelectedMonthWinLossRB" + baseCurrency;
            var selectedMonthAmtDueToBetTradeColumn = "SelectedMonthAmtDueToBT" + baseCurrency;
            var selectedMonthAmtDueToBaColumn = "SelectedMonthAmtDuetoBA" + baseCurrency;
            var selectedMonthAmtDueToRacingColumn = "SelectedMonthAmtDuetoRB" + baseCurrency;
            var selectedMonthCustomerCommColumn = "SelectedMonthPlayerComm" + baseCurrency;

            // Yesterday Columns
            var yesterdayLicColumn = "YesterdayLicenseeWL" + baseCurrency;
            var yesterdayBetTradeCommColumn = "YesterdayBetTradeComm" + baseCurrency;
            var yesterdayBaCommColumn = "YesterdayCommBA" + baseCurrency;
            var yesterdayRbCommColumn = "YesterdayCommRB" + baseCurrency;
            var yesterdayRacingRevenueColumn = "YesterdayWinLossRB" + baseCurrency;
            var yesterdayAmtDueToBetTradeColumn = "YesterdayAmtDueToBT" + baseCurrency;
            var yesterdayAmtDueToBaColumn = "YesterdayAmtDueToBa" + baseCurrency;
            var yesterdayAmtDueToRacingColumn = "YesterdayAmtDuetoRB" + baseCurrency;
            var yesterdayCustomerCommColumn = "YesterdayPlayerComm" + baseCurrency;

            // Today Columns
            var todayLicColumn = "TodayLicenseeWL" + baseCurrency;
            var todayBetTradeCommColumn = "TodayBetTradeComm" + baseCurrency;
            var todayBaCommColumn = "TodayCommBA" + baseCurrency;
            var todayRbCommColumn = "TodayCommRB" + baseCurrency;
            var todayRacingRevenueColumn = "TodayWinLossRB" + baseCurrency;
            var todayAmtDueToBetTradeColumn = "TodayAmtDuetoBT" + baseCurrency;
            var todayAmtDueToBaColumn = "TodayAmtDuetoBA" + baseCurrency;
            var todayAmtDueToRacingColumn = "TodayAmtDuetoRB" + baseCurrency;
            var todayCustomerCommColumn = "TodayPlayerComm" + baseCurrency;

            // Customer pending funds Columns
            var customerPedingFundsColumn = "MemberOutstanding" + baseCurrency;

            var reportData = new Balance();

            var reportBalanceList = new List<BalanceDetails>();

            if (reader.Read())
            {
                var selectedMonthBalance = new BalanceDetails
                    {
                        LicWinLoss = reader.ToDecimal(selectedMonthLicColumn),
                        BetTradeCommission = reader.ToDecimal(selectedMonthBetTradeCommColumn),
                        BACommission = reader.ToDecimal(selectedMonthBaCommColumn),
                        RBCommission = reader.ToDecimal(selectedMonthRbCommColumn),
                        RacingRevenue = reader.ToDecimal(selectedMonthRacingRevenueColumn),
                        AmtDueToBetTrade = reader.ToDecimal(selectedMonthAmtDueToBetTradeColumn),
                        AmtDueToBA = reader.ToDecimal(selectedMonthAmtDueToBaColumn),
                        AmtDueToRacing = reader.ToDecimal(selectedMonthAmtDueToRacingColumn),
                        CustomerCommission = reader.ToDecimal(selectedMonthCustomerCommColumn),
                        BalanceDateString = DateTime.Now.ToString("MMMM yyyy")
                    };

                reportBalanceList.Add(selectedMonthBalance);

                var yesterdayBalance = new BalanceDetails
                    {
                        LicWinLoss = reader.ToDecimal(yesterdayLicColumn),
                        BetTradeCommission = reader.ToDecimal(yesterdayBetTradeCommColumn),
                        BACommission = reader.ToDecimal(yesterdayBaCommColumn),
                        RBCommission = reader.ToDecimal(yesterdayRbCommColumn),
                        RacingRevenue = reader.ToDecimal(yesterdayRacingRevenueColumn),
                        AmtDueToBA = reader.ToDecimal(yesterdayAmtDueToBaColumn),
                        AmtDueToBetTrade = reader.ToDecimal(yesterdayAmtDueToBetTradeColumn),
                        AmtDueToRacing = reader.ToDecimal(yesterdayAmtDueToRacingColumn),
                        CustomerCommission = reader.ToDecimal(yesterdayCustomerCommColumn),
                        BalanceDateString =
                            string.Format("{0} ({1})", ViewResources.Yesterday, DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy"))
                    };

                reportBalanceList.Add(yesterdayBalance);

                var todayBalance = new BalanceDetails
                    {
                        LicWinLoss = reader.ToDecimal(todayLicColumn),
                        BetTradeCommission = reader.ToDecimal(todayBetTradeCommColumn),
                        BACommission = reader.ToDecimal(todayBaCommColumn),
                        RBCommission = reader.ToDecimal(todayRbCommColumn),
                        RacingRevenue = reader.ToDecimal(todayRacingRevenueColumn),
                        AmtDueToBetTrade = reader.ToDecimal(todayAmtDueToBetTradeColumn),
                        AmtDueToBA = reader.ToDecimal(todayAmtDueToBaColumn),
                        AmtDueToRacing = reader.ToDecimal(todayAmtDueToRacingColumn),
                        CustomerCommission = reader.ToDecimal(todayCustomerCommColumn),
                        BalanceDateString = string.Format("{0} ({1})", ViewResources.Today, DateTime.Now.ToString("MM/dd/yyyy"))
                    };

                reportBalanceList.Add(todayBalance);

                reportData.BalanceList = reportBalanceList;

                reportData.CustomerPendingFunds = reader.ToDecimal(customerPedingFundsColumn);
                reportData.TotalCustomerActive = reader.ToInt("TotalMemberActive");
                reportData.TotalCustomerClosed = reader.ToInt("TotalMemberClosed");
                reportData.TotalCustomerSuspended = reader.ToInt("TotalMemberSuspended");
                reportData.UserName = reader.To<string>("UserName");
            }

            return reportData;
        }
    }
}