// -----------------------------------------------------------------------
// <copyright file="RacingCommissionDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public class RacingCommissionDataAccess : BaseDataAccess, IRacingCommissionDataAccess
    {
        public RacingCommissionDataAccess(BalanceUp balanceUp)
            : base(balanceUp)
        {
        }

        /// <summary>
        /// Get data list.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="total">Total value.</param>
        /// <returns>The result value.</returns>
        public SortableList<RacingCommission> GetDataList(RacingCommissionMetaModel metaModel, ref RacingCommission total)
        {
            const string UspName = "Lrf_Rpt_CommByBettype_HR";
            var uspParams = new
            {
                LicenseeID = metaModel.LicenseeId,
                metaModel.UserId,
                metaModel.LicenseeType,
                metaModel.CurrencyId,
                metaModel.FromDate,
                metaModel.ToDate,
                BalanceUp.MoveBetsDay,
                metaModel.PageSize,
                metaModel.PageIndex
            };

            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    using (var reader = db.ExecuteReader(uspParams))
                    {
                        var baseCurrency = metaModel.BaseCurrency;

                        return this.SetToList(reader, baseCurrency, ref total);
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName, uspParams);
            }
        }

        /// <summary>
        /// Set to list.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="baseCurrency">Base currency value.</param>
        /// <param name="total">Total value.</param>
        /// <returns>The result value.</returns>
        public SortableList<RacingCommission> SetToList(IDataReader reader, string baseCurrency, ref RacingCommission total)
        {
            var dataList = new SortableList<RacingCommission>();

            // Determine para names based on currency
            var custId = "custID";
            var externalColumn = "NickName";
            var systemId = "UserName";

            var toteWinColumn = "Tote_Win" + baseCurrency;
            var totePlaceColumn = "Tote_Place" + baseCurrency;
            var toteWinPlaceColumn = "Tote_WinPlace" + baseCurrency;
            var toteTotalColumn = "Tote_Total" + baseCurrency;
            var customerCommColumn = "Tote_Total_PlayerComm" + baseCurrency;
            var sumToteWinColumn = "SumTote_Win" + baseCurrency;
            var sumTotePlaceColumn = "SumTote_Place" + baseCurrency;
            var sumToteWinPlaceColumn = "SumTote_WinPlace" + baseCurrency;
            var sumToteTotalColumn = "SumTote_Total" + baseCurrency;
            var sumTcustomerCommColumn = "SumTote_Total_PlayerComm" + baseCurrency;

            while (reader.Read())
            {
                var racingCommission = new RacingCommission();

                racingCommission.CustID = reader.ToInt(custId);
                racingCommission.ExternalID = reader.ToString(externalColumn);
                racingCommission.SystemID = reader.ToString(systemId);

                racingCommission.ToteWin = reader.ToDecimal(toteWinColumn);
                racingCommission.TotePlace = reader.ToDecimal(totePlaceColumn);
                racingCommission.ToteWinPlace = reader.ToDecimal(toteWinPlaceColumn);
                racingCommission.ToteTotal = reader.ToDecimal(toteTotalColumn);
                racingCommission.CustomerCommission = reader.ToDecimal(customerCommColumn);
                dataList.Add(racingCommission);
            }

            if (dataList.Count > 0 && reader.NextResult())
            {
                while (reader.Read())
                {
                    total.TotalRows = reader.ToInt("TotalRows");
                    total.ToteWin = reader.ToDecimal(sumToteWinColumn);
                    total.TotePlace = reader.ToDecimal(sumTotePlaceColumn);
                    total.ToteWinPlace = reader.ToDecimal(sumToteWinPlaceColumn);
                    total.ToteTotal = reader.ToDecimal(sumToteTotalColumn);
                    total.CustomerCommission = reader.ToDecimal(sumTcustomerCommColumn);
                }
            }

            return dataList;
        }
    }
}