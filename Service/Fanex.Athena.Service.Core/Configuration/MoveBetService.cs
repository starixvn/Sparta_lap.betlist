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
        private const string DefaultLanguage = "en-US";

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

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public string GetMoveBetNotification(string language)
        {
            var result = string.Empty;
            try
            {
                var moveBet = CacheService.Get<MoveBetNotification>(CacheKey.MemorycacheCommonMoveBet);
                if (moveBet == null)
                {
                    moveBet = this._moveBetDataAccess.GetMoveBetNotification();
                    CacheService.Set(CacheKey.MemorycacheCommonMoveBet, moveBet, CacheTimeoutSeconds);
                }

                if (moveBet.ContentByLanguage.ContainsKey(language))
                {
                    result = moveBet.ContentByLanguage[language];
                }
                else if (moveBet.ContentByLanguage.ContainsKey(DefaultLanguage))
                {
                    result = moveBet.ContentByLanguage[DefaultLanguage];
                }
            }
            catch (Exception exception)
            {
                LogHelper.WriteError(exception);
            }

            return result;
        }
    }
}