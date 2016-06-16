// -----------------------------------------------------------------------
// <copyright file="AuthorizationService.cs" company="Nexcel">
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

        public bool HasPermission(int licenseeId, IEnumerable<int> userFunctionIds, string permissionKey)
        {
            var functionList = _functionService.Functions;
            var function = functionList.FirstOrDefault(x => x.FunctionKey.Equals(permissionKey, System.StringComparison.OrdinalIgnoreCase));

            return function != null && userFunctionIds.Any(x => x == function.FunctionId);
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