// --------------------------------------------------------------------------
// <copyright file="ForecastOneXTwoMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.25">Created</change>
// </history>

namespace Fanex.Athena.Models.MetaModel.Forecast
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