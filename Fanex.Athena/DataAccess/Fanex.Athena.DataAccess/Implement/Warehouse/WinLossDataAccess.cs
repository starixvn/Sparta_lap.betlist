// -----------------------------------------------------------------------
// <copyright file="WinLossDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using System.Globalization;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Extension;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Warehouse;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Data warehouse win loss data access.
    /// </summary>
    public class WinLossDataAccess : BaseDataAccess, IWinLossDataAccess
    {
        #region IWinLossDataAccess Members

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel"> The model. </param>
        /// <param name="licenseeList"> The licensee list. </param>
        /// <returns> List win loss.</returns>
        public IList<WinLoss> GetDataList(WinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_rpt_DWWinloss";
            var dataList = new List<WinLoss>();
            var baseCurrency = metaModel.BaseCurrency == "eur" ? 2 : (metaModel.BaseCurrency == "usd") ? 1 : 0;
            var uspParams = new
                                {
                                    FromDate = metaModel.FromDate,
                                    ToDate = metaModel.ToDate,
                                    CurrencyId = metaModel.CurrencyId,
                                    groupingid = metaModel.ReportBy,
                                    drawfilter = metaModel.DrawFilter,
                                    UserId = metaModel.UserId,
                                };

            try
            {
                using (var db = new ObjectDb(UspName))
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
                                MemberWinLoss = reader.ToDecimal("winlossmember_total"),
                                Turnover = reader.ToDecimal("turnover100_total"),
                                BaComm = reader.ToDecimal("commission"),
                                Liveindicator = reader.FieldExists("liveindicator") && (reader.ToInt("liveindicator") == 1),
                                BettypeId = reader.FieldExists("bettypeid") ? reader.ToInt("bettypeid") : 0,
                                MemberWinLossCom = reader.ToDecimal("playercomm"),
                            });
                        }
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }

            return dataList;
        }

        /// <summary>
        /// Get data for excel.
        /// </summary>
        /// <param name="metaModel">Meta model.</param>
        /// <param name="licenseeList">Licensee list.</param>
        /// <returns>Data table.</returns>
        public DataTable GetDataForExcel(WinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_rpt_DWWinloss";
            var baseCurrency = metaModel.BaseCurrency == "eur" ? 2 : (metaModel.BaseCurrency == "usd") ? 1 : 0;
            var uspParams = new
                            {
                                datestart = metaModel.FromDate,
                                dateend = metaModel.ToDate,
                                currencyid = metaModel.CurrencyId == 0 ? -1 : metaModel.CurrencyId,
                                groupingid = metaModel.ReportBy,
                                basedcurrency = baseCurrency,
                                drawfilter = metaModel.DrawFilter
                            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    return db.ExecuteDataSet(uspParams).Tables[0];
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        #endregion IWinLossDataAccess Members
    }
}