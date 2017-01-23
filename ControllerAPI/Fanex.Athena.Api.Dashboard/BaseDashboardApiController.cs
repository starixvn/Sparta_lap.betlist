namespace Fanex.Athena.Api.Dashboard
{
    using System.Web.Http;
    using Model.Dashboard.MetaModel;

    public class BaseDashboardApiController : ApiController
    {
        protected int LicenseeId
        {
            get
            {
                return (int)ActionContext.ActionArguments["licenseeid"];
            }
        }

        protected int UserId
        {
            get
            {
                return (int)ActionContext.ActionArguments["userid"];
            }
        }

        protected void AssignAuthProperties(DashboardBaseModel model)
        {
            model.LicenseeId = this.LicenseeId;
            model.UserId = this.UserId;
        }
    }
}