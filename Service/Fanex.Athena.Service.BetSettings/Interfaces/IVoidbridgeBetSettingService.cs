// -----------------------------------------------------------------------
// <copyright file="IVoidbridgeBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IVoidbridgeBetSettingService
    {
        bool? GetStatus(int custId);

        IResultInfo UpdateStatus(int userId, int custId, string ip, bool status);
    }
}