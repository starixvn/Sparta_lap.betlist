using System.Threading.Tasks;
using Fanex.Athena.Model.Casino.MetaModel;
using Fanex.Athena.Model.Customer.MetaModel;

namespace Fanex.Athena.Service.Casino.Interfaces
{
    public interface ICasinoCustomerService
    {
        bool UpdateClosed(CustomerStatusMetaModel metaModel);

        bool UpdateSuspend(CustomerStatusMetaModel metaModel);

        Task<bool> UpdateCommission(RNGCasinoUpdateMemberComMetaModel metaModel);
    }
}