using System.Data;

namespace Fanex.Athena.Service.Casino.Interfaces
{
    public interface ICustomerApiWrapper
    {
        bool UpdateCustEnableCasino(int ibcCustId, bool isEnableCasino, bool isUpdateDownline, string ip, int modifyBy, string modifyUserName, int system);

        bool UpdateCustSuspend(int ibcCustID, bool isSuspend, string ip, int modifyBy, string modifyUserName, int system);

        bool UpdateCustClosed(int ibcCustID, bool isClosed, string ip, int modifyBy, string modifyUserName, int system);

        DataTable UpdateCustGameTypeStakeLevel(DataTable sourceTable);

        DataTable GetCustStakeLevel(int ibcCustID, int gameTypeId);
    }
}