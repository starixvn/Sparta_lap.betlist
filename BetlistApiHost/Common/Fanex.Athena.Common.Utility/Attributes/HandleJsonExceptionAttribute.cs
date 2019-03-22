using System;
using System.Web.Mvc;
using Fanex.Logging.Extensions.Web;

namespace Fanex.Athena.Common.Utility.Attributes
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class HandleJsonExceptionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            if (filterContext.Exception != null)
            {
                ExceptionHandler.Handle(filterContext.Exception);
                filterContext.HttpContext.Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                filterContext.Result = new JsonResult
                {
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    Data = new
                    {
                        Error = filterContext.Exception.Message
                    }
                };
                filterContext.ExceptionHandled = true;
            }
        }
    }
}