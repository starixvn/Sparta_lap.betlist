// -----------------------------------------------------------------------
// <copyright file="HandleAntiForgeryErrorAttribute.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Web.Mvc;
    using System.Web.Security;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class HandleAntiForgeryErrorAttribute : ActionFilterAttribute, IExceptionFilter
    {
        public void OnException(ExceptionContext filterContext)
        {
            var exception = filterContext.Exception as HttpAntiForgeryException;
            if (exception != null)
            {
                filterContext.Result = new RedirectResult(FormsAuthentication.DefaultUrl);
                filterContext.ExceptionHandled = true;
            }
        }
    }
}