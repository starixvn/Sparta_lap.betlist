namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.DTO;

    public interface IDcsBetSettingService
    {
        Task<bool?> GetStatusAsync(int licenseeId, int custId, int productId);

        Task<IResultInfo> UpdateStatusAsync(int licenseeId, int userId, int custId, string ip, bool status, int productId);

        Task<IList<BetSetting>> GetBetSettingsAsync(int licenseeId, int custId, int productId);

        Task<IResultInfo> UpdateBetSettingAsync(int licenseeId, int custId, int userId, string ip, IEnumerable<DcsBetSetting> betSettings);
    }
}