// -----------------------------------------------------------------------
// <copyright file="IDcsBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.DTO;

    public interface IDcsBetSettingDataAccess
    {
        Task<bool?> GetStatusAsync(int custId, int productId);

        Task<IResultInfo> UpdateStatusAsync(int userId, int custId, string ip, bool status, int productId);

        Task<IList<BetSetting>> GetBetSettingAsync(int custId, int productId);

        Task<IResultInfo> UpdateBetSettingAsync(int custId, int userId, string ip, string xmlData);
    }
}