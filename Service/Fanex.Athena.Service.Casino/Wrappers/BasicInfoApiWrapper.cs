namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System;
    using System.Data;
    using CasinoLibrary.Model.UserException;
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
                if (exception is ErrorException)
                {
                    Logging.Logger.Log.Error(exception.Message, exception);
                }
                else
                {
                    throw;
                }
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
                if (exception is ErrorException)
                {
                    Logging.Logger.Log.Error(exception.Message, exception);
                }
                else
                {
                    throw;
                }
            }
            return result;
        }
    }
}
