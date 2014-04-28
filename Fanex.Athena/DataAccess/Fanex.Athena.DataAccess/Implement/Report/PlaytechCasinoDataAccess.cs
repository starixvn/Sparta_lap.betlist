// -----------------------------------------------------------------------
// <copyright file="PlaytechCasinoDataAccess.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Report
{
    using System.Collections.Generic;
    using System.Data;
    using System.Data.SqlClient;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Interface.Report;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class PlaytechCasinoDataAccess : IPlaytechCasinoDataAccess
    {
        /// <summary>
        /// Get main win loss.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="totalInEur">Total in EUR.</param>
        /// <param name="totalInUsd">Total in USD.</param>
        /// <returns>List of PLAYTECH Casino.</returns>
        public IList<PlaytechCasino> GetMainWinLoss(PlaytechCasinoMetaModel metaModel, ref PlaytechCasino totalInEur, ref PlaytechCasino totalInUsd)
        {
            var uspName = "Lrf_Rpt_PlaytechWinloss";
            var result = new List<PlaytechCasino>();
            var currencyId = metaModel.CurrencyId < 0 ? 0 : metaModel.CurrencyId;
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserID", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("CurrencyID", DbType.Int32, currencyId);
                    db.AddInParameter("FromDate", DbType.DateTime, metaModel.FromDate);
                    db.AddInParameter("ToDate", DbType.DateTime, metaModel.ToDate);
                    db.AddInParameter("reporttype", DbType.Byte, metaModel.ReportBy);
                    db.AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);

                    using (var reader = db.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                var playTechCasinoItem = new PlaytechCasino
                                {
                                    WinlossDate = ConvertHelper.ToDateTime(reader.ToString("reporttype"), metaModel.ReportBy == ReportByType.Monthly),
                                    CurrencyId = reader.ToInt("Currency"),
                                    CurrencyName = reader.ToString("CurrencyName"),
                                };
                                GetWinLossData(reader, playTechCasinoItem, ref totalInEur, ref totalInUsd, metaModel.BaseCurrency);
                                result.Add(playTechCasinoItem);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }

            return result;
        }

        /// <summary>
        /// Get agent win loss.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="totalInEur">Total in EUR.</param>
        /// <param name="totalInUsd">Total in USD.</param>
        /// <returns>List of PLAYTECH Casino.</returns>
        public IList<PlaytechCasino> GetAgentWinLoss(PlaytechCasinoMetaModel metaModel, ref PlaytechCasino totalInEur, ref PlaytechCasino totalInUsd)
        {
            var uspName = "Lrf_Rpt_PlaytechWinlossDetail_Ag";
            var result = new List<PlaytechCasino>();
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserID", DbType.Int32, metaModel.UserId);
                    db.AddInParameter("CurrencyID", DbType.Int32, metaModel.DrillDownCurrency);
                    db.AddInParameter("FromDate", DbType.DateTime, metaModel.DrillDownFromDate);
                    db.AddInParameter("ToDate", DbType.DateTime, metaModel.DrillDownToDate);
                    db.AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);

                    using (var reader = db.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                var playTechCasinoItem = new PlaytechCasino
                                {
                                    CustId = reader.ToInt("CustId"),
                                    SystemId = reader.ToString("username"),
                                    CurrencyId = reader.ToInt("Currency"),
                                    CurrencyName = reader.ToString("CurrencyName"),
                                };
                                GetWinLossData(reader, playTechCasinoItem, ref totalInEur, ref totalInUsd, metaModel.BaseCurrency);
                                result.Add(playTechCasinoItem);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }

            return result;
        }

        /// <summary>
        /// Get member win loss.
        /// </summary>
        /// <param name="metaModel">Meta model value.</param>
        /// <param name="totalInEur">Total in EUR.</param>
        /// <param name="totalInUsd">Total in USD.</param>
        /// <returns>List of PLAYTECH Casino.</returns>
        public IList<PlaytechCasino> GetMemberWinLoss(PlaytechCasinoMetaModel metaModel, ref PlaytechCasino totalInEur, ref PlaytechCasino totalInUsd)
        {
            var uspName = "Lrf_Rpt_PlaytechWinlossDetail";
            var result = new List<PlaytechCasino>();
            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("LicenseeID", DbType.Int32, metaModel.LicenseeId);
                    db.AddInParameter("UserID", DbType.Int32, metaModel.UserId);                    
                    db.AddInParameter("CustID", DbType.Int32, metaModel.CustId);
                    db.AddInParameter("CurrencyID", DbType.Int32, metaModel.DrillDownCurrency);
                    db.AddInParameter("FromDate", DbType.DateTime, metaModel.DrillDownFromDate);
                    db.AddInParameter("ToDate", DbType.DateTime, metaModel.DrillDownToDate);
                    db.AddInParameter("excludetest", DbType.Int32, metaModel.IsExcludedTestData);

                    using (var reader = db.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                var playTechCasinoItem = new PlaytechCasino
                                {
                                    SystemId = reader.ToString("username"),
                                    ExternalId = reader.ToString("nickname"),
                                    CurrencyName = reader.ToString("CurrencyName"),
                                };
                                GetWinLossData(reader, playTechCasinoItem, ref totalInEur, ref totalInUsd, metaModel.BaseCurrency);
                                result.Add(playTechCasinoItem);
                            }
                        }
                    }
                }
            }
            catch (SqlException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, metaModel);
            }

            return result;
        }

        /// <summary>
        /// Get win loss data.
        /// </summary>
        /// <param name="reader">Reader value.</param>
        /// <param name="playTechCasinoItem">PLAYTECH Casino item.</param>
        /// <param name="totalInEur">Total in EUR.</param>
        /// <param name="totalInUsd">Total in USD.</param>
        /// <param name="baseCurrency">Base currency.</param>
        private void GetWinLossData(
            IDataReader reader, 
            PlaytechCasino playTechCasinoItem, 
            ref PlaytechCasino totalInEur, 
            ref PlaytechCasino totalInUsd, 
            string baseCurrency)
        {
            playTechCasinoItem.Turnover = reader.ToDecimal("Turnover" + baseCurrency);
            playTechCasinoItem.TotalTrans = reader.ToInt("BetCount");
            playTechCasinoItem.CustomerWinLoss = reader.ToDecimal("Winlost" + baseCurrency);
            playTechCasinoItem.LicenseeWinLoss = reader.ToDecimal("LicenseeWL" + baseCurrency);

            totalInEur.Turnover += reader.ToDecimal("TurnoverEUR");
            totalInEur.TotalTrans += reader.ToInt("BetCount");
            totalInEur.CustomerWinLoss += reader.ToDecimal("WinlostEUR");
            totalInEur.LicenseeWinLoss += reader.ToDecimal("LicenseeWLEUR");

            totalInUsd.Turnover += reader.ToDecimal("TurnoverUSD");
            totalInUsd.TotalTrans += reader.ToInt("BetCount");
            totalInUsd.CustomerWinLoss += reader.ToDecimal("WinlostUSD");
            totalInUsd.LicenseeWinLoss += reader.ToDecimal("LicenseeWLUSD");
        }
    }
}
