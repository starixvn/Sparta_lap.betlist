// --------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Base
{
    using System.Web.Http;
    using Attributes;
    using ONEbook.UM.Filters;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;
    using Service.Core.Configuration.Interfaces;

    [CustomApiAuthorize]
    [CheckUMFilter]
    [XFrameOptionApi]
    public class CommonApiController : ApiController
    {
        private readonly IMoveBetService _moveBetService;

        private readonly ILanguageService _languageService;

        public CommonApiController()
        {
            _moveBetService = new MoveBetService();
            _languageService = new LanguageService();
        }

        [HttpGet]
        public string GetMoveBetNotification()
        {
            var language = _languageService.GetCurrentLanguage();

            return _moveBetService.GetMoveBetNotification(language);
        }
    }
}