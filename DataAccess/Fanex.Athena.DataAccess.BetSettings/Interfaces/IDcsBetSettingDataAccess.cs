// -----------------------------------------------------------------------
// <copyright file="IDcsBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IDcsBetSettingDataAccess
    {
        bool? GetStatus(int custId, int productId);

        IResultInfo UpdateStatus(int userId, int custId, string ip, bool status, int productId);
    }
}