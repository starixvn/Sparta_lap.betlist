using Fanex.Athena.Model.BetSettings.MetaModel;
using Fanex.Athena.Model.Casino.MetaModel;
using Fanex.Athena.Model.Customer.MetaModel;
using System.Data;

namespace Fanex.Athena.Service.Casino.Wrappers
{
    public interface ICustomerApiWrapper
    {
        bool UpdateCustEnableCasino(BetSettingCasinoStatusMetaModel metaModel);

        bool UpdateCustSuspend(CustomerStatusMetaModel metaModele);

        bool UpdateCustClosed(CustomerStatusMetaModel metaModele);

        DataTable UpdateCustGameTypeStakeLevel(DataTable sourceTable);

        DataTable GetCustStakeLevel(int ibcCustId, int gameTypeId);

        bool UpdateComm(RNGCasinoUpdateMemberComMetaModel metaModel);
    }
}