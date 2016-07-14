// -----------------------------------------------------------------------
// <copyright file="PlaytechCasinoDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Report
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.DataAccess.Report.Interfaces;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
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
        /// <param name="totalInGbp">Total in GBP.</param>
        /// <returns>List of PLAYTECH Casino.</returns>
        public IList<PlaytechCasino> GetMainWinLoss(
            PlaytechCasinoMetaModel metaModel, 
            ref PlaytechCasino totalInEur, 
            ref PlaytechCasino totalInUsd,
            ref PlaytechCasino totalInGbp)
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
                                GetWinLossData(reader, playTechCasinoItem, totalInEur, totalInUsd, totalInGbp, metaModel.BaseCurrency);
                                result.Add(playTechCasinoItem);
                            }
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
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
        /// <param name="totalInGbp">Total in GBP.</param>
        /// <returns>List of PLAYTECH Casino.</returns>
        public IList<PlaytechCasino> GetAgentWinLoss(
            PlaytechCasinoMetaModel metaModel, 
            ref PlaytechCasino totalInEur, 
            ref PlaytechCasino totalInUsd,
            ref PlaytechCasino totalInGbp)
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
                                GetWinLossData(reader, playTechCasinoItem, totalInEur, totalInUsd, totalInGbp, metaModel.BaseCurrency);
                                result.Add(playTechCasinoItem);
                            }
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
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
        /// <param name="totalInGbp">Total in GBP.</param>
        /// <returns>List of PLAYTECH Casino.</returns>
        public IList<PlaytechCasino> GetMemberWinLoss(
            PlaytechCasinoMetaModel metaModel, 
            ref PlaytechCasino totalInEur, 
            ref PlaytechCasino totalInUsd,
            ref PlaytechCasino totalInGbp)
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
                                GetWinLossData(reader, playTechCasinoItem, totalInEur, totalInUsd, totalInGbp, metaModel.BaseCurrency);
                                result.Add(playTechCasinoItem);
                            }
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
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
        /// <param name="totalInGbp">Total in GBP.</param>
        /// <param name="baseCurrency">Base currency.</param>
        private static void GetWinLossData(
            IDataReader reader, 
            PlaytechCasino playTechCasinoItem, 
            PlaytechCasino totalInEur, 
            PlaytechCasino totalInUsd,
            PlaytechCasino totalInGbp, 
            string baseCurrency)
        {
            playTechCasinoItem.Turnover = reader.ToDecimal("Turnover" + baseCurrency);
            playTechCasinoItem.TotalTrans = reader.ToInt("BetCount");
            playTechCasinoItem.CustomerWinLoss = reader.ToDecimal("Winlost" + baseCurrency);
            playTechCasinoItem.LicenseeWinLoss = reader.ToDecimal("LicenseeWL" + baseCurrency);
            playTechCasinoItem.ProgressiveShare = reader.ToDecimal("PShare" + baseCurrency);
            playTechCasinoItem.ProgressiveWin = reader.ToDecimal("PWin" + baseCurrency);
            playTechCasinoItem.NetGaming = reader.ToDecimal("NetGaming" + baseCurrency);

            totalInEur.Turnover += reader.ToDecimal("TurnoverEUR");
            totalInEur.TotalTrans += reader.ToInt("BetCount");
            totalInEur.CustomerWinLoss += reader.ToDecimal("WinlostEUR");
            totalInEur.LicenseeWinLoss += reader.ToDecimal("LicenseeWLEUR");
            totalInEur.ProgressiveShare += reader.ToDecimal("PShareEUR");
            totalInEur.ProgressiveWin += reader.ToDecimal("PWinEUR");
            totalInEur.NetGaming += reader.ToDecimal("NetGamingEUR");

            totalInUsd.Turnover += reader.ToDecimal("TurnoverUSD");
            totalInUsd.TotalTrans += reader.ToInt("BetCount");
            totalInUsd.CustomerWinLoss += reader.ToDecimal("WinlostUSD");
            totalInUsd.LicenseeWinLoss += reader.ToDecimal("LicenseeWLUSD");
            totalInUsd.ProgressiveShare += reader.ToDecimal("PShareUSD");
            totalInUsd.ProgressiveWin += reader.ToDecimal("PWinUSD");
            totalInUsd.NetGaming += reader.ToDecimal("NetGamingUSD");

            totalInGbp.Turnover += reader.ToDecimal("TurnoverGBP");
            totalInGbp.TotalTrans += reader.ToInt("BetCount");
            totalInGbp.CustomerWinLoss += reader.ToDecimal("WinlostGBP");
            totalInGbp.LicenseeWinLoss += reader.ToDecimal("LicenseeWLGBP");
            totalInGbp.ProgressiveShare += reader.ToDecimal("PShareGBP");
            totalInGbp.ProgressiveWin += reader.ToDecimal("PWinGBP");
            totalInGbp.NetGaming += reader.ToDecimal("NetGamingGBP");
        }
    }
}
