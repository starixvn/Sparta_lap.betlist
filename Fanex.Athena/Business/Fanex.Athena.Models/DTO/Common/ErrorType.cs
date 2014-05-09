// -----------------------------------------------------------------------
// <copyright file="ErrorType.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Common
{
    public enum ErrorType
    {
        Success = 0,
        Unknown = -1,
        AccessDenied = 1,
        AccountClosed = 2,
        KickedOut = 3,
        UnderMaintenance = 4,
        SessionOverride
    }
}