namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using System.Web.Http.Cors;
    using Base.Attributes;
    using Model.Dashboard.MetaModel;

    [AllowAnonymous]
    [ApiOneAuthorization]
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class BaseDashboardApiController : ApiController
    {
        protected int LicenseeId => (int)ActionContext.ActionArguments["licenseeid"];

        protected int UserId => (int)ActionContext.ActionArguments["userid"];

        protected void AssignAuthProperties(DashboardBaseModel model)
        {
            model.LicenseeId = this.LicenseeId;
        }
    }
}