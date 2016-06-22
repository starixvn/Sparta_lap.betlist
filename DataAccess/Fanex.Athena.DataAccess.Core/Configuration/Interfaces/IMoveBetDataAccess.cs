// -----------------------------------------------------------------------
// <copyright file="IMoveBetDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>

namespace Fanex.Athena.DataAccess.Core.Configuration.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Configuration;

    public interface IMoveBetDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        MoveBet GetMoveBet();
    }
}