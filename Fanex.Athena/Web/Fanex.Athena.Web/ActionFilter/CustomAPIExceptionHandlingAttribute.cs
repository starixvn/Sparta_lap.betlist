// -----------------------------------------------------------------------
// <copyright file="CustomAPIExceptionHandlingAttribute.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Web.ActionFilter
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Web.Http.Filters;
    using System.Web.Http;
    using System.Net.Http;
    using System.Net;
    using Fanex.Logging;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class CustomAPIExceptionHandlingAttribute : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext context)
        {
            LogService.Error(context.Exception.Message);
            base.OnException(context);
        }
    }
}
