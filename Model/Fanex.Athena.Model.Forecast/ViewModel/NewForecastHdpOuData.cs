// -----------------------------------------------------------------------
// <copyright file="NewForecastHdpOuData.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Forecast.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Forecast.DTO.NewForecastHdpOu;

    /// <summary>
    /// NewForecast HDP OU Data class.
    /// </summary>
    public class NewForecastHdpOuData
    {
        public NewForecastHdpOuData()
        {
            TotalItemList = new List<NewForecastHdpOuTotalItem>();
            DetailItemList = new List<NewForecastHdpOuDetailItem>();
        }

        public IList<NewForecastHdpOuTotalItem> TotalItemList { get; set; }

        public IList<NewForecastHdpOuDetailItem> DetailItemList { get; set; }
    }
}