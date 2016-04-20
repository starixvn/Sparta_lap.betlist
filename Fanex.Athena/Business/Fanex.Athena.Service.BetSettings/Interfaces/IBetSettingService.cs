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
    using Fanex.Athena.Models.ViewModel.BetSettings;

    public interface IBetSettingService
    {
        BetSettingViewModel GetSportBookViewModel(int custId);

        IResultInfo UpdateSportsBookBetSetting(CommonBetSetting setting, IDictionary<string, object> hashParam);

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Reviewed")]
        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled);
    }
}