// --------------------------------------------------------------------------
// <copyright file="ForecastBaseViewModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2012.12.26">Created</change>
// </history>

namespace Fanex.Athena.Model.Forecast.ViewModel
{
    using System.Collections.Generic;

    /// <summary>
    /// ForecastBaseViewModel class.
    /// </summary>
    public class ForecastBaseViewModel
    {
        public IList<string> LicenseeTypeList { get; set; }

        public IDictionary<string, int> CurrencyList { get; set; }
    }
}