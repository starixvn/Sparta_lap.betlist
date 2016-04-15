// -----------------------------------------------------------------------
// <copyright file="BetSettingBaseService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.BetSettings
{
    using Fanex.Athena.Service.Core.Users;
    using Fanex.Athena.Service.Core.Users.Interfaces;
    using Models.DTO.Common;

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

        protected bool HasPermissionOnData(int custId)
        {
            return _authorizationService.HasPermissionOnData(custId, 0);
        }
    }
}