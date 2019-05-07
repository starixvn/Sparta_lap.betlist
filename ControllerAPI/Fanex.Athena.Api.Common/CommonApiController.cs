// --------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Base
{
    using System.Threading.Tasks;
    using System.Web.Http;
    using Attributes;
    using ONEbook.UM.Filters;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration.Interfaces;

    [CustomApiAuthorize]
    [CheckUMFilter]
    [XFrameOptionApi]
    public class CommonApiController : ApiController
    {
        private readonly IMoveBetService _moveBetService;

        private readonly ILanguageService _languageService;

        public CommonApiController(IMoveBetService moveBetService, ILanguageService languageService)
        {
            _moveBetService = moveBetService;
            _languageService = languageService;
        }

        [HttpGet]
        public async Task<string> GetMoveBetNotification()
        {
            var language = _languageService.GetCurrentLanguage();

            return await _moveBetService.GetMoveBetNotificationAsync(language).ConfigureAwait(false);
        }
    }
}