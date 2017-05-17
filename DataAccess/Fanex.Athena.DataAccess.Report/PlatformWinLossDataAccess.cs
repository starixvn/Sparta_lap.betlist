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
    using Core.Exception;
    using Core.Extensions;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;
    using Model.Core.DTO.Configuration;

    public class PlatformWinLossDataAccess : BaseDataAccess, IPlatformWinLossDataAccess
    {
        public PlatformWinLossDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        public IList<PlatformWinLossSummary> GetSummaryData(PlatformWinLossMetaModel metaModel, out PlatformWinLossSummary totalData)
        {
            const string UspName = "Lrf_Rpt_PlatformWinloss";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel);
                    using (var reader = db.ExecuteReader())
                    {
                        return ReadSummaryData(reader, metaModel, out totalData);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        public IList<PlatformWinLossMember> GetMemberData(PlatformWinLossMetaModel metaModel, out PlatformWinLossMember totalData)
        {
            const string UspName = "Lrf_Rpt_PlatformWinlossDetail";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    UpdateUspParams(db, metaModel);
                    using (var reader = db.ExecuteReader())
                    {
                        return ReadMemberData(reader, metaModel, out totalData);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }

        #region Private

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static IList<PlatformWinLossSummary> ReadSummaryData(IDataReader reader, PlatformWinLossMetaModel metaModel, out PlatformWinLossSummary totalData)
        {
            var dataList = new List<PlatformWinLossSummary>();
            totalData = new PlatformWinLossSummary();

            var turnoverColumn = "Turnover" + metaModel.BaseCurrency;
            var winlostComumn = "Winlost" + metaModel.BaseCurrency;
            var playerCommColumn = "PlayerComm" + metaModel.BaseCurrency;
            var licenseeWlColumn = "LicenseeWL" + metaModel.BaseCurrency;
            var parentRows = new Dictionary<int, PlatformWinLossSummary>();
            var childId = -1;
            while (reader.Read())
            {
                var platformType = reader.ToInt("PlatformType");
                var productCode = reader.ToString("productprefix").ToUpper();
                PlatformWinLossSummary parent;
                if (!parentRows.ContainsKey(platformType))
                {
                    parent = new PlatformWinLossSummary()
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

                totalData.TotalTransactions += child.TotalTransactions;
                totalData.Turnover += child.Turnover;
                totalData.CustomerWinLoss += child.CustomerWinLoss;
                totalData.CustomerComm += child.CustomerComm;
                totalData.LicenseeWL += child.LicenseeWL;
            }

            totalData.TurnoverPercent = totalData.Turnover > 0 ? 100 : 0;
            if (totalData.Turnover > 0)
            {
                foreach (var item in dataList)
                {
                    item.TurnoverPercent = item.Turnover / totalData.Turnover * 100;
                }
            }

            return dataList;
        }

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        private static IList<PlatformWinLossMember> ReadMemberData(
            IDataReader reader,
            PlatformWinLossMetaModel metaModel,
            out PlatformWinLossMember totalData)
        {
            var dataList = new List<PlatformWinLossMember>();
            totalData = new PlatformWinLossMember();

            var turnoverColumn = "Turnover" + metaModel.BaseCurrency;
            var winlostComumn = "Winlost" + metaModel.BaseCurrency;
            var playerCommColumn = "PlayerComm" + metaModel.BaseCurrency;
            var licenseeWlColumn = "LicenseeWL" + metaModel.BaseCurrency;
            var commBaColumn = "CommBA" + metaModel.BaseCurrency;
            var commRbColumn = "CommRB" + metaModel.BaseCurrency;
            var amtDueToBaComumn = "AmtDueToBA" + metaModel.BaseCurrency;
            var amtDueToRbColumn = "AmtDueToRB" + metaModel.BaseCurrency;
            var winLossRbColumn = "WinlossRB" + metaModel.BaseCurrency;

            var index = 1;
            while (reader.Read())
            {
                var item = new PlatformWinLossMember
                {
                    Index = index,
                    CurrencyName = reader.ToString("currencyName"),
                    TotalTransactions = reader.ToInt("Betcount"),
                    Turnover = reader.ToDecimal(turnoverColumn),
                    CustomerWinLoss = reader.ToDecimal(winlostComumn),
                    CustomerComm = reader.ToDecimal(playerCommColumn),
                    LicenseeWL = reader.ToDecimal(licenseeWlColumn),
                    BACommission = reader.ToDecimal(commBaColumn),
                    RacingRevenue = reader.ToDecimal(winLossRbColumn) + reader.ToDecimal(commRbColumn),
                    AmtDueToBA = reader.ToDecimal(amtDueToBaComumn),
                    AmtDueToRacing = reader.ToDecimal(amtDueToRbColumn),
                    CurrencyId = reader.ToInt("Currency")
                };

                item.SystemId = reader.ToString("username");
                item.CustomerId = reader.ToInt("custid");
                item.ExternalId = reader.ToString("nickname");

                dataList.Add(item);
                index++;

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

            return dataList;
        }

        private void UpdateUspParams(IObjectDb db, PlatformWinLossMetaModel metaModel)
        {
            db.AddInParameter("PlatformType", DbType.Byte, metaModel.PlatformType);
            db.AddInParameter("movebetsday", DbType.DateTime, BalanceUp.MoveBetsDay);
            db.AddInParameter("DrawFilter", DbType.Int16, metaModel.DrawFilter);
            db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
            db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
            db.AddInParameter("AgentList", DbType.String, metaModel.AgentListXml);

            if (metaModel.ProductList != null)
            {
                foreach (var product in metaModel.ProductList)
                {
                    db.AddInParameter("show" + product, DbType.Boolean, true);
                }
            }
        }

        #endregion Private
    }
}