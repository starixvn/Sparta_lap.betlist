namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using ONEbook.UM.Filters;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CheckUMFilter]
    public class ReportBaseController : BaseController
    {
        public const int PageSizeExcel = int.MaxValue - 1000;

        private readonly IBalanceUpService _balanceUpService;
        private readonly IProductService _productService;

        public ReportBaseController()
            : base()
        {
            _balanceUpService = new BalanceUpService();
            _productService = new ProductService();
        }

        protected IBalanceUpService BalanceUpService
        {
            get { return _balanceUpService; }
        }

        protected IProductService ProductService
        {
            get { return _productService; }
        }
    }
}