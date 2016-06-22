// -----------------------------------------------------------------------
// <copyright file="IVoidbridgeBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IVoidbridgeBetSettingService
    {
        bool? GetStatus(int licenseeId, int custId);

        IResultInfo UpdateStatus(int licenseeId, int userId, int custId, string ip, bool status);
    }
}