// -----------------------------------------------------------------------
// <copyright file="NewForecastHdpOuData.cs" company="nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Forecast
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Forecast.NewForecastHdpOu;

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