namespace Fanex.Athena.Controllers.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility;
    using Model.Core.DTO.Common;
    using Model.Core.DTO.Configuration;
    using Model.Core.MetaModel.Product;
    using ONEbook.UM.Filters;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Users.Interfaces;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class ReportBaseController : BaseController
    {
        public const int PageSizeExcel = int.MaxValue - 1000;

        private readonly IBalanceUpDataAccess _balanceUpDataAccess;
        private readonly IProductService _productService;

        public ReportBaseController()
        {
            _balanceUpDataAccess = new BalanceUpDataAccess();
            _productService = new ProductService();
        }

        public ReportBaseController(
            ILanguageService languageService,
            ISessionService sessionService,
            IAppSettings appSetting,
            IBalanceUpDataAccess balanceUpDataAccess,
            IProductService productService)
            : base(languageService, sessionService, appSetting)
        {
            _balanceUpDataAccess = balanceUpDataAccess;
            _productService = productService;
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

        protected BalanceUp GetBalanceUp()
        {
            return _balanceUpDataAccess.GetBalanceUp();
        }
    }
}