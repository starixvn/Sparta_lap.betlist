namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System.Data;
    using CasinoLibrary.SBAgent;

    public class BasicInfoApiWrapper : IBasicInfoApiWrapper
    {
        private readonly BasicInfo _basicInfoApi;

        public BasicInfoApiWrapper()
        {
            _basicInfoApi = new BasicInfo();
        }

        public DataTable GetGameKind(string casinoNo)
        {
            return _basicInfoApi.GetGameKind(casinoNo);
        }

        public DataTable GetGameType(int gameKind, int sbCustID, int sbExchangeID)
        {
            return _basicInfoApi.GetGameType(gameKind, sbCustID, sbCustID);
        }
    }
}