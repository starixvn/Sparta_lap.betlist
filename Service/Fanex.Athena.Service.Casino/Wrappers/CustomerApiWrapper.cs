namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System;
    using System.Data;
    using CasinoLibrary.Model.UserException;
    using CasinoLibrary.SBAgent;

    public class CustomerApiWrapper : ICustomerApiWrapper
    {
        private const int SystemId = 4;
        private readonly Customer _customerApi;

        public CustomerApiWrapper()
        {
            _customerApi = new Customer();
        }

        public bool UpdateCustEnableCasino(int ibcCustId, bool isEnableCasino, bool isUpdateDownline, string ip, int modifyBy, string modifyUserName)
        {
            try
            {
                return _customerApi.UpdateCustEnableCasino(ibcCustId, isEnableCasino, isUpdateDownline, ip, modifyBy, modifyUserName, SystemId);
            }
            catch (Exception userException)
            {
                if (userException is ErrorException)
                {
                    Logging.Logger.Log.Error(userException.Message, userException);
                }
                else
                {
                    throw;
                }
            }
            return false;
        }

        public bool UpdateCustSuspend(int ibcCustID, bool isSuspend, string ip, int modifyBy, string modifyUserName)
        {
            try
            {
                return _customerApi.UpdateCustSuspend(ibcCustID, isSuspend, ip, modifyBy, modifyUserName, SystemId);
            }
            catch (Exception userException)
            {
                if (userException is ErrorException)
                {
                    Logging.Logger.Log.Error(userException.Message, userException);
                }
                else
                {
                    throw;
                }
            }
            return false;
        }

        public bool UpdateCustClosed(int ibcCustID, bool isClosed, string ip, int modifyBy, string modifyUserName)
        {
            try
            {
                return _customerApi.UpdateCustClosed(ibcCustID, isClosed, ip, modifyBy, modifyUserName, SystemId);
            }
            catch (Exception userException)
            {
                if (userException is ErrorException)
                {
                    Logging.Logger.Log.Error(userException.Message, userException);
                }
                else
                {
                    throw;
                }
            }
            return false;
        }

        public DataTable UpdateCustGameTypeStakeLevel(DataTable sourceTable)
        {
            DataTable result = null;
            try
            {
                result = _customerApi.UpdateCustGameTypeStakeLevel(sourceTable);
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

        public DataTable GetCustStakeLevel(int ibcCustId, int gameTypeId)
        {
            return _customerApi.GetCustStakeLevel(ibcCustId, gameTypeId);
        }

        public bool UpdateComm(string[] ibcCustID, decimal memberRollingRate, string ip, int modifyBy, string modifyUserName)
        {
            try
            {
                return _customerApi.UpdateComm(ibcCustID, memberRollingRate, ip, modifyBy, modifyUserName, SystemId);
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
            return false;
        }

    }
}