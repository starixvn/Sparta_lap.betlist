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
    using NPOI.SS.UserModel;
    using Fanex.Athena.Web.ActionFilter;

    /// <summary>
    /// Base API Controller.
    /// </summary>
    [CustomApiAuthorize]
    public class BaseApiController : ApiController
    {
        public readonly string[] IgnoreCasinoAngBingo = new string[] { Product.Cs, Product.Bg };
        public readonly string[] IgnoreKeno = new string[] { Product.Kn };
        public readonly string[] IgnoreCasinoAndBingoAndKeno = new string[] { Product.Cs, Product.Bg, Product.Kn };

        protected readonly IProductService ProductService;
        protected readonly ICurrencyService CurrencyService;
        protected readonly ISessionService SessionService;
        protected readonly ILanguageService LangService;

        protected BaseApiController()
        {
            SessionService = new SessionService();
            ProductService = new ProductService();
            CurrencyService = new CurrencyService();
            LangService = new LanguageService();
        }


        /// <summary>
        /// Generate excel steam.
        /// </summary>
        /// <param name="workbook">Excel workbook.</param>
        /// <param name="fileName">Excel file name.</param>
        /// <returns>Http Response Message.</returns>
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
            var currentLanguage = LangService.GetCurrentLanguage();
            LangService.SetPageCulture(currentLanguage);
            base.Initialize(controllerContext);
        }
    }
}