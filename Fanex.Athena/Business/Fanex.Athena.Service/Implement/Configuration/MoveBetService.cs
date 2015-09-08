// -----------------------------------------------------------------------
// <copyright file="MoveBetService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Configuration
{
    using System;
    using Fanex.Athena.DataAccess.Implement.Configuration;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Logging;

    public class MoveBetService : IMoveBetService
    {
        #region Constants and Fields

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
            MoveBet moveBet = null;
            try
            {
                moveBet = this._moveBetDataAccess.GetMoveBet();
            }
            catch (Exception exception)
            {
                LogService.Error(exception.Message, exception);
            }

            return moveBet;
        }

        #endregion Public Properties
    }
}