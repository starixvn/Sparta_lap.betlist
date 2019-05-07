// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using System.Threading.Tasks;
    using Core;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Interfaces;
    using Model.BetSettings.MetaModel;

    public class RngCasinoBetSettingDataAccess : BaseDataAccess, IRngCasinoBetSettingDataAccess
    {
        private const int RngCasinoSportId = 251;

        private readonly ICoreCasinoBetSettingDataAccess _coreCasinoBetSettingDataAccess;

        public RngCasinoBetSettingDataAccess(
            ICoreCasinoBetSettingDataAccess coreCasinoBetSettingDataAccess)
        {
            _coreCasinoBetSettingDataAccess = coreCasinoBetSettingDataAccess;
        }

        public Task<IResultInfo> UpdateStatusAsync(CoreCasinoStatusMetaModel metaModel)
        {
            return _coreCasinoBetSettingDataAccess.UpdateCasinoStatusAsync(metaModel, RngCasinoSportId);
        }
    }
}