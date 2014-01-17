// --------------------------------------------------------------------------
// <copyright file="ForecastService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.19">Created</change>
// </history>

namespace Fanex.Athena.Service.Implement.Forecast
{
    using System.Collections.Generic;
    using System.Data;
    using DataAccess.Extension;
    using DataAccess.Implement.Forecast;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.DTO.Forecast.NewForecastHdpOu;
    using Fanex.Common;
    using Interface.Forecast;
    using Models.DTO.Forecast.Forecast1X2;
    using Models.DTO.Forecast.ForecastAH;
    using Models.DTO.Forecast.ForecastHDOU;
    using Models.MetaModel.Forecast;
    using Models.ViewModel.Forecast;

    /// <summary>
    /// Forecast Service.
    /// </summary>
    public class ForecastService : IForecastService
    {
        #region Fields

        private readonly ForecastDataAccess _forecastDataAccess;

        #endregion Fields

        #region Constructor

        public ForecastService()
        {
            this._forecastDataAccess = new ForecastDataAccess();
        }

        #endregion Constructor

        #region Forecast AH

        /// <summary>
        /// Get forecast top odds by match.
        /// </summary>
        /// <param name="matchId">The match id.</param>
        /// <param name="betType">The bet type.</param>
        /// <returns>The Forecast data.</returns>
        public ForecastAh GetFcTopOddsByMatch(int matchId, int betType)
        {
            return _forecastDataAccess.GetFcTopOddsByMatch(matchId, betType);
        }

        /// <summary>
        /// Get forecast handicap.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The forecast handicap data.</returns>
        public ForecastValue GetForecastHandicap(ForecastAhMetaModel metaModel, int userId)
        {
            var dtfcHandicap = _forecastDataAccess.GetForecastHandicap(metaModel, userId);
            var fcresult = new ForecastValue();

            if (dtfcHandicap.Rows.Count > 0)
            {
                foreach (DataRow dr in dtfcHandicap.Rows)
                {
                    fcresult.ForeCastValue0 =
                        GetFcHandicapValue(dr, metaModel.CurrentScore, -2, fcresult.ForeCastValue0, metaModel.IsConvert, metaModel.BaseCurrency);
                    fcresult.ForeCastValue1 = GetFcHandicapValue(
                        dr,
                        metaModel.CurrentScore,
                        -1,
                        fcresult.ForeCastValue1,
                        metaModel.IsConvert,
                        metaModel.BaseCurrency);
                    fcresult.ForeCastValue2 = GetFcHandicapValue(
                        dr,
                        metaModel.CurrentScore,
                        0,
                        fcresult.ForeCastValue2,
                        metaModel.IsConvert,
                        metaModel.BaseCurrency);
                    fcresult.ForeCastValue3 = GetFcHandicapValue(
                        dr,
                        metaModel.CurrentScore,
                        1,
                        fcresult.ForeCastValue3,
                        metaModel.IsConvert,
                        metaModel.BaseCurrency);
                    fcresult.ForeCastValue4 = GetFcHandicapValue(
                        dr,
                        metaModel.CurrentScore,
                        2,
                        fcresult.ForeCastValue4,
                        metaModel.IsConvert,
                        metaModel.BaseCurrency);
                }

                fcresult.HasData = true;
            }
            else
            {
                fcresult.HasData = false;
            }

            return fcresult;
        }

        /// <summary>
        /// Get forecast over under.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <param name="userId">The user id.</param>
        /// <returns>The forecast over under data.</returns>
        public ForecastValue GetForacastOverUnder(ForecastAhMetaModel metaModel, int userId)
        {
            var dtoverUnder = _forecastDataAccess.GetForecastOverUnder(metaModel, userId);
            var ouresult = new ForecastValue();

            if (dtoverUnder.Rows.Count > 0)
            {
                foreach (DataRow dr in dtoverUnder.Rows)
                {
                    ouresult.ForeCastValue0 =
                        GetFcOverUnderValue(dr, metaModel.CurrentScore, 0, ouresult.ForeCastValue0, metaModel.IsConvert, metaModel.BaseCurrency);
                    ouresult.ForeCastValue1 =
                        GetFcOverUnderValue(dr, metaModel.CurrentScore, 1, ouresult.ForeCastValue1, metaModel.IsConvert, metaModel.BaseCurrency);
                    ouresult.ForeCastValue2 =
                        GetFcOverUnderValue(dr, metaModel.CurrentScore, 2, ouresult.ForeCastValue2, metaModel.IsConvert, metaModel.BaseCurrency);
                    ouresult.ForeCastValue3 =
                        GetFcOverUnderValue(dr, metaModel.CurrentScore, 3, ouresult.ForeCastValue3, metaModel.IsConvert, metaModel.BaseCurrency);
                    ouresult.ForeCastValue4 =
                        GetFcOverUnderValue(dr, metaModel.CurrentScore, 4, ouresult.ForeCastValue4, metaModel.IsConvert, metaModel.BaseCurrency);
                }

                ouresult.HasData = true;
            }
            else
            {
                ouresult.HasData = false;
            }

            return ouresult;
        }

        #endregion Forecast AH

        #region Forecast 1 x 2

        public IList<League> GetFcRunningLeague(ForecastBaseMetaModel metaModel)
        {
            return _forecastDataAccess.GetFcRunningLeague(metaModel);
        }

        public IList<ForecastOneXTwo> GetFcOneXTwoByMatch(ForecastOneXTwoMetaModel metaModel, int userId)
        {
            return _forecastDataAccess.GetFcOneXTwoByMatch(metaModel, userId);
        }

        public IList<ForecastOneXTwo> GetFcOneXTwoByLeague(ForecastOneXTwoMetaModel metaModel)
        {
            return _forecastDataAccess.GetFcOneXTwoByLeague(metaModel);
        }

        #endregion Forecast 1 x 2

        #region Forecast Handicap Over/Under

        public IList<ForecastHandicapOverUnder> GetFcHandicapOverUnder(ForecastHdpOuMetaModel metaModel)
        {
            return _forecastDataAccess.GetFcHandicapOverUnder(metaModel);
        }

        #endregion Forecast Handicap Over/Under

        #region New Forecast

        public NewForecastHdpOuData GetNewForecastData(NewForecastHdpOuMetaModel metaModel, NewForecastHdpOuMatchInfo headerInfo)
        {
            return metaModel.IsValid
                ? _forecastDataAccess.GetNewForecastData(metaModel, headerInfo)
                : new NewForecastHdpOuData();
        }

        #endregion New Forecast

        #region Utility

        /// <summary>
        /// Get Handicap value.
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <param name="currentScore">The current score.</param>
        /// <param name="scoreOffset">The score off set.</param>
        /// <param name="forecast">The forecast.</param>
        /// <param name="isConvert">True if you want to convert values to base currency.</param>
        /// <param name="baseCurrency">The base currency.</param>
        /// <returns>The Handicap value.</returns>
        private static decimal GetFcHandicapValue(
            DataRow dr,
            int currentScore,
            int scoreOffset,
            decimal forecast,
            bool isConvert,
            string baseCurrency)
        {
            decimal oneHalf = new decimal(0.5);
            var alcolumnName = DataHelper.GetCurrencyColumnName("al", isConvert, baseCurrency);
            var hlcolumnName = DataHelper.GetCurrencyColumnName("hl", isConvert, baseCurrency);
            var offset = ConvertToDec(dr["offset"].ToString());
            var forecastScore = Converter.ToDecimal(currentScore - offset + scoreOffset);

            if (offset % 1 == 0 && forecastScore == 0)
            {
                return forecast;
            }

            if (forecastScore > 0 && forecastScore < oneHalf)
            {
                forecast += ConvertToDec(dr[alcolumnName].ToString()) / 2;
            }
            else if (forecastScore >= oneHalf)
            {
                forecast += ConvertToDec(dr[alcolumnName].ToString());
            }
            else if (forecastScore < 0 && forecastScore > -oneHalf)
            {
                forecast += ConvertToDec(dr[hlcolumnName].ToString()) / 2;
            }
            else if (forecastScore <= oneHalf)
            {
                forecast += ConvertToDec(dr[hlcolumnName].ToString());
            }

            return forecast;
        }

        /// <summary>
        /// Get Over/Under value.
        /// </summary>
        /// <param name="dr">The data row.</param>
        /// <param name="currentScore">The current score.</param>
        /// <param name="scoreOffset">The score off set.</param>
        /// <param name="forecast">The forecast.</param>
        /// <param name="isconvert">True if you want to convert values to base currency.</param>
        /// <param name="baseCurrency">The base currency.</param>
        /// <returns>The Over/Under value.</returns>
        private static decimal GetFcOverUnderValue(
            DataRow dr,
            int currentScore,
            int scoreOffset,
            decimal forecast,
            bool isconvert,
            string baseCurrency)
        {
            decimal oneHalf = new decimal(0.5);
            var alcolumnName = DataHelper.GetCurrencyColumnName("al", isconvert, baseCurrency);
            var hlcolumnName = DataHelper.GetCurrencyColumnName("hl", isconvert, baseCurrency);
            var offset = ConvertToDec(dr["offset"].ToString());
            var forecastScore = currentScore - offset + scoreOffset;

            if (offset % 1 == 0 && forecastScore == 0)
            {
                return forecast;
            }

            if (forecastScore > 0 && forecastScore < oneHalf)
            {
                forecast += ConvertToDec(dr[alcolumnName].ToString()) / 2;
            }
            else if (forecastScore >= oneHalf)
            {
                forecast += ConvertToDec(dr[alcolumnName].ToString());
            }
            else if (forecastScore < 0 && forecastScore > -oneHalf)
            {
                forecast += ConvertToDec(dr[hlcolumnName].ToString()) / 2;
            }
            else if (forecastScore <= oneHalf)
            {
                forecast += ConvertToDec(dr[hlcolumnName].ToString());
            }

            return forecast;
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