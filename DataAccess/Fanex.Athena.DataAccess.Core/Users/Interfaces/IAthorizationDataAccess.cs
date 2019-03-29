// -----------------------------------------------------------------------
// <copyright file="IAthorizationDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Threading.Tasks;

namespace Fanex.Athena.DataAccess.Core.Users.Interfaces
{
    public interface IAthorizationDataAccess
    {
        Task<bool> IsCustomerBelonging(int licenseeId, int custId, long transid);
    }
}