// -----------------------------------------------------------------------
// <copyright file="IRngCasinoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.MetaModel;

    public interface IRngCasinoBetSettingDataAccess
    {
        Task<IResultInfo> UpdateStatusAsync(CoreCasinoStatusMetaModel metaModel);
    }
}