namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.MetaModel;

    public interface ICoreCasinoBetSettingDataAccess
    {
        Task<IResultInfo> UpdateCasinoStatusAsync(CoreCasinoStatusMetaModel metaModel, int sportId);
    }
}