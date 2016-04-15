namespace Fanex.Athena.Web.ActionFilter
{
    using System.Web.Mvc;
    using System.Web.Routing;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;

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