// -----------------------------------------------------------------------
// <copyright file="RNGCasinoService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Casino
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.Casino;
    using Fanex.Athena.Models.DTO.Casino.RNGResultDetail;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Models.ViewModel.Casino.RNGResultDetail;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Interface.Casino;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Common;
    using Fanex.Logging;

    /// <summary>
    /// RNG Casino service class.
    /// </summary>
    public class RNGCasinoService : IRNGCasinoService
    {
        private readonly ILanguageService _langService = new LanguageService();
        private readonly IExChangeRateService _exchangeRateService = new ExChangeRateService();
        private readonly string _subDomainPath = Extension.Utility.AppSettings.GetValue("SubDomainPath");

        public DataTable GetRNGCasinoResultDataTable(RNGCasinoResultMetalModel metaModel)
        {
            DataTable dataTable;
            GetResultFromAPIRealTime(metaModel, out dataTable);
            return dataTable;
        }

        /// <summary>
        /// Get RNG Casino Result.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The RNG Casino result list.</returns>
        public List<RNGCasinoResult> GetRNGCasinoResult(RNGCasinoResultMetalModel metaModel)
        {
            List<RNGCasinoResult> casinos = new List<RNGCasinoResult>();
            DataTable dataTable;
            GetResultFromAPI(metaModel, out dataTable);

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                var currency = GetDataRowExtension(dataTable.Rows[0], "Currency");
                var date = Converter.ToDateTime(GetDataRowExtension(dataTable.Rows[0], "StartTime"));
                var exchangeRate = this._exchangeRateService.GetExChangeRate(currency, date);
                for (int i = 0; i < dataTable.Rows.Count; i++)
                {
                    var dr = dataTable.Rows[i];
                    var casino = MapDataToCasinoItem(dr);
                    casino.Index = i + 1;
                    casino.Result = casino.Result.Replace("/Form", this._subDomainPath + "/Form");
                    casino.Result = casino.Result.Replace("/img", this._subDomainPath + "/img");
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
            var casinoLib = new CasinoLibrary.SBAgent.Report();
            try
            {
                var tables = casinoLib.WinLossDetail(metaModel.UserBetId);

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
                LogService.Error(exception.Message, exception);
            }

            return resultModel;
        }

        #region Commission

        /// <summary>
        /// Update member commission.
        /// </summary>
        /// <param name="metaModel">Meta model data.</param>
        /// <returns>Success or fail.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public bool UpdateMemberCommission(RNGCasinoUpdateMemberComMetaModel metaModel)
        {
            const string API_ERROR_MSG = "Found errors when calling casino API to update customer commission.\n customers=";
            const string AdditionErrorMsgPattern = "\n IP={0},CustomerId={1},ModifyBy={2},ModifyBy={3},System={4},Site={5}";
            var customer = new CasinoLibrary.SBAgent.Customer();
            var additionalErrorMsg = string.Format(
                AdditionErrorMsgPattern,
                metaModel.FromIp,
                string.Join(",", metaModel.CustomerId),
                metaModel.ModifyBy,
                metaModel.ModifyUserName,
                metaModel.FromSystem,
                metaModel.SiteName);
            var success = false;
            try
            {
                success = customer.UpdateComm(
                     metaModel.CustomerId,
                     metaModel.Comm,
                     metaModel.FromIp,
                     metaModel.ModifyBy,
                     metaModel.ModifyUserName,
                     metaModel.FromSystem);
                if (!success)
                {
                    LogService.Error(API_ERROR_MSG + string.Join(",", metaModel.CustomerId) + additionalErrorMsg);
                }
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message + additionalErrorMsg, exception);
            }

            return success;
        }

        #endregion Commission

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
        /// Get result from API.
        /// </summary>
        /// <param name="metalModel">The metal model.</param>
        /// <param name="dataTable">The data table.</param>
        private static void GetResultFromAPI(RNGCasinoResultMetalModel metalModel, out DataTable dataTable)
        {
            var casinoLib = new CasinoLibrary.SBAgent.Report();
            dataTable = casinoLib.WinLossHandHistory(metalModel.RefNo, metalModel.BetType, RNGCasinoSetting.CasinoGroup1, metalModel.IsShowPic);
            if (dataTable == null)
            {
                dataTable = casinoLib.RealTimeWinLossHandHistory(
                metalModel.FromDate,
                metalModel.ToDate,
                metalModel.CustId,
                RNGCasinoSetting.CasinoGroup1,
                metalModel.IsShowPic);
            }
        }

        private static void GetResultFromAPIRealTime(RNGCasinoResultMetalModel metalModel, out DataTable dataTable)
        {
            var casinoLib = new CasinoLibrary.SBAgent.Report();
            dataTable = casinoLib.RealTimeWinLossHandHistory(
            metalModel.FromDate,
            metalModel.ToDate,
            metalModel.CustId,
            RNGCasinoSetting.CasinoGroup1,
            metalModel.IsShowPic);
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
                LogService.Error(exception.Message, exception);
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

            ExChangeRate exchangeRate;
            if (detailTable.Rows.Count > 0)
            {
                var currency = GetDataRowExtension(detailTable.Rows[0], "Currency");
                exchangeRate = this._exchangeRateService.GetExChangeRate(currency, betTime);
            }
            else
            {
                exchangeRate = new ExChangeRate { EUR = 1, USD = 1, GBP = 1 };
            }

            foreach (DataRow row in headTable.Rows)
            {
                var headItem = MapDataToHeaderItem(row);

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
                var detailItem = MapDataToDetailItem(row);

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

        /// <summary>
        /// Get columns by language.
        /// </summary>
        /// <param name="columnName">The original column name.</param>
        /// <returns>The language column.</returns>
        private string GetColumnNameByCurrentLanguage(string columnName)
        {
            var langKey = "_En";
            var currentLang = this._langService.GetCurrentLanguage();

            switch (currentLang)
            {
                case "zh-CN":
                    langKey = "_Cs";
                    break;

                case "zh-TW":
                    langKey = "_Ct";
                    break;

                case "ko-KR":
                    langKey = "_Ko";
                    break;
            }

            return columnName + langKey;
        }

        #region Map Data

        /// <summary>
        /// Mapping data from API to DTO.
        /// </summary>
        /// <param name="row">The data row.</param>
        /// <returns>The RNG Casino.</returns>
        private RNGCasinoResult MapDataToCasinoItem(DataRow row)
        {
            var gameTypeName = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("GameTypeName"));
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
                Result = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("Result")),
                Status = Converter.ToInt(GetDataRowExtension(row, "Status")),
                GameGroupId = Converter.ToInt(GetDataRowExtension(row, "GameGroupId")),
                GameTypeNo = GetDataRowExtension(row, "GameTypeNo"),
                GameTypeName = gameTypeName,
                GameGroupName = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("GameGroupName")),
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
        private CasinoWinLossDetail MapDataToDetailItem(DataRow row)
        {
            return new CasinoWinLossDetail
            {
                BetTypeId = Converter.ToInt(GetDataRowExtension(row, "BetTypeId")),
                BetTypeName = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("BetTypeName")),
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
        private CasinoWinLossDetailHeader MapDataToHeaderItem(DataRow row)
        {
            return new CasinoWinLossDetailHeader
            {
                ShowType = GetDataRowExtension(row, "ShowType"),
                TableName = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("TableName")),
                BetTime = Converter.ToDateTime(GetDataRowExtension(row, "BetTime")),
                UserName = GetDataRowExtension(row, "UserName"),
                ResultName = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("ResultName")),
                Result = GetDataRowExtension(row, "Result"),
                PlayerHistoryNo = GetDataRowExtension(row, "PlayerHistoryNo_Result"),
                Symbol = GetDataRowExtension(row, "Symbol"),
                CoinSize = GetDataRowExtension(row, "CoinSize"),
                CoinSizeAmount = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("CoinSizeAmount")),
                Bet = GetDataRowExtension(row, "Bet"),
                Odds = GetDataRowExtension(row, "Odds"),
                Multiplier = GetDataRowExtensionMultiple(row, "Multiplier_Result", "Multiplier"),
                Win = GetDataRowExtension(row, "Win"),
                WinResult = GetDataRowExtensionMultiple(row, "Win_Result", "WinResult"),
                ModeResult = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("ModeResult")),
                AutoSpinResult = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("AutoSpinResult")),
                FreeSpinResult = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("FreeSpinResult")),
                BonusName = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("BonusName")),
                LineNo = GetDataRowExtension(row, "LineNo"),
                TotalLineBet = GetDataRowExtension(row, "TotalLineBet"),
                RoundTypeResult = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("RoundTypeResult")),
                BetPerLine = GetDataRowExtension(row, "BetPerLine"),
                RankResult = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("RankResult")),
                FinalHandResult = GetDataRowExtension(row, "FinalResult"),
                BetTypeName = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("BetTypeName")),
                AutoPlayResult = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("AutoPlayResult")),
                ActionName = GetDataRowExtension(row, GetColumnNameByCurrentLanguage("ActionName")),
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