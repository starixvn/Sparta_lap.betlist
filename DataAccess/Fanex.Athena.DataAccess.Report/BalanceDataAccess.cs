// -----------------------------------------------------------------------
// <copyright file="CreditBalanceDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Resources;
    using Session.Interfaces;

    public class BalanceDataAccess : BaseDataAccess, IBalanceDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public BalanceDataAccess(
            IAppSettings appSetting,
            ISessionStorage sessionStorage,
            IBalanceUpDataAccess balanceUpDataAccess)
            : base(appSetting, sessionStorage)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public Balance GetReportData(BalanceMetaModel metaModel)
        {
            var uspParam = new
            {
                metaModel.LicenseeId,
                MoveBetsDay = _balanceUpDataAccess.GetBalanceUp().MoveBetsDay,
                metaModel.FromDate,
                metaModel.ToDate,
                metaModel.IsOld,
                AgentList = metaModel.AgentListXml
            };

            using (var db = GetObjectDb("Lrf_CustomerSelBalance"))
            {
                using (var reader = db.ExecuteReader(uspParam))
                {
                    return BindData(reader, metaModel.BaseCurrency);
                }
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static Balance BindData(IDataReader reader, string baseCurrency)
        {
            // Selected Month Columns
            var selectedMonthLicColumn = "SelectedMonthLicenseeWL" + baseCurrency;
            var selectedMonthBetTradeCommColumn = "SelectedMonthBetTradeComm" + baseCurrency;
            var selectedMonthBaCommColumn = "SelectedMonthCommBA" + baseCurrency;
            var selectedMonthAmtDueToBetTradeColumn = "SelectedMonthAmtDueToBT" + baseCurrency;
            var selectedMonthAmtDueToBaColumn = "SelectedMonthAmtDuetoBA" + baseCurrency;
            var selectedMonthCustomerCommColumn = "SelectedMonthPlayerComm" + baseCurrency;

            // Yesterday Columns
            var yesterdayLicColumn = "YesterdayLicenseeWL" + baseCurrency;
            var yesterdayBetTradeCommColumn = "YesterdayBetTradeComm" + baseCurrency;
            var yesterdayBaCommColumn = "YesterdayCommBA" + baseCurrency;
            var yesterdayAmtDueToBetTradeColumn = "YesterdayAmtDueToBT" + baseCurrency;
            var yesterdayAmtDueToBaColumn = "YesterdayAmtDuetoBA" + baseCurrency;
            var yesterdayCustomerCommColumn = "YesterdayPlayerComm" + baseCurrency;

            // Today Columns
            var todayLicColumn = "TodayLicenseeWL" + baseCurrency;
            var todayBetTradeCommColumn = "TodayBetTradeComm" + baseCurrency;
            var todayBaCommColumn = "TodayCommBA" + baseCurrency;
            var todayAmtDueToBetTradeColumn = "TodayAmtDueToBT" + baseCurrency;
            var todayAmtDueToBaColumn = "TodayAmtDuetoBA" + baseCurrency;
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
                    AmtDueToBetTrade = reader.ToDecimal(selectedMonthAmtDueToBetTradeColumn),
                    AmtDueToBA = reader.ToDecimal(selectedMonthAmtDueToBaColumn),
                    CustomerCommission = reader.ToDecimal(selectedMonthCustomerCommColumn),
                    BalanceDateString = DateTime.Now.ToString("MMMM yyyy")
                };

                reportBalanceList.Add(selectedMonthBalance);

                var yesterdayDateString = DateTime.Now.AddDays(-1).ToString("MM/dd/yyyy");
                var yesterdayBalance = new BalanceDetails
                {
                    LicWinLoss = reader.ToDecimal(yesterdayLicColumn),
                    BetTradeCommission = reader.ToDecimal(yesterdayBetTradeCommColumn),
                    BACommission = reader.ToDecimal(yesterdayBaCommColumn),
                    AmtDueToBA = reader.ToDecimal(yesterdayAmtDueToBaColumn),
                    AmtDueToBetTrade = reader.ToDecimal(yesterdayAmtDueToBetTradeColumn),
                    CustomerCommission = reader.ToDecimal(yesterdayCustomerCommColumn),
                    BalanceDateString = $"{ViewResources.Yesterday} ({yesterdayDateString})"
                };

                reportBalanceList.Add(yesterdayBalance);
                var todayDateString = DateTime.Now.ToString("MM/dd/yyyy");
                var todayBalance = new BalanceDetails
                {
                    LicWinLoss = reader.ToDecimal(todayLicColumn),
                    BetTradeCommission = reader.ToDecimal(todayBetTradeCommColumn),
                    BACommission = reader.ToDecimal(todayBaCommColumn),
                    AmtDueToBetTrade = reader.ToDecimal(todayAmtDueToBetTradeColumn),
                    AmtDueToBA = reader.ToDecimal(todayAmtDueToBaColumn),
                    CustomerCommission = reader.ToDecimal(todayCustomerCommColumn),
                    BalanceDateString = $"{ViewResources.Today} ({todayDateString})"
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