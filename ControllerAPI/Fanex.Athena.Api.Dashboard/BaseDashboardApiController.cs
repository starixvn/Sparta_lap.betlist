namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;

    public class BaseDashboardApiController : ApiController
    {
        protected void AssignAuthProperties(DashboardBaseModel model)
        {
            model.LicenseeId = (int)ActionContext.ActionArguments["licenseeid"];
            model.UserId = (int)ActionContext.ActionArguments["userid"];
        }
    }
}