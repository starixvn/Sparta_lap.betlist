namespace Fanex.Athena.DataAccess.BetSettings.Interfaces
{
    using Fanex.Athena.Model.Core.DTO.Common;
    using Model.BetSettings.MetaModel;

    public interface ICoreCasinoBetSettingDataAccess
    {
        IResultInfo UpdateCasinoStatus(CoreCasinoStatusMetaModel metaModel, int sportId);

        void WriteLogUpdatingAction(int userId, int custId, string storeProcedureName, string trackingValue1, string trackingValue2);
    }
}