using System.Collections.Generic;
using Fanex.Athena.BetList.Model.DTO;

namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    public interface IIPDataAccess
    {
        IList<IPRangeEntity> GetVNIPs();
    }
}
