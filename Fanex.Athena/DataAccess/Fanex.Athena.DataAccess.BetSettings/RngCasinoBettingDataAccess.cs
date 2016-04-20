// -----------------------------------------------------------------------
// <copyright file="RngCasinoBetSettingDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.BetSettings
{
    using Fanex.Athena.Models.DTO.Common;
    using Interfaces;

    public class RngCasinoBetSettingDataAccess : CasinoBettingBaseDataAccess, IRngCasinoBetSettingDataAccess
    {
        private const int RngCasinoSportId = 251;

        public IResultInfo UpdateStatus(int userId, int custId, int status, string language, string ip)
        {
            return UpdateCasinoStatus(userId, custId, RngCasinoSportId, status, ip, language);
        }
    }
}