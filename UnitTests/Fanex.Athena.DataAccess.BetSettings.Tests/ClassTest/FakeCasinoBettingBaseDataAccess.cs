using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.Model.Core.DTO.Common;

namespace Fanex.Athena.DataAccess.BetSettings.Tests.ClassTest
{
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
