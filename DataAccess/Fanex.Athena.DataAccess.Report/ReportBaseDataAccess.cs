using System.Collections.Generic;
using Fanex.Athena.DataAccess.Core;
using Fanex.Athena.Extension.Utility;
using Fanex.Athena.Session;
using Fanex.Athena.Session.Interfaces;

namespace Fanex.Athena.DataAccess.Report
{
    public class ReportBaseDataAccess : BaseDataAccess
    {
        private readonly ISessionStorage _sessionStorage;

        private IDictionary<int, string> _currencyList;

        public ReportBaseDataAccess() : this(null, null)
        {
        }

        public ReportBaseDataAccess(IAppSettings appSetting, ISessionStorage sessionStorage)
            : base(appSetting)
        {
            _sessionStorage = sessionStorage ?? new SessionStorage();
        }

        protected string GetCurrencyName(int currencyId)
        {
            if (_currencyList == null)
            {
                _currencyList = _sessionStorage.UserSession.CurrencyList;
            }

            if (_currencyList.ContainsKey(currencyId))
            {
                return _currencyList[currencyId];
            }

            return currencyId.ToString();
        }
    }
}