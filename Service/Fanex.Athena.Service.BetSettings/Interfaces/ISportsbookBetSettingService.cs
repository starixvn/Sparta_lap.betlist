// -----------------------------------------------------------------------
// <copyright file="ISportsbookBetSettingService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.BetSettings.ViewModel;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface ISportsbookBetSettingService
    {
        BetSettingViewModel GetViewModel(int licenseeId, int custId);

        IResultInfo UpdateBetSetting(CommonBetSetting setting, IDictionary<string, object> hashParam);

        [SuppressMessage("Microsoft.Design", "CA1045:DoNotPassTypesByReference", Justification = "Reviewed")]
        IResultInfo UpdateEsportStatus(CommonBetSetting setting, bool isDisabled);
    }
}