// --------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.BetList.Controller
{
    using System.Globalization;
    using System.Threading;
    using System.Web.Mvc;
    using System.Web.Routing;
    using System.Web.SessionState;
    using Common.Utility.Attributes;
    using Fanex.Athena.BetList.Controller.CustomActionResult;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CustomAuth]
    [HandleJsonException]
    public class BaseController : Controller
    {
        private const string LANGUAGEKEY = "Language";

        protected override void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);

            var languageCode = requestContext.HttpContext.Request.Headers[LANGUAGEKEY];
            if (!string.IsNullOrWhiteSpace(languageCode))
            {
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(languageCode);
            }
        }

        protected new JsonResult Json(object data)
        {
            return new JsonNetResult
            {
                Data = data,
                MaxJsonLength = int.MaxValue
            };
        }
    }
}