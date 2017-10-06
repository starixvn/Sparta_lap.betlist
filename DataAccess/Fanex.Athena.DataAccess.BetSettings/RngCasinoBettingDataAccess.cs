// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using Core;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Interfaces;

    public class RngCasinoBetSettingDataAccess : BaseDataAccess, IRngCasinoBetSettingDataAccess
    {
        private const int RngCasinoSportId = 251;

        private readonly ICoreCasinoBetSettingDataAccess _coreCasinoBetSettingDataAccess;

        public RngCasinoBetSettingDataAccess()
        {
            _coreCasinoBetSettingDataAccess = new CoreCasinoBetSettingDataAccess();
        }

        public RngCasinoBetSettingDataAccess(ICoreCasinoBetSettingDataAccess coreCasinoBetSettingDataAccess)
        {
            _coreCasinoBetSettingDataAccess = coreCasinoBetSettingDataAccess;
        }

        public IResultInfo UpdateStatus(int userId, int custId, int status, string language, string ip)
        {
            return _coreCasinoBetSettingDataAccess.UpdateCasinoStatus(userId, custId, RngCasinoSportId, status, language, ip);
        }
    }
}