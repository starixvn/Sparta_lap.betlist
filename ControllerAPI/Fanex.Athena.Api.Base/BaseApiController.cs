// --------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.Base
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Http;
    using System.Net.Http.Headers;
    using System.Web.Http;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using NPOI.SS.UserModel;
    using ONEbook.UM.Filters;

    [CheckUMFilter]
    [XFrameOptionApi]
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

        protected BaseApiController(ISessionService sessionService, IProductService productService, ILanguageService languageService)
        {
            _sessionService = sessionService;
            _productService = productService;
            _langService = languageService;
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
        protected internal virtual HttpResponseMessage ExcelStream(IWorkbook workbook, string fileName)
        {
            var result = new HttpResponseMessage(HttpStatusCode.OK);
            using (var memoryStream = new MemoryStream())
            {
                workbook.Write(memoryStream);
                result.Content = new ByteArrayContent(memoryStream.ToArray());
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = fileName
                };
            }

            return result;
        }

        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);
            var currentLanguage = _langService.GetCurrentLanguage();
            _langService.SetPageCulture(currentLanguage);
        }

        protected string RemoveInvalidProducts(string productCodeList, string functionProductConfigKey)
        {
            if (string.IsNullOrWhiteSpace(productCodeList))
            {
                return string.Empty;
            }

            var productList = ProductService.GetProductAccess(
                SessionService.LicenseeSession.LicenseeId,
                SessionService.UserSession.Language,
                functionProductConfigKey).ProductList;

            var validProductCodes = productCodeList.Split(',').Where(x => productList.Any(y => y.ProductCode.Equals(x, StringComparison.OrdinalIgnoreCase)));
            return string.Join(",", validProductCodes.ToArray());
        }
    }
}