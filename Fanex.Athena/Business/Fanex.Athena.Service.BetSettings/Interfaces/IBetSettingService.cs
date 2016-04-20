// -----------------------------------------------------------------------
// <copyright file="IBetSettingService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.Models.DTO.BetSettings;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.MetaModel.Casino;
    using Fanex.Athena.Models.ViewModel.BetSettings;

    /// <summary>
    ///     IBet setting service.
    /// </summary>
    public interface IBetSettingService
    {
        BetSettingViewModel GetKenoViewModel(int custId);

        BetSettingViewModel GetSportBookViewModel(int custId);

        BetSettingViewModel GetVirtualSportsViewModel(int custId);

        IResultInfo UpdateKenoBetSetting(int custId, int userId, string language, string ip, KenoBetSetting setting);

        IResultInfo UpdateSportsBookBetSetting(CommonBetSetting setting, IDictionary<string, object> hashParam);

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Reviewed")]
        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled);

        IResultInfo UpdateVirtualSportsBetSetting(int custId, int userId, string language, string ip, IList<VSBetSetting> betSettings);
    }
}