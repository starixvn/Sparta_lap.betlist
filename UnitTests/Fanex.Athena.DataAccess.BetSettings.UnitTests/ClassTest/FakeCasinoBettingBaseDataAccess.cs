namespace Fanex.Athena.DataAccess.BetSettings.UnitTests.ClassTest
{
    using Fanex.Athena.Model.Core.DTO.Common;

    public class FakeCasinoBettingBaseDataAccess : CasinoBettingBaseDataAccess
    {
        public new IResultInfo UpdateCasinoStatus(int userId, int custId, int sportid, int status, string language, string ip)
        {
            return base.UpdateCasinoStatus(userId, custId, sportid, status, language, ip);
        }

        public new void WriteLogUpdatingAction(int userId, int custId, string storeProcedureName, string trackingValue1, string trackingValue2)
        {
            base.WriteLogUpdatingAction(userId, custId, storeProcedureName, trackingValue1, trackingValue2);
        }
    }
}