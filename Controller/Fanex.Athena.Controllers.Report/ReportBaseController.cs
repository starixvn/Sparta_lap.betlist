namespace Fanex.Athena.Controllers.Report
{
    using System;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Resources;
    using Service.Core.Common;
    using Service.Core.Common.Interfaces;
    using Service.Core.Configuration;

    public class ReportBaseController : ReadonlyBaseController
    {
        public const int PageSizeExcel = int.MaxValue - 1000;

        private readonly IBalanceUpService _balanceUpService;
        private readonly IProductService _productService;
        private readonly IMoveBetService _moveBetService;

        public ReportBaseController()
            : base()
        {
            _balanceUpService = new BalanceUpService();
            _productService = new ProductService();
            _moveBetService = new MoveBetService();
        }

        protected IBalanceUpService BalanceUpService
        {
            get { return _balanceUpService; }
        }

        protected IProductService ProductService
        {
            get { return _productService; }
        }

        protected void AddNotificationMessageToViewBag()
        {
            var moveBet = _moveBetService.GetMoveBet();

            if (!moveBet.EstimatedTime.HasValue || DateTime.Now > moveBet.EstimatedTime)
            {
                return;
            }

            var message = ViewResources.NotificationMessage;
            var startDateString = moveBet.AvailableWinlostDate.ToString("MM/dd/yyyy");
            var endDateString = moveBet.EstimatedTime.Value.ToString("HH:mm:ss MM/dd/yyyy");
            ViewBag.NotificationMessage = string.Format(message, startDateString, endDateString);
        }
    }
}