// -----------------------------------------------------------------------
// <copyright file="MoveBetService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Configuration
{
    using System;
    using DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core.Configuration;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Core.Helpers;

    public class MoveBetService : IMoveBetService
    {
        private const int CacheTimeoutSeconds = 60; // 1 minutes.

        private readonly IMoveBetDataAccess _moveBetDataAccess;

        private readonly IAppSettings _appSettings;

        public MoveBetService()
        {
            this._moveBetDataAccess = new MoveBetDataAccess();
            this._appSettings = new AppSettings();
        }

        public MoveBetService(IAppSettings appSettings, IMoveBetDataAccess moveBetDataAccess)
        {
            this._moveBetDataAccess = moveBetDataAccess;
            this._appSettings = appSettings;
        }

        /// <summary>
        ///     Gets GetBalanceUp.
        /// </summary>
        /// <value>The balance up day.</value>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public MoveBet GetMoveBet()
        {
            try
            {
                var endDate = _appSettings.MoveBetDayEnd;
                if (endDate.HasValue)
                {
                    return new MoveBet()
                    {
                        AvailableWinlostDate = _appSettings.MoveBetDayStart.Value,
                        EstimatedTime = endDate
                    };
                }

                var moveBet = CacheService.Get<MoveBet>(CacheKey.MemorycacheCommonMoveBet);

                if (moveBet != null)
                {
                    return moveBet;
                }

                moveBet = this._moveBetDataAccess.GetMoveBet();
                CacheService.Set(CacheKey.MemorycacheCommonMoveBet, moveBet, CacheTimeoutSeconds);
                return moveBet;
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception);
            }

            return new MoveBet();
        }
    }
}