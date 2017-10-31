namespace Fanex.Athena.Service.Casino
{
    using System.Data;
    using CasinoLibrary.SBAgent;
    using Interfaces;

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

        public DataTable GetGameType(int gameKind)
        {
            return _basicInfoApi.GetGameType(gameKind);
        }
    }
}