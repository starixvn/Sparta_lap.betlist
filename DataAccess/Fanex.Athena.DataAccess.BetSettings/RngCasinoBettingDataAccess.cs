// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using Core;
    using Extension.Utility;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Interfaces;
    using Model.BetSettings.MetaModel;
    using Session.Interfaces;

    public class RngCasinoBetSettingDataAccess : BaseDataAccess, IRngCasinoBetSettingDataAccess
    {
        private const int RngCasinoSportId = 251;

        private readonly ICoreCasinoBetSettingDataAccess _coreCasinoBetSettingDataAccess;

        public RngCasinoBetSettingDataAccess(
            IAppSettings appSetting,
            ISessionStorage sessionStorage,
            ICoreCasinoBetSettingDataAccess coreCasinoBetSettingDataAccess)
            : base(appSetting, sessionStorage)
        {
            _coreCasinoBetSettingDataAccess = coreCasinoBetSettingDataAccess;
        }

        public IResultInfo UpdateStatus(CoreCasinoStatusMetaModel metaModel)
        {
            return _coreCasinoBetSettingDataAccess.UpdateCasinoStatus(metaModel, RngCasinoSportId);
        }
    }
}