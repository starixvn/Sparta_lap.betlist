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
    using System.Diagnostics.CodeAnalysis;
    using System.Globalization;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Core.Extensions;
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

        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel"> The model. </param>
        /// <returns> List win loss.</returns>
        public IList<WinLoss> GetDataList(WinLossMetaModel metaModel)
        {
            const string UspName = "Lrf_rpt_DWWinloss";
            var dataList = new List<WinLoss>();
            var uspParams = new
            {
                FromDate = metaModel.FromDate,
                ToDate = metaModel.ToDate,
                CurrencyId = metaModel.CurrencyId,
                groupingid = metaModel.ReportBy,
                drawfilter = metaModel.DrawFilter,
                UserId = metaModel.UserId,
                basedcurrency = metaModel.BaseCurrencyId
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

        #endregion IWinLossDataAccess Members
    }
}