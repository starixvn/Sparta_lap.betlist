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
    using Utility;
    using Utility.Interfaces;

    public class MoveBetService : IMoveBetService
    {
        private const string DefaultLanguage = "en-US";

        private const int CacheTimeoutSeconds = 60;

        private readonly ICoreLogger _logger;

        private readonly IMoveBetDataAccess _moveBetDataAccess;

        private readonly IMemoryCacheService _cache;

        public MoveBetService()
        {
            _logger = new CoreLogger();
            _moveBetDataAccess = new MoveBetDataAccess();
            _cache = new MemoryCacheService();
        }

        public MoveBetService(ICoreLogger logger, IMoveBetDataAccess moveBetDataAccess, IMemoryCacheService cache)
        {
            _logger = logger;
            _moveBetDataAccess = moveBetDataAccess;
            _cache = cache;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "Reviewed. Suppression is OK here.")]
        public string GetMoveBetNotification(string language)
        {
            try
            {
                var moveBet = _cache.Get<MoveBetNotification>(CacheKey.MemorycacheCommonMoveBet);
                if (moveBet == null)
                {
                    moveBet = this._moveBetDataAccess.GetMoveBetNotification();
                    _cache.Set(CacheKey.MemorycacheCommonMoveBet, moveBet, CacheTimeoutSeconds);
                }

                return GetMessage(moveBet, language);
            }
            catch (Exception exception)
            {
                _logger.Error(exception);
            }

            return string.Empty;
        }

        private static string GetMessage(MoveBetNotification moveBet, string language)
        {
            if (moveBet.ContentByLanguage.ContainsKey(language))
            {
                return moveBet.ContentByLanguage[language];
            }

            if (moveBet.ContentByLanguage.ContainsKey(DefaultLanguage))
            {
                return moveBet.ContentByLanguage[DefaultLanguage];
            }

            return string.Empty;
        }
    }
}