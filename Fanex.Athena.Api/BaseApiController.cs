// --------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Users;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Users;
    using Fanex.Athena.Web.ActionFilter;
    using NPOI.SS.UserModel;
    using System.Web.Http.Filters;
    /// <summary>
    /// Base API Controller.
    /// </summary>
    [CustomApiAuthorize]
    [CustomAPIExceptionHandling]
    public class BaseApiController : ApiController
    {
        private readonly ILanguageService _langService;
        private readonly IProductService _productService;
        private readonly ISessionService _sessionService;

        protected BaseApiController()
        {
            _sessionService = new SessionService();
            _productService = new ProductService();
            _langService = new LanguageService();
        }

        protected IProductService ProductService
        {
            get
            {
                return _productService;
            }
        }

        protected ISessionService SessionService
        {
            get
            {
                return _sessionService;
            }
        }

        /// <summary>
        /// Generate excel steam.
        /// </summary>
        /// <param name="workbook">Excel workbook.</param>
        /// <param name="fileName">Excel file name.</param>
        /// <returns>Http Response Message.</returns>
        [System.CLSCompliant(false)]
        [NonAction]
        public static HttpResponseMessage ExcelStream(IWorkbook workbook, string fileName)
        {
            var memoryStream = new MemoryStream();
            workbook.Write(memoryStream);

            var result = new HttpResponseMessage(HttpStatusCode.OK) { Content = new ByteArrayContent(memoryStream.ToArray()) };
            result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

            // We used attachment to force download
            result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = fileName
            };

            return result;
        }

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            var currentLanguage = _langService.GetCurrentLanguage();
            _langService.SetPageCulture(currentLanguage);
            base.Initialize(controllerContext);
        }
    }
}