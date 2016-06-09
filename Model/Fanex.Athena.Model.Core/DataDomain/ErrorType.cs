// -----------------------------------------------------------------------
// <copyright file="ErrorType.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DataDomain
{
    public enum ErrorType
    {
        /// <summary>
        /// The success action.
        /// </summary>
        Success = 0,

        /// <summary>
        /// The unknown error.
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// The account is denied.
        /// </summary>
        AccessDenied = 1,

        /// <summary>
        /// The account is closed.
        /// </summary>
        AccountClosed = 2,

        /// <summary>
        /// The kick out action.
        /// </summary>
        KickedOut = 3,

        /// <summary>
        /// Under maintenance.
        /// </summary>
        UnderMaintenance = 4,

        /// <summary>
        /// Session override.
        /// </summary>
        SessionOverride
    }
}