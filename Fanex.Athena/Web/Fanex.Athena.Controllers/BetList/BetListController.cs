// -----------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel">
// Report controller
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Controllers.BetList
{
    #region Using

    using System;
    using System.Linq;
    using System.Web.Mvc;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Service.Implement.BetList;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.BetList;
    using Fanex.Athena.Service.Interface.Common;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;

    #endregion Using

    /// <summary>
    ///     TODO: Update summary.
    /// </summary>
    public partial class BetListController : ReadonlyBaseController
    {
        #region Constant

        /// <summary>
        /// Max row count for excel file.
        /// </summary>
        public const int PageSizeExcel = 63000;

        #endregion Constant

        #region Fields

        /// <summary>
        /// The list contains all bet type of racing.
        /// </summary>
        private static readonly int[] RacingBetType = { 31, 32, 33, 41, 42, 43 };

        /// <summary>
        /// Balance Up Service.
        /// </summary>
        private readonly IBalanceUpService _balanceUpService;

        /// <summary>
        /// Product Service.
        /// </summary>
        private readonly IProductService _productService;

        /// <summary>
        /// Bet List Service.
        /// </summary>
        private readonly IBetListService _betListService;

        /// <summary>
        /// Ticket Helper.
        /// </summary>
        private readonly ITicketHelper _ticketHelper;

        #endregion Fields

        #region constructor

        public BetListController()
        {
            this._productService = new ProductService();
            this._betListService = new BetListService();
            this._ticketHelper = new TicketHelper();
            this._balanceUpService = new BalanceUpService();
        }

        #endregion constructor

        #region Utility

        /// <summary>
        /// Set the Licensee Id, Balance Up Date.
        /// Remove the UTC part of DateTime value.
        /// </summary>
        /// <param name="metaModel">The metal model.</param>
        /// <returns>The bet list metal model.</returns>
        public BetListMetaModel CreateDefaultMetaModel(BetListMetaModel metaModel = null)
        {
            metaModel = metaModel ?? new BetListMetaModel();
            metaModel.LicenseeId = SessionService.LicenseeSession.LicenseeId;
            metaModel.UserId = SessionService.UserSession.UserId;

            var balanceUpInfo = this._balanceUpService.BalanceUpInfo;
            metaModel.BalanceUpDay = DateTime.SpecifyKind(balanceUpInfo.BalanceUpDay.Date, DateTimeKind.Utc);
            metaModel.MoveBetsDay = DateTime.SpecifyKind(balanceUpInfo.MoveBetsDay.Date, DateTimeKind.Utc);
            metaModel.FromDate = DateTime.SpecifyKind(metaModel.FromDate.Date, DateTimeKind.Utc);
            metaModel.ToDate = DateTime.SpecifyKind(metaModel.ToDate.Date, DateTimeKind.Utc);
            metaModel.Date = DateTime.SpecifyKind(metaModel.Date.Date, DateTimeKind.Utc);

            return metaModel;
        }

        #endregion Utility

        #region Override

        /// <summary>
        /// Override the max JSON Length.
        /// </summary>
        /// <param name="data">The data object.</param>
        /// <param name="contentType">The content type.</param>
        /// <param name="contentEncoding">The encoding.</param>
        /// <param name="behavior">The JSON request behavior.</param>
        /// <returns>The JSON result.</returns>
        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult
            {
                Data = data,
                ContentType = contentType,
                ContentEncoding = contentEncoding,
                JsonRequestBehavior = behavior,
                MaxJsonLength = int.MaxValue
            };
        }

        #endregion Override

        /// <summary>
        /// Check if the bet type is racing bet type.
        /// </summary>
        /// <param name="betType">The bet type.</param>
        /// <returns>True or false.</returns>
        [NonAction]
        private static bool IsRacing(int betType)
        {
            return RacingBetType.Contains(betType);
        }
    }
}