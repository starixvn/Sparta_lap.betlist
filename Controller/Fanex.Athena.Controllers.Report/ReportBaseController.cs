namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using Model.Core.MetaModel.Product;
    using ONEbook.UM.Filters;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;
    using Model.Core.DataDomain;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class ReportBaseController : BaseController
    {
        public const int PageSizeExcel = int.MaxValue - 1000;

        private readonly IProductService _productService;

        public ReportBaseController()
        {
            _productService = new ProductService();
        }

        public ReportBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IProductService productService)
            : base(languageService, sessionService, appSetting)
        {
            _productService = productService ?? new ProductService();
        }

        protected IProductService ProductService
        {
            get { return _productService; }
        }

        protected static IList<ProductOption> ToProductOptions(IEnumerable<Product> productList)
        {
            return productList.Select(i => new ProductOption
            {
                Code = i.ProductCode,
                Name = i.ProductName,
                Status = true
            }).ToList();
        }

    }
}