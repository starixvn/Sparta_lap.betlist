// --------------------------------------------------------------------------
// <copyright file="ForecastAhMetaModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.19">Created</change>
// </history>

namespace Fanex.Athena.Model.Forecast.MetaModel
{
    using System;

    /// <summary>
    /// ForecastAhMetaModel class.
    /// </summary>
    [Serializable]
    public class ForecastAHMetaModel : ForecastBaseMetaModel
    {
        public int CurrentScore { get; set; }

        public int MatchId { get; set; }
    }
}