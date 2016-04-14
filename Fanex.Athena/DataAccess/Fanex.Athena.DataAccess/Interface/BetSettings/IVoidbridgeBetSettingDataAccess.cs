// -----------------------------------------------------------------------
// <copyright file="IVoidbridgeBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Interface.BetSettings
{
    using Fanex.Athena.Models.DTO.Common;

    public interface IVoidbridgeBetSettingDataAccess
    {
        bool? GetStatus(int custId);

        IResultInfo UpdateStatus(int userId, int custId, string ip, bool status);
    }
}