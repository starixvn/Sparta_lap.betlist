// --------------------------------------------------------------------------
// <copyright file="ForecastService.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Service.Forecast
{
    using System.Collections.Generic;
    using System.Data;
    using DataAccess.Core.Common;
    using DataAccess.Core.Common.Interfaces;
    using DataAccess.Forecast;
    using DataAccess.Forecast.Interfaces;
    using Extension.Helper;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Model.Forecast.DTO.ForecastHdou;
    using Fanex.Common;
    using Interfaces;
    using Model.Forecast.DTO.Forecast1X2;
    using Model.Forecast.DTO.ForecastAH;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;

    /// <summary>
    /// Forecast Service.
    /// </summary>
    public class ForecastService : IForecastService
    {
        private readonly ILeagueDataAccess _leagueDataAccess;

        private readonly IForecastDataAccess _forecastDataAccess;

        public ForecastService()
        {
            _leagueDataAccess = new LeagueDataAccess();
            _forecastDataAccess = new ForecastDataAccess();
        }

        public ForecastService(ILeagueDataAccess leagueDataAccess, IForecastDataAccess forecastDataAccess)
        {
            _leagueDataAccess = leagueDataAccess;
            _forecastDataAccess = forecastDataAccess;
        }

        #region Forecast AH

        public ForecastAH GetForecastTopOddsByMatch(int matchId, int betType)
        {
            return _forecastDataAccess.GetForecastTopOddsByMatch(matchId, betType);
        }

        public ForecastValue GetForecastHandicap(ForecastAHMetaModel metaModel, int userId)
        {
            var forecastHandicapTable = _forecastDataAccess.GetForecastHandicap(metaModel, userId);
            var forecastResult = new ForecastValue();

            if (forecastHandicapTable != null && forecastHandicapTable.Rows.Count > 0)
            {
                foreach (DataRow row in forecastHandicapTable.Rows)
                {
                    forecastResult.ForeCastValue0 =
                        GetForecastValue(row, metaModel.CurrentScore, -2, forecastResult.ForeCastValue0, metaModel.IsConvert, metaModel.BaseCurrency);
                    forecastResult.ForeCastValue1 = GetForecastValue(
                        row,
                        metaModel.CurrentScore,
                        -1,
                        forecastResult.ForeCastValue1,
                        metaModel.IsConvert,
                        metaModel.BaseCurrency);
                    forecastResult.ForeCastValue2 = GetForecastValue(
                        row,
                        metaModel.CurrentScore,
                        0,
                        forecastResult.ForeCastValue2,
                        metaModel.IsConvert,
                        metaModel.BaseCurrency);
                    forecastResult.ForeCastValue3 = GetForecastValue(
                        row,
                        metaModel.CurrentScore,
                        1,
                        forecastResult.ForeCastValue3,
                        metaModel.IsConvert,
                        metaModel.BaseCurrency);
                    forecastResult.ForeCastValue4 = GetForecastValue(
                        row,
                        metaModel.CurrentScore,
                        2,
                        forecastResult.ForeCastValue4,
                        metaModel.IsConvert,
                        metaModel.BaseCurrency);
                }

                forecastResult.HasData = true;
            }
            else
            {
                forecastResult.HasData = false;
            }

            return forecastResult;
        }

        public ForecastValue GetForecastOverUnder(ForecastAHMetaModel metaModel, int userId)
        {
            var overUnderTable = _forecastDataAccess.GetForecastOverUnder(metaModel, userId);
            var result = new ForecastValue();

            if (overUnderTable != null && overUnderTable.Rows.Count > 0)
            {
                foreach (DataRow row in overUnderTable.Rows)
                {
                    result.ForeCastValue0 =
                        GetForecastValue(row, metaModel.CurrentScore, 0, result.ForeCastValue0, metaModel.IsConvert, metaModel.BaseCurrency);
                    result.ForeCastValue1 =
                        GetForecastValue(row, metaModel.CurrentScore, 1, result.ForeCastValue1, metaModel.IsConvert, metaModel.BaseCurrency);
                    result.ForeCastValue2 =
                        GetForecastValue(row, metaModel.CurrentScore, 2, result.ForeCastValue2, metaModel.IsConvert, metaModel.BaseCurrency);
                    result.ForeCastValue3 =
                        GetForecastValue(row, metaModel.CurrentScore, 3, result.ForeCastValue3, metaModel.IsConvert, metaModel.BaseCurrency);
                    result.ForeCastValue4 =
                        GetForecastValue(row, metaModel.CurrentScore, 4, result.ForeCastValue4, metaModel.IsConvert, metaModel.BaseCurrency);
                }

                result.HasData = true;
            }
            else
            {
                result.HasData = false;
            }

            return result;
        }

        #endregion Forecast AH

        #region Forecast 1 x 2

        public IEnumerable<League> GetForecastRunningLeague(ForecastBaseMetaModel metaModel)
        {
            return _leagueDataAccess.GetRunningLeagues(metaModel.AgentListXml, metaModel.BetTypeString);
        }

        public IList<ForecastOneXTwo> GetForecastOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel)
        {
            return _forecastDataAccess.GetForecastOneXTwoByMatch(metaModel);
        }

        public IList<ForecastOneXTwo> GetForecastOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel)
        {
            return _forecastDataAccess.GetForecastOneXTwoByLeague(metaModel);
        }

        #endregion Forecast 1 x 2

        #region Forecast Handicap Over/Under

        public IList<ForecastHandicapOverUnder> GetForecastHandicapOverUnder(ForecastHandicapOverUnderMetaModel metaModel)
        {
            return _forecastDataAccess.GetForecastHandicapOverUnder(metaModel);
        }

        #endregion Forecast Handicap Over/Under

        #region New Forecast

        public NewForecastHdpOuData GetNewForecastData(NewForecastHandicapOverUnderMetaModel metaModel, NewForecastHdpOuMatchInfo headerInfo)
        {
            return metaModel.IsValid
                ? _forecastDataAccess.GetNewForecastData(metaModel, headerInfo)
                : new NewForecastHdpOuData();
        }

        #endregion New Forecast

        #region Utility

        private static decimal GetForecastValue(
            DataRow dataRow,
            int currentScore,
            int scoreOffset,
            decimal forecast,
            bool isConvert,
            string baseCurrency)
        {
            const decimal OneHalf = 0.5m;
            var alcolumnName = DataHelper.GetCurrencyColumnName("al", isConvert, baseCurrency);
            var hlcolumnName = DataHelper.GetCurrencyColumnName("hl", isConvert, baseCurrency);
            var offset = ConvertToDec(dataRow["offset"].ToString());
            var forecastScore = currentScore - offset + scoreOffset;

            if (offset % 1 == 0 && forecastScore == 0)
            {
                return forecast;
            }

            if (forecastScore > 0 && forecastScore < OneHalf)
            {
                return forecast + (ConvertToDec(dataRow[alcolumnName].ToString()) / 2);
            }
            else if (forecastScore >= OneHalf)
            {
                return forecast + ConvertToDec(dataRow[alcolumnName].ToString());
            }
            else if (forecastScore < 0 && forecastScore > -OneHalf)
            {
                return forecast + (ConvertToDec(dataRow[hlcolumnName].ToString()) / 2);
            }
            else
            {
                return forecast + ConvertToDec(dataRow[hlcolumnName].ToString());
            }
        }

        #region Convert datarow value to decimal

        private static decimal ConvertToDec(string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
            {
                return 0;
            }

            return Converter.ToDecimal(stringValue);
        }

        #endregion Convert datarow value to decimal

        #endregion Utility
    }
}