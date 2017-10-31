namespace Fanex.Athena.Service.Casino.Interfaces
{
    public interface ICustomerApiWrapper
    {
        bool UpdateCustEnableCasino(int ibcCustId, bool isEnableCasino, bool isUpdateDownline, string ip, int modifyBy, string modifyUserName, int system);
    }
}