// -----------------------------------------------------------------------
// <copyright file="IThirdPartyBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;

    public interface IThirdPartyBetSettingDataAccess
    {
        ThirdPartyBetSetting GetBetSetting(int custId, int productid);

        IResultInfo UpdateStatus(int productId, int userId, int custId, string language, string ip, bool isEnabled);
    }
}