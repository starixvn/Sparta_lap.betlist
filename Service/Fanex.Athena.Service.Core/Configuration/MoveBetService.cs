// -----------------------------------------------------------------------
// <copyright file="MoveBetService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Configuration
{
    using System;
    using DataAccess.Core.Configuration.Interfaces;
    using Extension.Utility.Interfaces;
    using Fanex.Athena.DataAccess.Core.Configuration;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Core.Helpers;

    public class MoveBetService : IMoveBetService
    {
        private const string DefaultLanguage = "en-US";

        private const int CacheTimeoutSeconds = 60; // 1 minutes.

        private readonly IMoveBetDataAccess _moveBetDataAccess;

        private readonly IMemoryCacheService _cache;

        public MoveBetService()
        {
            _moveBetDataAccess = new MoveBetDataAccess();
            _cache = new MemoryCacheService();
        }

        public MoveBetService(IMoveBetDataAccess moveBetDataAccess)
        {
            this._moveBetDataAccess = moveBetDataAccess;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public string GetMoveBetNotification(string language)
        {
            var result = string.Empty;
            try
            {
                var moveBet = _cache.Get<MoveBetNotification>(CacheKey.MemorycacheCommonMoveBet);
                if (moveBet == null)
                {
                    moveBet = this._moveBetDataAccess.GetMoveBetNotification();
                    _cache.Set(CacheKey.MemorycacheCommonMoveBet, moveBet, CacheTimeoutSeconds);
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