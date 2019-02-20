using System.Web.Mvc;
using System.Web.SessionState;
using Fanex.Athena.Common.Utility.Attributes;
using ONEbook.UM.Filters;

namespace Fanex.Athena.Result.Controller
{
    [SessionState(SessionStateBehavior.ReadOnly)]
    [CustomAuth]
    [CheckUMFilter]
    public class BaseResultController : System.Web.Mvc.Controller
    {
    }
}