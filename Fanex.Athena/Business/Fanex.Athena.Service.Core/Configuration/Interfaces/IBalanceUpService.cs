// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IBalanceUpService.cs" company="Nexcel">
// The i balance up service.
// </copyright>
// <summary>
//   The i balance up service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Configuration.Interfaces
{
    using Fanex.Athena.Models.DTO.Configuration;

    /// <summary>
    ///     The i balance up service.
    /// </summary>
    public interface IBalanceUpService
    {
        #region Public Properties

        BalanceUp BalanceUpInfo { get; }

        #endregion Public Properties
    }
}