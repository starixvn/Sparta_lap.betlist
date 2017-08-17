// -----------------------------------------------------------------------
// <copyright file="IDcsBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.DTO;

    public interface IDcsBetSettingDataAccess
    {
        bool? GetStatus(int custId, int productId);

        IResultInfo UpdateStatus(int userId, int custId, string ip, bool status, int productId);

        IList<BetSetting> GetBetSetting(int custId, int productId);

        IResultInfo UpdateBetSetting(int custId, int userId, string ip, string xmlData);
    }
}