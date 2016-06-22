// -----------------------------------------------------------------------
// <copyright file="IThirdPartyBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IThirdPartyBetSettingDataAccess
    {
        ThirdPartyBetSetting GetBetSetting(int custId, int productid);

        IResultInfo UpdateStatus(int productId, int userId, int custId, string language, string ip, bool isEnabled);
    }
}