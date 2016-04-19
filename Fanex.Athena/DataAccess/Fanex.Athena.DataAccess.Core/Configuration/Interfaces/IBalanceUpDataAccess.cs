// -----------------------------------------------------------------------
// <copyright file="IBalanceUpDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Configuration.Interfaces
{
    using Fanex.Athena.Models.DTO.Configuration;

    public interface IBalanceUpDataAccess
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate",
            Justification = "Reviewed. Suppression is OK here.")]
        BalanceUp GetBalanceUpDay();
    }
}