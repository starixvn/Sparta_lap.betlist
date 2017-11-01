namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System.Data;
    using CasinoLibrary.SBAgent;

    public class CustomerApiWrapper : ICustomerApiWrapper
    {
        private readonly Customer _customerApi;

        public CustomerApiWrapper()
        {
            _customerApi = new Customer();
        }

        public bool UpdateCustEnableCasino(int ibcCustId, bool isEnableCasino, bool isUpdateDownline, string ip, int modifyBy, string modifyUserName, int system)
        {
            return _customerApi.UpdateCustEnableCasino(ibcCustId, isEnableCasino, isUpdateDownline, ip, modifyBy, modifyUserName, system);
        }

        public bool UpdateCustSuspend(int ibcCustID, bool isSuspend, string ip, int modifyBy, string modifyUserName, int system)
        {
            return _customerApi.UpdateCustSuspend(ibcCustID, isSuspend, ip, modifyBy, modifyUserName, system);
        }

        public bool UpdateCustClosed(int ibcCustID, bool isClosed, string ip, int modifyBy, string modifyUserName, int system)
        {
            return _customerApi.UpdateCustClosed(ibcCustID, isClosed, ip, modifyBy, modifyUserName, system);
        }

        public DataTable UpdateCustGameTypeStakeLevel(DataTable sourceTable)
        {
            return _customerApi.UpdateCustGameTypeStakeLevel(sourceTable);
        }

        public DataTable GetCustStakeLevel(int ibcCustId, int gameTypeId)
        {
            return _customerApi.GetCustStakeLevel(ibcCustId, gameTypeId);
        }

        public bool UpdateComm(string[] ibcCustID, decimal memberRollingRate, string ip, int modifyBy, string modifyUserName, int system)
        {
            return _customerApi.UpdateComm(ibcCustID, memberRollingRate, ip, modifyBy, modifyUserName, system);
        }
    }
}