// -----------------------------------------------------------------------
// <copyright file="RNGCasinoService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Casino
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Interfaces;
    using Extension.Helper;
    using Fanex.Athena.Model.Casino.DTO;
    using Fanex.Athena.Model.Casino.DTO.RNGResultDetail;
    using Fanex.Athena.Model.Casino.MetaModel;
    using Fanex.Athena.Model.Casino.ViewModel;
    using Fanex.Athena.Model.Core.DataDomain;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Casino.Interfaces;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Helpers;
    using Fanex.Common;
    using Wrappers;

    public class RNGCasinoService : IRNGCasinoService
    {
        private readonly IReportApiWrapper _reportApiWrapper;

        private readonly ILanguageService _langService;

        private readonly IExChangeRateDataAccess _exchangeRateDataAccess;

        public RNGCasinoService() : this(null, null, null)
        {
        }

        public RNGCasinoService(
            IReportApiWrapper reportApiWrapper,
            ILanguageService langService,
            IExChangeRateDataAccess exchangeRateDataAccess)
        {
            _reportApiWrapper = reportApiWrapper ?? new ReportApiWrapper();
            _langService = langService ?? new LanguageService();
            _exchangeRateDataAccess = exchangeRateDataAccess ?? new ExChangeRateDataAccess();
        }

        public List<RNGCasinoResult> GetRNGCasinoResult(RNGCasinoResultMetaModel metaModel)
        {
            var casinos = new List<RNGCasinoResult>();
            var dataTable = GetResultFromAPI(metaModel);
            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                var currency = GetDataRowExtension(dataTable.Rows[0], "Currency");

                var date = Converter.ToDateTime(GetDataRowExtension(dataTable.Rows[0], "StartTime"));
                var exchangeRate = this._exchangeRateDataAccess.GetExChangeRate(currency, date);
                var language = _langService.GetCurrentLanguage();
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var dr = dataTable.Rows[i];
                    var casino = MapDataToCasinoItem(dr, language);
                    casino.Index = i + 1;
                    if (metaModel.IsShowPic)
                    {
                        casino.Result = casino.Result.Replace("<img", "<br /><img");
                    }

                    var rate = GetExchangeRate(exchangeRate, metaModel.BaseCurrency);

                    if (rate != 1)
                    {
                        casino.BeginBalance *= rate;
                        casino.EndBlance *= rate;
                        casino.Bet *= rate;
                        casino.Stake *= rate;
                        casino.Win *= rate;
                        casino.WinLoss *= rate;
                        casino.RollingComm *= rate;
                    }

                    casinos.Add(casino);
                }
            }

            return casinos;
        }

        /// <summary>
        /// Get RNG casino detail result.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The detail result.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public CasinoHandHistoryDetailViewModel GetRNGCasinoResultDetail(RNGCasinoResultDetailMetaModel metaModel)
        {
            var resultModel = new CasinoHandHistoryDetailViewModel();
            try
            {
                var tables = _reportApiWrapper.WinLossDetail(metaModel.UserBetId);

                if (tables != null && tables.Length > 1)
                {
                    var headTable = tables[0];
                    var detailTable = tables[1];

                    if (headTable != null && headTable.Rows.Count > 0 && detailTable != null)
                    {
                        ProcessResultDetail(metaModel, resultModel, headTable, detailTable);
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception);
            }

            return resultModel;
        }

        #region Ultilities

        /// <summary>
        /// Get exchange rate.
        /// </summary>
        /// <param name="exchangeRate">Exchange rate data.</param>
        /// <param name="currency">Currency name.</param>
        /// <returns>Exchange rate value.</returns>
        private static decimal GetExchangeRate(ExChangeRate exchangeRate, string currency)
        {
            switch (currency.ToUpperInvariant())
            {
                case "EUR":
                    return exchangeRate.EUR;

                case "USD":
                    return exchangeRate.USD;

                case "GBP":
                    return exchangeRate.GBP;

                default:
                    return 1;
            }
        }

        /// <summary>
        /// Get value by base currency.
        /// </summary>
        /// <param name="originValue">The original value.</param>
        /// <param name="exchangeRate">The exchange rate.</param>
        /// <param name="currency">The base currency.</param>
        /// <returns>The converted value.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        private static decimal GetBaseCurrencyValue(string originValue, ExChangeRate exchangeRate, string currency)
        {
            var rate = GetExchangeRate(exchangeRate, currency);
            try
            {
                if (!string.IsNullOrEmpty(originValue))
                {
                    var array = originValue.Split(' ').ToList();
                    if (array.Any())
                    {
                        return Converter.ToDecimal(array.LastOrDefault()) * rate;
                    }
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception);
            }

            return 0;
        }

        /// <summary>
        /// Get coin size value by currency.
        /// </summary>
        /// <param name="originValue">The original value.</param>
        /// <param name="exchangeRate">The exchange rate.</param>
        /// <param name="currency">The base currency.</param>
        /// <returns>The converted value.</returns>
        private static string GetCoinSizeByBaseCurrency(string originValue, ExChangeRate exchangeRate, string currency)
        {
            var rate = GetExchangeRate(exchangeRate, currency);

            if (!string.IsNullOrEmpty(originValue))
            {
                var array = originValue.Split(' ');
                if (array.Length > 3)
                {
                    return (Converter.ToDecimal(array[1]) * rate) + " " + array[2] + array[3];
                }
            }

            return string.Empty;
        }

        private static string GetDataRowExtension(DataRow row, string columnName)
        {
            return row.Table.Columns.Contains(columnName) ? row[columnName].ToString() : string.Empty;
        }

        private static string GetDataRowExtensionMultiple(DataRow row, string columnName, string extColumnName)
        {
            return row.Table.Columns.Contains(columnName) ? row[columnName].ToString()
                    : row.Table.Columns.Contains(extColumnName) ? row[extColumnName].ToString()
                    : string.Empty;
        }

        private DataTable GetResultFromAPI(RNGCasinoResultMetaModel metalModel)
        {
            const string BetType = "Casino";
            var dataTable = _reportApiWrapper.WinLossHandHistory(metalModel.RefNo, BetType, RNGCasinoSetting.CasinoGroup1, metalModel.IsShowPic);
            if (dataTable == null)
            {
                dataTable = _reportApiWrapper.RealTimeWinLossHandHistory(
                metalModel.FromDate,
                metalModel.ToDate,
                metalModel.CustId,
                RNGCasinoSetting.CasinoGroup1,
                metalModel.IsShowPic);
            }

            return dataTable;
        }

        /// <summary>
        /// Process result detail.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="resultModel">The result.</param>
        /// <param name="headTable">The header data.</param>
        /// <param name="detailTable">The detail data.</param>
        private void ProcessResultDetail(
            RNGCasinoResultDetailMetaModel metaModel,
            CasinoHandHistoryDetailViewModel resultModel,
            DataTable headTable,
            DataTable detailTable)
        {
            // get exchange rate
            var betTime = Converter.ToDateTime(GetDataRowExtension(headTable.Rows[0], "BetTime"));
            var language = _langService.GetCurrentLanguage();
            ExChangeRate exchangeRate;
            if (detailTable.Rows.Count > 0)
            {
                var currency = GetDataRowExtension(detailTable.Rows[0], "Currency");
                exchangeRate = this._exchangeRateDataAccess.GetExChangeRate(currency, betTime);
            }
            else
            {
                exchangeRate = new ExChangeRate { EUR = 1, USD = 1, GBP = 1 };
            }

            foreach (DataRow row in headTable.Rows)
            {
                var headItem = MapDataToHeaderItem(row, language);

                headItem.CoinSizeBaseCurrencyValue = GetBaseCurrencyValue(headItem.CoinSize, exchangeRate, metaModel.CurrencyName);
                headItem.WinBaseCurrencyValue = GetBaseCurrencyValue(headItem.Win, exchangeRate, metaModel.CurrencyName);
                headItem.WinResultBaseCurrencyValue = GetBaseCurrencyValue(headItem.WinResult, exchangeRate, metaModel.CurrencyName);
                headItem.BetBaseCurrencyValue = GetBaseCurrencyValue(headItem.Bet, exchangeRate, metaModel.CurrencyName);
                headItem.WinLossResultBaseCurrecyValue = GetBaseCurrencyValue(headItem.WinLossResult, exchangeRate, metaModel.CurrencyName);
                headItem.CoinSizeAmount = GetCoinSizeByBaseCurrency(headItem.CoinSizeAmount, exchangeRate, metaModel.CurrencyName);

                resultModel.CasinoDetailHeaderList.Add(headItem);
            }

            foreach (DataRow row in detailTable.Rows)
            {
                var detailItem = MapDataToDetailItem(row, language);

                var rate = GetExchangeRate(exchangeRate, metaModel.CurrencyName);

                if (rate != 1)
                {
                    detailItem.Bet *= rate;
                    detailItem.Win *= rate;
                    detailItem.WinLoss *= rate;
                }

                resultModel.CasinoDetailList.Add(detailItem);
            }
        }

        #region Map Data

        /// <summary>
        /// Mapping data from API to DTO.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>The RNG Casino.</returns>
        private RNGCasinoResult MapDataToCasinoItem(DataRow row, string language)
        {
            var gameTypeName = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("GameTypeName", language));
            gameTypeName = gameTypeName.Replace("<BR>", " ");

            return new RNGCasinoResult
            {
                SessionLogId = GetDataRowExtension(row, "SessionLogId"),
                PlayerHistoryNo = GetDataRowExtension(row, "PlayerHistoryNo"),
                GameId = GetDataRowExtension(row, "GameId"),
                GameNo = GetDataRowExtension(row, "GameNo"),
                GameTypeId = Converter.ToInt(GetDataRowExtension(row, "GameTypeId")),
                BeginBalance = Converter.ToDecimal(GetDataRowExtension(row, "BeginBalance")),
                EndBlance = Converter.ToDecimal(GetDataRowExtension(row, "EndBalance")),
                Bet = Converter.ToDecimal(GetDataRowExtension(row, "Bet")),
                Stake = Converter.ToDecimal(GetDataRowExtension(row, "Stake")),
                Win = Converter.ToDecimal(GetDataRowExtension(row, "Win")),
                WinLoss = Converter.ToDecimal(GetDataRowExtension(row, "WinLoss")),
                UserBetId = Converter.ToInt(GetDataRowExtension(row, "UserBetID")),
                GameTableId = Converter.ToInt(GetDataRowExtension(row, "GameTableID")),
                StartTime = Converter.ToDateTime(GetDataRowExtension(row, "StartTime")).ToString("M/d/yyyy\nhh:mm:ss tt"),
                EndTime = Converter.ToDateTime(GetDataRowExtension(row, "EndTime")).ToString("M/d/yyyy\nhh:mm:ss tt"),
                ShoeNo = GetDataRowExtension(row, "ShoeNo"),
                HandNo = GetDataRowExtension(row, "HandNo"),
                ResultId = GetDataRowExtension(row, "ResultId"),
                UserName = GetDataRowExtension(row, "UserName"),
                Result = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("Result", language)),
                Status = Converter.ToInt(GetDataRowExtension(row, "Status")),
                GameGroupId = Converter.ToInt(GetDataRowExtension(row, "GameGroupId")),
                GameTypeNo = GetDataRowExtension(row, "GameTypeNo"),
                GameTypeName = gameTypeName,
                GameGroupName = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("GameGroupName", language)),
                Currency = GetDataRowExtension(row, "Currency"),
                TableName = GetDataRowExtension(row, "TableName"),
                RollingComm = Converter.ToDecimal(GetDataRowExtension(row, "RollComm")),
            };
        }

        /// <summary>
        /// Map data from API.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>The detail data.</returns>
        private CasinoWinLossDetail MapDataToDetailItem(DataRow row, string language)
        {
            return new CasinoWinLossDetail
            {
                BetTypeId = Converter.ToInt(GetDataRowExtension(row, "BetTypeId")),
                BetTypeName = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("BetTypeName", language)),
                Bet = Converter.ToDecimal(GetDataRowExtension(row, "Bet")),
                Odds = Converter.ToDecimal(GetDataRowExtension(row, "Odds")),
                Win = Converter.ToDecimal(GetDataRowExtension(row, "Win")),
                WinLoss = Converter.ToDecimal(GetDataRowExtension(row, "WinLoss")),
                GameTypeId = Converter.ToInt(GetDataRowExtension(row, "GameTypeID")),
                Currency = GetDataRowExtension(row, "Currency"),
                BetContent = GetDataRowExtension(row, "BetContent"),
            };
        }

        /// <summary>
        /// Map data from API for header section.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>The detail header data.</returns>
        private CasinoWinLossDetailHeader MapDataToHeaderItem(DataRow row, string language)
        {
            return new CasinoWinLossDetailHeader
            {
                ShowType = GetDataRowExtension(row, "ShowType"),
                TableName = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("TableName", language)),
                BetTime = Converter.ToDateTime(GetDataRowExtension(row, "BetTime")),
                UserName = GetDataRowExtension(row, "UserName"),
                ResultName = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("ResultName", language)),
                Result = GetDataRowExtension(row, "Result"),
                PlayerHistoryNo = GetDataRowExtension(row, "PlayerHistoryNo_Result"),
                Symbol = GetDataRowExtension(row, "Symbol"),
                CoinSize = GetDataRowExtension(row, "CoinSize"),
                CoinSizeAmount = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("CoinSizeAmount", language)),
                Bet = GetDataRowExtension(row, "Bet"),
                Odds = GetDataRowExtension(row, "Odds"),
                Multiplier = GetDataRowExtensionMultiple(row, "Multiplier_Result", "Multiplier"),
                Win = GetDataRowExtension(row, "Win"),
                WinResult = GetDataRowExtensionMultiple(row, "Win_Result", "WinResult"),
                ModeResult = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("ModeResult", language)),
                AutoSpinResult = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("AutoSpinResult", language)),
                FreeSpinResult = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("FreeSpinResult", language)),
                BonusName = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("BonusName", language)),
                LineNo = GetDataRowExtension(row, "LineNo"),
                TotalLineBet = GetDataRowExtension(row, "TotalLineBet"),
                RoundTypeResult = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("RoundTypeResult", language)),
                BetPerLine = GetDataRowExtension(row, "BetPerLine"),
                RankResult = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("RankResult", language)),
                FinalHandResult = GetDataRowExtension(row, "FinalResult"),
                BetTypeName = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("BetTypeName", language)),
                AutoPlayResult = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("AutoPlayResult", language)),
                ActionName = GetDataRowExtension(row, CasinoHelper.GetColumnNameByLanguage("ActionName", language)),
                WinLossResult = GetDataRowExtension(row, "WinLoss_Result"),
                HandNoResult = GetDataRowExtension(row, "HandNoResult"),
                PickNo = GetDataRowExtension(row, "Pick_No"),
                HitNo = GetDataRowExtension(row, "Hit_No"),
                PickCount = Converter.ToInt(GetDataRowExtension(row, "Pick_Count")),
                HitCount = Converter.ToInt(GetDataRowExtension(row, "Hit_Count")),
            };
        }

        #endregion Map Data

        #endregion Ultilities
    }
}