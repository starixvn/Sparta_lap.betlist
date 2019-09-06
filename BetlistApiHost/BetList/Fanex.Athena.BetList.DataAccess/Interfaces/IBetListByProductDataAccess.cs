using System.Threading.Tasks;

using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;

namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    public interface IBetListByProductDataAccess
    {
        Task<TicketResult> QueryAsync(BetListByProductMetaModel metaModel);

    }
}
