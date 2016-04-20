// -----------------------------------------------------------------------
// <copyright file="ILiveCasinoBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using Fanex.Athena.Models.DTO.Common;

    public interface IRngCasinoBetSettingDataAccess
    {
        IResultInfo UpdateRngCasinoStatus(int userId, int custId, int status, string language, string ip);
    }
}