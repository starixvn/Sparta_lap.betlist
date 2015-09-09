// -----------------------------------------------------------------------
// <copyright file="MoveBetService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Configuration
{
    using Fanex.Athena.DataAccess.Implement.Configuration;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Service.Implement.Common;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Logging;

    public class MoveBetService : IMoveBetService
    {
        #region Constants and Fields
        
        private const int CacheTimeoutSeconds = 60; // 1 minutes.

        private readonly MoveBetDataAccess _moveBetDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BalanceUpService" /> class.
        /// </summary>
        public MoveBetService()
        {
            this._moveBetDataAccess = new MoveBetDataAccess();
        }

        #endregion Constructors and Destructors

        #region Public Properties

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
                    var moveBet = CacheService.Get<MoveBet>(CacheKey.MemorycacheCommonMoveBet);

                    if (moveBet != null)
                    {
                        return moveBet;
                    }

                    moveBet = this._moveBetDataAccess.GetMoveBet();
                    CacheService.Set(CacheKey.MemorycacheCommonMoveBet, moveBet, CacheTimeoutSeconds);
                    return moveBet;
                }
                catch (System.Exception ex)
                {
                    LogService.Error(ex.Message, ex);
                }

                return new MoveBet();
        }

        #endregion Public Properties
    }
}