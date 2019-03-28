// -----------------------------------------------------------------------
// <copyright file="ISportsbookBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.ViewModel;

    public interface ISportsbookBetSettingDataAccess
    {
        Task<BetSettingViewModel> GetBetSettingAsync(int custId);

        Task<IResultInfo> UpdateBetSettingAsync(CommonBetSetting data, IDictionary<string, object> setting);
    }
}