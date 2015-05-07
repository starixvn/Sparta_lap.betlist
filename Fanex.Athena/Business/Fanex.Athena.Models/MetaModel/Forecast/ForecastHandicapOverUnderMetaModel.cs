// --------------------------------------------------------------------------
// <copyright file="ForecastHandicapOverUnderMetaModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.26">Created</change>
// </history>

namespace Fanex.Athena.Models.MetaModel.Forecast
{
    /// <summary>
    /// Forecast HDP OU MetaModel class.
    /// </summary>
    public class ForecastHandicapOverUnderMetaModel : ForecastBaseMetaModel
    {
        public ForecastHandicapOverUnderMetaModel()
        {
            BetTypeString = "1";
            CurrencyId = 0;
            BetType = 1;
        }
    }
}