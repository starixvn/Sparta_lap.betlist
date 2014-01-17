// -----------------------------------------------------------------------
// <copyright file="IAthorizationDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.Users
{
    public interface IAthorizationDataAccess
    {
        bool IsCustomerBelonging(int licenseeId, int custId, long transid);
    }
}