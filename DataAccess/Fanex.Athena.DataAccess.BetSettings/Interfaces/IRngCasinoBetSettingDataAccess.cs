// -----------------------------------------------------------------------
// <copyright file="IRngCasinoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IRngCasinoBetSettingDataAccess
    {
        IResultInfo UpdateStatus(int userId, int custId, int status, string language, string ip);
    }
}