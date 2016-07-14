// -----------------------------------------------------------------------
// <copyright file="IAthorizationDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Users.Interfaces
{
    public interface IAthorizationDataAccess
    {
        bool IsCustomerBelonging(int licenseeId, int custId, long transid);
    }
}