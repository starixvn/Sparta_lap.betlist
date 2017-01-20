namespace Fanex.Athena.DataAccess.Dashboard
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Dashboard.Interfaces;
    using Model.Dashboard.DTO;
    using Model.Dashboard.MetaModel;
    using Model.Dashboard.ViewModel;

    public class TurnoverByPlatformDataAccess : IWidgetByPlatformDataAccess
    {
        public WidgetByPlatformViewModel GetData(DashboardBaseModel metaModel)
        {
            return new WidgetByPlatformViewModel
            {
                Data = new List<Platform>
                {
                    new Platform { Name = "Asia", Value = 100M },
                    new Platform { Name = "China", Value = 300M },
                    new Platform { Name = "WAP", Value = 800M },
                    new Platform { Name = "Smartsite", Value = 700M },
                    new Platform { Name = "Others", Value = 1700M }
                }
            };
        }
    }
}