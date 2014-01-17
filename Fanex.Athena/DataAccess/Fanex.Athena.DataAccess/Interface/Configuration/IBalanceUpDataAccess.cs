// -----------------------------------------------------------------------
// <copyright file="IBalanceUpDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Configuration
{
    using Fanex.Athena.Models.DTO.Configuration;

    public interface IBalanceUpDataAccess
    {
        BalanceUp GetBalanceUpDay();
    }
}