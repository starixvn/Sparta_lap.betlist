namespace Fanex.Athena.Controllers.Report
{
    using System;
    using Fanex.Athena.Service.Interface.Configuration;
    using Models.ViewModel;
    using Service.Implement.Common;
    using Service.Implement.Configuration;
    using Service.Interface.Common;

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

        protected IMoveBetService MoveBetService
        {
            get { return _moveBetService; }
        }

        protected void AddNotificationMessageToViewBag()
        {
            var moveBet = MoveBetService.GetMoveBet();

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
