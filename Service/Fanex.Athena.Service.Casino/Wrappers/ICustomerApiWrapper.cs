using System.Data;

namespace Fanex.Athena.Service.Casino.Wrappers
{
    public interface ICustomerApiWrapper
    {
        bool UpdateCustEnableCasino(int ibcCustId, bool isEnableCasino, bool isUpdateDownline, string ip, int modifyBy, string modifyUserName);

        bool UpdateCustSuspend(int ibcCustID, bool isSuspend, string ip, int modifyBy, string modifyUserName);

        bool UpdateCustClosed(int ibcCustID, bool isClosed, string ip, int modifyBy, string modifyUserName);

        DataTable UpdateCustGameTypeStakeLevel(DataTable sourceTable);

        DataTable GetCustStakeLevel(int ibcCustId, int gameTypeId);

        bool UpdateComm(string[] ibcCustID, decimal memberRollingRate, string ip, int modifyBy, string modifyUserName);
    }
}