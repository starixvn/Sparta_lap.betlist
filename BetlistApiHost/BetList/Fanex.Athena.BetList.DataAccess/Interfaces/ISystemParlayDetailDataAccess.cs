using System.Threading.Tasks;
using Fanex.Athena.BetList.Model.MetaModel;
using Fanex.BetList.Core.Entities;

namespace Fanex.Athena.BetList.DataAccess.Interfaces
{
    public interface ISystemParlayDetailDataAccess
    {
        Task<SystemParlayData> GetData(SystemParlayDetailMetaModel metaModel);
    }
}