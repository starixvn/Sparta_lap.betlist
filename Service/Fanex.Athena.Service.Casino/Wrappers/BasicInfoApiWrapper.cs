namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System;
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
            DataTable result = null;
            try
            {
                result = _basicInfoApi.GetGameKind(casinoNo);
            }
            catch (Exception exception)
            {
                Logging.Logger.Log.Error(exception.Message, exception);
            }
            return result;
        }

        public DataTable GetGameType(int gameKind, int sbCustID, int sbExchangeID)
        {
            DataTable result = null;
            try
            {
                result = _basicInfoApi.GetGameType(gameKind, sbCustID, sbCustID);
            }
            catch (Exception exception)
            {
                Logging.Logger.Log.Error(exception.Message, exception);
            }
            return result;
        }
    }
}
