// -----------------------------------------------------------------------
// <copyright file="WinLossDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Warehouse
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
    using Fanex.Athena.DataAccess.Warehouse.Interfaces;
    using Fanex.Athena.Model.Warehouse.DTO;
    using Fanex.Athena.Model.Warehouse.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class WinLossDataAccess : BaseDataAccess, IWinLossDataAccess
    {
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<WinLoss> GetDataList(WinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_rpt_DWWinloss";
            var dataList = new List<WinLoss>();
            var uspParams = new
            {
                LicenseeId = metaModel.LicenseeId,
                AgentList = metaModel.AgentListXml,
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                groupingid = metaModel.ReportBy,
                drawfilter = metaModel.DrawFilter,
                basedcurrency = metaModel.BaseCurrencyId
            };
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var groupingList = GroupingMetaModel.GetCollection();
                        while (reader.Read())
                        {
                            var groupByName = (metaModel.ReportBy == GroupingMetaModel.DefaultValue) ? "date"
                                                  : groupingList[metaModel.ReportBy.ToString(CultureInfo.InvariantCulture)].Replace(" ", string.Empty);
                            dataList.Add(new WinLoss
                            {
                                ReportBy = metaModel.ReportBy,
                                BetCount = reader.ToInt("betcount"),
                                GroupByValue = reader[groupByName],
                                MemberWinLoss = reader.ToDecimal("MemberWinLost"),
                                LicenseeWinloss = reader.ToDecimal("LicenseeWL"),
                                Turnover = reader.ToDecimal("Turnover"),
                                Liveindicator = reader.HasColumn("liveindicator") && (reader.ToInt("liveindicator") == 1),
                                BettypeId = reader.HasColumn("bettype") ? reader.ToInt("bettype") : 0,
                            });
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }

            return dataList;
        }
    }
}