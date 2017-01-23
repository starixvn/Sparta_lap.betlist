// -----------------------------------------------------------------------
// <copyright file="IAthorizationDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Users.Interfaces
{
    public interface IAthorizationDataAccess
    {
        bool IsCustomerBelonging(int licenseeId, int custId, long transid);
    }
}