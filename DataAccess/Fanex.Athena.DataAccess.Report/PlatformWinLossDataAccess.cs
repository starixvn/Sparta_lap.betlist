// -----------------------------------------------------------------------
// <copyright file="PlatformWinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Core;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Core.Exception;
    using Extension.Helper;
    using Extension.Utility;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Model.Core.ResultModel;
    using Session.Interfaces;

    public class PlatformWinLossDataAccess : BaseDataAccess, IPlatformWinLossDataAccess
    {
        private readonly IBalanceUpDataAccess _balanceUpDataAccess;

        public PlatformWinLossDataAccess()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
        }

        public PlatformWinLossDataAccess(
            ISessionStorage sessionStorage,
            IBalanceUpDataAccess balanceUpDataAccess)
            : base(null, sessionStorage)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
        }

        public ReportResultModel<PlatformWinLossSummary> GetSummaryData(PlatformWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_PlatformWinloss";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel);
                    using (var reader = db.ExecuteReader())
                    {
                        return ReadSummaryData(reader, metaModel);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName);
            }
        }

        public SortableReportResultModel<PlatformWinLossMember> GetMemberData(PlatformWinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_PlatformWinlossDetail";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel);
                    using (var reader = db.ExecuteReader())
                    {
                        return ReadMemberData(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                throw new DataAccessException(sqlException, UspName);
            }
        }

        #region Private

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static ReportResultModel<PlatformWinLossSummary> ReadSummaryData(IDataReader reader, PlatformWinLossMetaModel metaModel)
        {
            var dataList = new List<PlatformWinLossSummary>();
            var totalData = new PlatformWinLossSummary();

            var turnoverColumn = "Turnover" + metaModel.BaseCurrency;
            var winlostComumn = "Winlost" + metaModel.BaseCurrency;
            var playerCommColumn = "PlayerComm" + metaModel.BaseCurrency;
            var licenseeWlColumn = "LicenseeWL" + metaModel.BaseCurrency;
            var parentRows = new Dictionary<int, PlatformWinLossSummary>();
            var childId = -1;
            while (reader.Read())
            {
                var platformType = reader.ToInt("PlatformType");
                var productCode = reader.ToString("productprefix").ToUpperInvariant();
                PlatformWinLossSummary parent;
                if (!parentRows.ContainsKey(platformType))
                {
                    parent = new PlatformWinLossSummary
                    {
                        PlatformType = platformType,
                        Name = Model.Core.DataDomain.PlatformType.GetPlatformName(platformType),
                        ProductCode = string.Empty
                    };
                    parentRows.Add(platformType, parent);
                    dataList.Add(parent);
                }
                else
                {
                    parent = parentRows[platformType];
                }

                var child = new PlatformWinLossSummary
                {
                    PlatformType = childId--,
                    Parent = platformType.ToString(),
                    IsLeaf = true,
                    Level = 1,
                    Name = DataHelper.GetProductName(productCode),
                    TotalTransactions = reader.ToInt("Betcount"),
                    Turnover = reader.ToDecimal(turnoverColumn),
                    CustomerWinLoss = reader.ToDecimal(winlostComumn),
                    CustomerComm = reader.ToDecimal(playerCommColumn),
                    LicenseeWL = reader.ToDecimal(licenseeWlColumn),
                    ProductCode = productCode
                };

                parent.TotalTransactions += child.TotalTransactions;
                parent.Turnover += child.Turnover;
                parent.CustomerWinLoss += child.CustomerWinLoss;
                parent.CustomerComm += child.CustomerComm;
                parent.LicenseeWL += child.LicenseeWL;

                dataList.Add(child);
                AddTotalData(totalData, child);
            }

            totalData.TurnoverPercent = totalData.Turnover > 0 ? 100 : 0;
            if (totalData.Turnover > 0)
            {
                foreach (var item in dataList)
                {
                    item.TurnoverPercent = item.Turnover / totalData.Turnover * 100;
                }
            }

            return new ReportResultModel<PlatformWinLossSummary>
            {
                DataList = dataList,
                TotalData = totalData
            };
        }

        private static void AddTotalData(PlatformWinLossSummary totalData, PlatformWinLossSummary child)
        {
            totalData.TotalTransactions += child.TotalTransactions;
            totalData.Turnover += child.Turnover;
            totalData.CustomerWinLoss += child.CustomerWinLoss;
            totalData.CustomerComm += child.CustomerComm;
            totalData.LicenseeWL += child.LicenseeWL;
        }

        private static void AddTotalData(PlatformWinLossMember totalData, PlatformWinLossMember item)
        {
            totalData.TotalTransactions += item.TotalTransactions;
            totalData.Turnover += item.Turnover;
            totalData.CustomerWinLoss += item.CustomerWinLoss;
            totalData.CustomerComm += item.CustomerComm;
            totalData.LicenseeWL += item.LicenseeWL;
            totalData.BACommission += item.BACommission;
            totalData.RacingRevenue += item.RacingRevenue;
            totalData.AmtDueToBA += item.AmtDueToBA;
            totalData.AmtDueToRacing += item.AmtDueToRacing;
        }

        private SortableReportResultModel<PlatformWinLossMember> ReadMemberData(IDataReader reader, string baseCurrency)
        {
            var dataList = new SortableList<PlatformWinLossMember>();
            var totalData = new PlatformWinLossMember();

            var turnoverColumn = "Turnover" + baseCurrency;
            var winlostComumn = "Winlost" + baseCurrency;
            var playerCommColumn = "PlayerComm" + baseCurrency;
            var licenseeWlColumn = "LicenseeWL" + baseCurrency;
            var commBaColumn = "CommBA" + baseCurrency;
            var commRbColumn = "CommRB" + baseCurrency;
            var amtDueToBaComumn = "AmtDueToBA" + baseCurrency;
            var amtDueToRbColumn = "AmtDueToRB" + baseCurrency;
            var winLossRbColumn = "WinlossRB" + baseCurrency;

            var index = 1;
            while (reader.Read())
            {
                var currencyId = reader.ToInt("Currency");
                var item = new PlatformWinLossMember
                {
                    Index = index,
                    CurrencyName = GetCurrencyName(currencyId),
                    TotalTransactions = reader.ToInt("Betcount"),
                    Turnover = reader.ToDecimal(turnoverColumn),
                    CustomerWinLoss = reader.ToDecimal(winlostComumn),
                    CustomerComm = reader.ToDecimal(playerCommColumn),
                    LicenseeWL = reader.ToDecimal(licenseeWlColumn),
                    BACommission = reader.ToDecimal(commBaColumn),
                    RacingRevenue = reader.ToDecimal(winLossRbColumn) + reader.ToDecimal(commRbColumn),
                    AmtDueToBA = reader.ToDecimal(amtDueToBaComumn),
                    AmtDueToRacing = reader.ToDecimal(amtDueToRbColumn),
                    CurrencyId = reader.ToInt("Currency"),
                    SystemId = reader.ToString("username"),
                    CustomerId = currencyId,
                    ExternalId = reader.ToString("nickname")
                };

                dataList.Add(item);
                index++;
                AddTotalData(totalData, item);
            }

            return new SortableReportResultModel<PlatformWinLossMember>
            {
                DataList = dataList,
                TotalData = totalData
            };
        }

        private void UpdateUspParams(IObjectDb db, PlatformWinLossMetaModel metaModel)
        {
            db.AddInParameter("PlatformType", DbType.Byte, metaModel.PlatformType);
            db.AddInParameter("movebetsday", DbType.DateTime, _balanceUpDataAccess.GetBalanceUp().MoveBetsDay);
            db.AddInParameter("DrawFilter", DbType.Int16, metaModel.DrawFilter);
            db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);

            foreach (var product in metaModel.ProductOptions)
            {
                db.AddInParameter("show" + product.Code, DbType.Boolean, product.Status);
            }
        }

        #endregion Private
    }
}