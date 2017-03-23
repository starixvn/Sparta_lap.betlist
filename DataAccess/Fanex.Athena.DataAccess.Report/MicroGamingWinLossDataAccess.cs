// -----------------------------------------------------------------------
// <copyright file="MicroGamingWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class MicroGamingWinLossDataAccess : BaseDataAccess, IMicroGamingWinLossDataAccess
    {
        public MicroGamingWinLossDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        public SortableList<MicroGamingWinLoss> GetSummaryWinloss(MicroGamingWinLossMetaModel metaModel, out IDictionary<string, MicroGamingWinLoss> summaryTotal)
        {
            var uspName = "LRF_Rpt_MicroGamingWinloss";
            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);
                    db.AddInParameter("licenseeid", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("fromdate", DbType.Date, metaModel.FromDate.Date);
                    db.AddInParameter("todate", DbType.Date, metaModel.ToDate.Date);
                    db.AddInParameter("ReportType", DbType.Int32, metaModel.ReportBy);
                    using (var reader = db.ExecuteReader())
                    {
                        return GetData(reader, metaModel, out summaryTotal);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static SortableList<MicroGamingWinLoss> GetData(
            IDataReader reader,
            MicroGamingWinLossMetaModel metaModel,
            out IDictionary<string, MicroGamingWinLoss> summaryTotal)
        {
            var result = new SortableList<MicroGamingWinLoss>();

            summaryTotal = new Dictionary<string, MicroGamingWinLoss>();
            foreach (var currency in SystemConstants.BaseCurrencyList)
            {
                summaryTotal.Add(currency, new MicroGamingWinLoss());
            }

            var winlostColumn = "winlost" + metaModel.BaseCurrency;
            var licenseewlColumn = "licenseewinlost" + metaModel.BaseCurrency;
            var turnoverColumn = "turnover" + metaModel.BaseCurrency;
            var progressiveContributionColumn = "ProgressiveContribution" + metaModel.BaseCurrency;
            var progressivePayoutColumn = "ProgressivePayout" + metaModel.BaseCurrency;
            var amtDueToMGCColumn = "AmtDueToMGC" + metaModel.BaseCurrency;
            var licenseeNetWinColumn = "LicenseeNetWin" + metaModel.BaseCurrency;
            var progressiveFeeColumn = "ProgressiveFee" + metaModel.BaseCurrency;
            var index = 1;
            while (reader.Read())
            {
                var item = new MicroGamingWinLoss
                {
                    Index = index++,
                    TypeID = reader.ToInt("typeid"),
                    TypeName = reader.ToString("typename"),
                    Date = ConvertHelper.ToDateTime(reader.ToString("winlostdate"), metaModel.ReportBy == ReportByType.Monthly),
                    CurrencyID = reader.ToInt("currency"),
                    CurrencyName = reader.ToString("currencyname"),
                    TurnOver = reader.ToDecimal(turnoverColumn),
                    TotalTransaction = reader.ToInt("totaltrans"),
                    CustomerWinLoss = reader.ToDecimal(winlostColumn),
                    ProgressiveContribution = reader.ToDecimal(progressiveContributionColumn),
                    ProgressivePayout = reader.ToDecimal(progressivePayoutColumn),
                    LicenseeWL = reader.ToDecimal(licenseewlColumn),
                    LicenseeNetWin = reader.ToDecimal(licenseeNetWinColumn),
                    AmountDueToMGC = reader.ToDecimal(amtDueToMGCColumn),
                    ProgressiveFee = reader.ToDecimal(progressiveFeeColumn)
                };

                result.Add(item);

                foreach (var currency in SystemConstants.BaseCurrencyList)
                {
                    summaryTotal[currency].TurnOver += reader.ToDecimal("turnover" + currency);
                    summaryTotal[currency].TotalTransaction += reader.ToInt("totaltrans");
                    summaryTotal[currency].CustomerWinLoss += reader.ToDecimal("winlost" + currency);
                    summaryTotal[currency].ProgressiveContribution += reader.ToDecimal("ProgressiveContribution" + currency);
                    summaryTotal[currency].ProgressivePayout += reader.ToDecimal("ProgressivePayout" + currency);
                    summaryTotal[currency].LicenseeWL += reader.ToDecimal("licenseewinlost" + currency);
                    summaryTotal[currency].LicenseeNetWin += reader.ToDecimal("LicenseeNetWin" + currency);
                    summaryTotal[currency].AmountDueToMGC += reader.ToDecimal("AmtDueToMGC" + currency);
                    summaryTotal[currency].ProgressiveFee += reader.ToDecimal("ProgressiveFee" + currency);
                }
            }

            return result;
        }
    }
}