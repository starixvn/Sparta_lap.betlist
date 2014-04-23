using System;
using Fanex.Athena.Service.Implement.Configuration;
using Fanex.Athena.Service.Interface.Configuration;
using Fanex.Athena.Service.Interface.Common;
using Fanex.Athena.Service.Implement.Common;
using Fanex.Athena.Service.Interface.Integration;
using Fanex.Athena.Service.Implement.Integration;

public partial class sso : System.Web.UI.Page
{
    protected override void OnLoad(EventArgs e)
    {
        IMaintenanceService _maintenanceService = new MaintenanceService();

        if (_maintenanceService.MaintenanceInfo.IsDownTime)
        {
            Server.Transfer("~/Utility/UnderMaintenance");
        }
        else
        {
            ISSOService service = new SSOService();
            service.SSO();
        }
    }
}
