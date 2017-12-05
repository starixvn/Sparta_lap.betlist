// -----------------------------------------------------------------------
// <copyright file="IRngCasinoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.MetaModel;

    public interface IRngCasinoBetSettingDataAccess
    {
        IResultInfo UpdateStatus(CoreCasinoStatusMetaModel metaModel);
    }
}