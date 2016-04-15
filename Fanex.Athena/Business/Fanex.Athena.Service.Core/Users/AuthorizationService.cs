// -----------------------------------------------------------------------
// <copyright file="AuthorizationService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Diagnostics;
    using System.Linq;
    using System.Web;
    using Fanex.Athena.DataAccess.Implement.Users;
    using Fanex.Athena.DataAccess.Interface.Users;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;

    /// <summary>
    /// Authorization service class.
    /// </summary>
    public class AuthorizationService : IAuthorizationService
    {
        #region AuthorizePath

        private readonly IAthorizationDataAccess _authorizationDataAccess = new AuthorizationDataAccess();
        private readonly IFunctionService _functionService = new FunctionService();
        private readonly ISessionService _sessionService = new SessionService();

        /// <summary>
        ///     Authorize a http request by checking it's parameter: customer id, transaction id.
        ///     Check if current user has permission on that url.
        ///     Check if current user has permission on customer id, transaction id.
        /// </summary>
        /// <param name="licenseeId">Licensee Id.</param>
        /// <param name="userFunctionList">List of functions which user has permission on.</param>
        /// <param name="custId">Customer Id.</param>
        /// <param name="transId">Transaction Id of bet ticker.</param>
        /// <param name="url">The path of requested page.</param>
        /// <returns>
        ///     If the current licensee can not action this url, return false.
        ///     If customer Id > 0 return true if the customer id is belonged to the licensee.
        ///     If transaction Id > 0 return true if the transaction id is belonged to a customer of the licensee.
        /// </returns>
        public bool AuthorizePath(int licenseeId, IList<int> userFunctionList, int custId, long transId, string url)
        {
            if (userFunctionList == null)
            {
                return false;
            }

            /* Check permission on the url*/
            var functionList = _functionService.Functions;
            var mappedUrl = functionList.FirstOrDefault(f => CanMapUrl(url, f.Url));

            /* Only authorize the function in function list*/
            var hasPermission = mappedUrl == null || userFunctionList.Any(f => f == mappedUrl.FunctionId);

            if (!hasPermission)
            {
                return false;
            }

            /*Check belonging*/
            if (custId != 0 || transId != 0)
            {
                return HasPermissionOnData(custId, transId);
            }

            return true;
        }

        /// <summary>
        ///     Authorize a http request by checking it's parameter: customer id, transaction id.
        ///     Check if current user has permission on that url.
        ///     Check if current user has permission on customer id and transaction id.
        /// </summary>
        /// <param name="licenseeId">Licensee Id.</param>
        /// <param name="userFunctionList">List of functions which user has permission on.</param>
        /// <param name="url">The path of requested page.</param>
        /// <returns>
        ///     If the current licensee can not action this url, return false.
        /// </returns>
        public bool AuthorizePath(int licenseeId, IList<int> userFunctionList, string url)
        {
            if (userFunctionList == null)
            {
                return false;
            }

            // Check permission on the url
            var functionList = _functionService.Functions;
            var mappedUrl = functionList.FirstOrDefault(f => CanMapUrl(url, f.Url));

            // Only authorize the function in function list
            return mappedUrl == null || userFunctionList.Any(f => f == mappedUrl.FunctionId);
        }

        #endregion AuthorizePath

        #region Private Method

        /// <summary>
        ///     Check member and transaction id belonging.
        /// </summary>
        /// <param name="custId">Customer Id.</param>
        /// <param name="transId">Transaction Id of bet ticker.</param>
        /// <returns>
        ///     If customer id > 0 return true if the customer id is belonged to the licensee.
        ///     If transaction Id > 0 return true if the transaction id is belonged to a customer of the licensee.
        /// </returns>
        [DebuggerStepThrough]
        public bool HasPermissionOnData(int custId, long transId)
        {
            if (custId == 0 && transId == 0)
            {
                return true;
            }

            var licenseeSession = _sessionService.LicenseeSession;

            return _authorizationDataAccess.IsCustomerBelonging(licenseeSession.LicenseeId, custId, transId);
        }

        private static string GetExecutionUrl(string rawUrl)
        {
            var index = rawUrl.IndexOf("?", System.StringComparison.Ordinal);
            return index > 0 ? rawUrl.Substring(0, index) : rawUrl;
        }

        private static NameValueCollection GetUrlParameters(string rawUrl)
        {
            var index = rawUrl.IndexOf("?", System.StringComparison.Ordinal);
            var query = index > 0 ? rawUrl.Substring(index + 1) : string.Empty;

            return index > 0
                       ? HttpUtility.ParseQueryString(query)
                       : new NameValueCollection();
        }

        /// <summary>
        ///     Check if an url can map with an url pattern.
        /// </summary>
        /// <param name="rawUrl">The url which need be checked.</param>
        /// <param name="urlPattern">The url pattern.</param>
        /// <returns>Can map url.</returns>
        private static bool CanMapUrl(string rawUrl, string urlPattern)
        {
            if (urlPattern.Length == 0)
            {
                return false;
            }

            rawUrl = rawUrl.ToLower();
            urlPattern = urlPattern.ToLower();

            var rawExcutionUrl = GetExecutionUrl(rawUrl);
            var rawParams = GetUrlParameters(rawUrl);
            var excutionPatternUrl = GetExecutionUrl(urlPattern);
            var paramPattern = GetUrlParameters(urlPattern);

            if (!rawExcutionUrl.Contains(excutionPatternUrl))
            {
                return false;
            }

            foreach (var key in paramPattern.AllKeys)
            {
                if (!rawParams.AllKeys.Contains(key))
                {
                    return false;
                }

                if (rawParams[key] != paramPattern[key])
                {
                    return false;
                }
            }

            return true;
        }

        #endregion Private Method
    }
}