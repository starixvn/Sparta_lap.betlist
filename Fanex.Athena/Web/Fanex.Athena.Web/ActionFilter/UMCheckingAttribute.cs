

namespace Fanex.Athena.Web.ActionFilter
{
    using Service.Implement.Configuration;
    using Service.Interface.Configuration;
    using System.Web.Mvc;
    using System.Web.Routing;
    public class UMCheckingAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            IMaintenanceService maintenanceService = new MaintenanceService();

            if (maintenanceService.MaintenanceInfo.IsDownTime)
            {
                var routeValue = new
                {
                    controller = "Utility",
                    action = "UnderMaintenance"

                };

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(routeValue));
            }
            
            base.OnActionExecuting(filterContext);
        }
    }
}
