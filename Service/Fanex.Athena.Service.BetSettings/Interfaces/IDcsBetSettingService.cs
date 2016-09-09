// -----------------------------------------------------------------------
// <copyright file="IDcsBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IDcsBetSettingService
    {
        bool? GetStatus(int licenseeId, int custId, int productId);

        IResultInfo UpdateStatus(int licenseeId, int userId, int custId, string ip, bool status, int productId);
    }
}