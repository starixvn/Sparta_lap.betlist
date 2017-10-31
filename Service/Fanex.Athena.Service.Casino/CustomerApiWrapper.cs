namespace Fanex.Athena.Service.Casino
{
    using CasinoLibrary.SBAgent;
    using Interfaces;

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
    }
}