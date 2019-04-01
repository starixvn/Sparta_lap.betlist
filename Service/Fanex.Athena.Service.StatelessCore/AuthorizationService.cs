// -----------------------------------------------------------------------
// <copyright file="AuthorizationService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.StatelessCore
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Threading.Tasks;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.StatelessCore.Interfaces;

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAthorizationDataAccess _authorizationDataAccess;
        private readonly IFunctionService _functionService;

        public AuthorizationService(IAthorizationDataAccess authorizationDataAccess, IFunctionService functionService)
        {
            _authorizationDataAccess = authorizationDataAccess;
            _functionService = functionService;
        }

        public bool HasPermissions(IEnumerable<int> userFunctionList, params string[] permissionKeys)
        {
            var allFunctions = _functionService.Functions;
            var functions = allFunctions.Where(x => permissionKeys.Contains(x.FunctionKey, StringComparer.InvariantCultureIgnoreCase));

            return functions.Any() && userFunctionList.Any(id => functions.Any(x => x.FunctionId == id));
        }

        public bool HasPermissions(IEnumerable<string> userFunctionList, params string[] permissionKeys)
        {
            return userFunctionList.Any(x => permissionKeys.Contains(x, StringComparer.InvariantCultureIgnoreCase));
        }

        [DebuggerStepThrough]
        public async Task<bool> HasPermissionOnData(int licenseeId, int custId, long transId)
        {
            if (licenseeId <= 0)
            {
                throw new ArgumentException("licenseeId parameter is invalid.");
            }

            if (custId == 0 && transId == 0)
            {
                return true;
            }

            return await _authorizationDataAccess.IsCustomerBelonging(licenseeId, custId, transId).ConfigureAwait(false);
        }
    }
}