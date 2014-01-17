// --------------------------------------------------------------------------
// <copyright file="ForecastHandicapOverUnder.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.26">Created</change>
// </history>

namespace Fanex.Athena.Models.DTO.Forecast.ForecastHDOU
{
    /// <summary>
    /// ForecastHandicapOverUnder class.
    /// </summary>
    public class ForecastHandicapOverUnder : ForecastBase
    {
        // Offset
        public int OffsetHdpMinus1 { get; set; }

        public int OffsetHdp { get; set; }

        public int OffsetHdpPlus1 { get; set; }

        public int OffsetOuPlus2 { get; set; }

        public int OffsetOuPlus1 { get; set; }

        public int OffsetOu { get; set; }

        // Full time
        public decimal HandicapMinus1 { get; set; }

        public decimal Handicap { get; set; }

        public decimal HandicapPlus1 { get; set; }

        public decimal OverUnderPlus2 { get; set; }

        public decimal OverUnderPlus1 { get; set; }

        public decimal OverUnder { get; set; }

        // Half time
        public decimal HtHandicapMinus1 { get; set; }

        public decimal HtHandicap { get; set; }

        public decimal HtHandicapPlus1 { get; set; }

        public decimal HtOverUnderPlus2 { get; set; }

        public decimal HtOverUnderPlus1 { get; set; }

        public decimal HtOverUnder { get; set; }
    }
}