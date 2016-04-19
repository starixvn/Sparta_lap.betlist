// -----------------------------------------------------------------------
// <copyright file="IMoveBetDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>

namespace Fanex.Athena.DataAccess.Core.Configuration.Interfaces
{
    using Fanex.Athena.Models.DTO.Configuration;

    public interface IMoveBetDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        MoveBet GetMoveBet();
    }
}