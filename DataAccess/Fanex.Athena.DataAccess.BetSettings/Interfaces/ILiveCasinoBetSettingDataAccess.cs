// -----------------------------------------------------------------------
// <copyright file="ILiveCasinoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.BetSettings.DTO;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.MetaModel;

    public interface ILiveCasinoBetSettingDataAccess
    {
        IList<LiveCasinoBetSetting> GetBetSetting(int custId, string language);

        IResultInfo UpdateBetSetting(int userId, int custId, LiveCasinoBetSetting param, string ip, string language);

        IResultInfo UpdateStatus(CoreCasinoStatusMetaModel metaModel);
    }
}