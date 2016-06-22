// --------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Api.Base
{
    using System.IO;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Attributes;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using NPOI.SS.UserModel;
    using ONEbook.UM.Filters;

    /// <summary>
    /// Base API Controller.
    /// </summary>
    [CustomApiAuthorize]
    [CustomAPIExceptionHandling]
    [CheckUMFilter]
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
        protected static HttpResponseMessage ExcelStream(IWorkbook workbook, string fileName)
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
            base.Initialize(controllerContext);
            var currentLanguage = _langService.GetCurrentLanguage();
            _langService.SetPageCulture(currentLanguage);
        }
    }
}