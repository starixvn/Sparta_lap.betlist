namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface ICoreCasinoBetSettingDataAccess
    {
        IResultInfo UpdateCasinoStatus(int userId, int custId, int sportid, int status, string language, string ip);

        void WriteLogUpdatingAction(int userId, int custId, string storeProcedureName, string trackingValue1, string trackingValue2);
    }
}