// --------------------------------------------------------------------------
// <copyright file="ForecastBaseMetaModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.26">Created</change>
// </history>

namespace Fanex.Athena.Model.Forecast.MetaModel
{
    using System;
    using Model.Core.MetaModel;

    /// <summary>
    /// ForecastBaseMetaModel class.
    /// </summary>
    [Serializable]
    public class ForecastBaseMetaModel : BaseMetaModel
    {
        public bool IsPopUp { get; set; }

        public int BetType { get; set; }

        public string BetTypeString { get; set; }

        public int LeagueId { get; set; }
    }
}