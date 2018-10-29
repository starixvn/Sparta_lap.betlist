// -----------------------------------------------------------------------
// <copyright file="ISportsbookBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.ViewModel;

    public interface ISportsbookBetSettingDataAccess
    {
        BetSettingViewModel GetBetSetting(int custId);

        IResultInfo UpdateBetSetting(CommonBetSetting data, IDictionary<string, object> setting);
    }
}