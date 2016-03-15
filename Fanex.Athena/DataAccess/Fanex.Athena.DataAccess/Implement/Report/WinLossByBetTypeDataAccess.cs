// -----------------------------------------------------------------------
// <copyright file="WinLossByBetTypeDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Data;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    ///     Win Loss By Bet Type Data Access.
    /// </summary>
    public class WinLossByBetTypeDataAccess : BaseDataAccess, IWinLossByBetTypeDataAccess
    {
        public WinLossByBetTypeDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }        

        #region Utility

        /// <summary>
        /// Set to list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <returns>The result value.</returns>
        public static SortableList<WinLossByBetType> SetToList(IDataReader reader, string baseCurrency)
        {
            var dataList = new SortableList<WinLossByBetType>();
            var turnoverColumn = "Turnover" + baseCurrency;
            var winlossColumn = "MemberWinLost" + baseCurrency;
            var custCommColumn = "PlayerComm" + baseCurrency;
            var licenseeWlColumn = "LicenseeWL" + baseCurrency;
            var commBaColumn = "CommBA" + baseCurrency;
            var commRbColumn = "CommRB" + baseCurrency;
            var racebookRevenue = "WinLossRB" + baseCurrency;
            var amtDueToBaComumn = "AmtDueToBA" + baseCurrency;
            var amtDueToRbColumn = "AmtDueToRB" + baseCurrency;

            while (reader.Read())
            {
                var winLossByBetType = new WinLossByBetType();

                winLossByBetType.LiveIndicator = reader.ToBool("liveindicator");
                winLossByBetType.BetType = reader.ToInt("bettype");
                winLossByBetType.Turnover = reader.ToDecimal(turnoverColumn);
                winLossByBetType.CustomerWinLoss = reader.ToDecimal(winlossColumn);
                winLossByBetType.CustomerComm = reader.ToDecimal(custCommColumn);
                winLossByBetType.LicenseeWL = reader.ToDecimal(licenseeWlColumn);
                winLossByBetType.BACommission = reader.ToDecimal(commBaColumn);
                winLossByBetType.RBComm = reader.ToDecimal(commRbColumn);
                winLossByBetType.RacingRevenue = reader.ToDecimal(racebookRevenue);
                winLossByBetType.AmtDueToBA = reader.ToDecimal(amtDueToBaComumn);
                winLossByBetType.AmtDueToRacing = reader.ToDecimal(amtDueToRbColumn);

                dataList.Add(winLossByBetType);
            }

            return dataList;
        }

        #endregion Utility

        #region Win Loss By Bet Type Data

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel">Meta value.</param>
        /// <returns>The result value.</returns>
        public SortableList<WinLossByBetType> GetDataList(WinLossByBetTypeMetaModel metaModel)
        {
            const string UspName = "Lrf_Rpt_WinLossByBettype";
            var uspParams = new
                {
                    metaModel.LicenseeId,
                    metaModel.UserId,
                    metaModel.LicenseeType,
                    metaModel.CurrencyId,
                    metaModel.FromDate,
                    metaModel.ToDate,
                    BalanceUp.MoveBetsDay,
                    BalanceUp.BalanceUpDay
                };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        return SetToList(reader, metaModel.BaseCurrency);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        #endregion Win Loss By Bet Type Data
    }
}