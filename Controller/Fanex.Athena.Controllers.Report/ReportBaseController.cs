namespace Fanex.Athena.Controllers.Report
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using DataAccess.Core.Configuration;
    using DataAccess.Core.Configuration.Interfaces;
    using Model.Core.DTO.Configuration;
    using ONEbook.UM.Filters;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;

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

        protected IProductService ProductService
        {
            get { return _productService; }
        }

        protected BalanceUp GetBalanceUp()
        {
            return _balanceUpDataAccess.GetBalanceUp();
        }
    }
}