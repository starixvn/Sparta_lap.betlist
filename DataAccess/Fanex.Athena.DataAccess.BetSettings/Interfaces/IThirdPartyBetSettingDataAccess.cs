// -----------------------------------------------------------------------
// <copyright file="IThirdPartyBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IThirdPartyBetSettingDataAccess
    {
        Task<ThirdPartyBetSetting> GetBetSettingAsync(int custId, int productid);

        Task<IResultInfo> UpdateStatusAsync(int productId, int userId, int custId, string language, string ip, bool isEnabled);
    }
}