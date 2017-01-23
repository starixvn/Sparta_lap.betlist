// --------------------------------------------------------------------------
// <copyright file="ForecastBaseMetaModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

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