namespace Fanex.Athena.DataAccess.Dashboard
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class LicenseeWinLossByPlatformDataAccess : IWidgetByPlatformDataAccess
    {
        public WidgetByPlatformViewModel GetData(DashboardBaseModel metaModel)
        {
            return new WidgetByPlatformViewModel
            {
                Data = new List<KeyValuePair<string, decimal>>()
            };
        }
    }
}