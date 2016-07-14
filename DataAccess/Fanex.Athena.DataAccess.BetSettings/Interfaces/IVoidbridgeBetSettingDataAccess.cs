// -----------------------------------------------------------------------
// <copyright file="IVoidbridgeBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IVoidbridgeBetSettingDataAccess
    {
        bool? GetStatus(int custId);

        IResultInfo UpdateStatus(int userId, int custId, string ip, bool status);
    }
}