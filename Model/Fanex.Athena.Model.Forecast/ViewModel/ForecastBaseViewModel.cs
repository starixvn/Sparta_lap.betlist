// --------------------------------------------------------------------------
// <copyright file="ForecastBaseViewModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.ViewModel
{
    using System.Collections.Generic;

    public class ForecastBaseViewModel
    {
        public IList<string> LicenseeTypeList { get; set; }

        public IDictionary<int, string> CurrencyList { get; set; }
    }
}