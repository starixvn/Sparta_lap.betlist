// -----------------------------------------------------------------------
// <copyright file="CustomAPIExceptionHandlingAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Web.Http.Filters;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public sealed class CustomAPIExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            LogService.Error(actionExecutedContext.Exception.Message);
            base.OnException(actionExecutedContext);
        }
    }
}
