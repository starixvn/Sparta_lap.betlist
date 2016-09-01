// -----------------------------------------------------------------------
// <copyright file="AuthorizationService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Users
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using Fanex.Athena.DataAccess.Core.Users;
    using Fanex.Athena.DataAccess.Core.Users.Interfaces;
    using Fanex.Athena.Service.Core.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;
    using Fanex.Athena.Service.Core.Users.Interfaces;

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IAthorizationDataAccess _authorizationDataAccess;
        private readonly IFunctionService _functionService;

        public AuthorizationService()
        {
            _authorizationDataAccess = new AuthorizationDataAccess();
            _functionService = new FunctionService();
        }

        public bool HasPermissions(IEnumerable<int> userFunctionIds, string[] permissionKeys)
        {
            var allFunctions = _functionService.Functions;
            var functions = allFunctions.Where(x => permissionKeys.Contains(x.FunctionKey, StringComparer.InvariantCultureIgnoreCase));

            return functions.Any() && userFunctionIds.Any(id => functions.Any(x => x.FunctionId == id));
        }

        [DebuggerStepThrough]
        public bool HasPermissionOnData(int licenseeId, int custId, long transId)
        {
            if (licenseeId <= 0)
            {
                throw new ArgumentException("licenseeId parameter is invalid.");
            }

            if (custId == 0 && transId == 0)
            {
                return true;
            }

            return _authorizationDataAccess.IsCustomerBelonging(licenseeId, custId, transId);
        }
    }
}