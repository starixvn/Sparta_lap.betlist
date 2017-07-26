namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.DTO;

    public interface IDcsBetSettingService
    {
        bool? GetStatus(int licenseeId, int custId, int productId);

        IResultInfo UpdateStatus(int licenseeId, int userId, int custId, string ip, bool status, int productId);

        IList<BetSetting> GetBetSettings(int licenseeId, int custId, int productId);

        IResultInfo UpdateBetSetting(int licenseeId, int custId, int userId, string ip, IEnumerable<DcsBetSetting> betSettings);
    }
}