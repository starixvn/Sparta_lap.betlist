// --------------------------------------------------------------------------
// <copyright file="ForecastOneXTwoMetaModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.MetaModel
{
    /// <summary>
    /// ForecastOneXTwoMetaModel class.
    /// </summary>
    public class ForecastOneXTwoMetaModel : ForecastBaseMetaModel
    {
        public ForecastOneXTwoMetaModel()
        {
            CurrencyId = 0;
            BetType = 5;
            BetTypeString = "5";
        }

        public int MatchId { get; set; }
    }
}