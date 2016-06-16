// -----------------------------------------------------------------------
// <copyright file="BetSettingBaseService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Model.Core.DTO.Common;

    public class BetSettingBaseService
    {
        private readonly IAuthorizationService _authorizationService;

        public BetSettingBaseService()
        {
            _authorizationService = new AuthorizationService();
        }

        protected static IResultInfo ErrorResult(string message)
        {
            return new ResultInfo
            {
                Code = 1,
                Message = message
            };
        }

        protected bool HasPermissionOnData(int licenseeId, int custId)
        {
            return _authorizationService.HasPermissionOnData(licenseeId, custId, 0);
        }
    }
}