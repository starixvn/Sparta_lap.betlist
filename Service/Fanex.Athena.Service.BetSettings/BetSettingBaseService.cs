// -----------------------------------------------------------------------
// <copyright file="BetSettingBaseService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetSettings
{
    using System.Threading.Tasks;
    using Fanex.Athena.Service.StatelessCore.Interfaces;
    using Model.Core.DTO.Common;

    public class BetSettingBaseService
    {
        private readonly IAuthorizationService _authorizationService;

        public BetSettingBaseService(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        protected static IResultInfo ErrorResult(string message)
        {
            return new ResultInfo
            {
                Code = 1,
                Message = message
            };
        }

        protected async Task<bool> HasPermissionOnData(int licenseeId, int custId)
        {
            return await _authorizationService.HasPermissionOnData(licenseeId, custId, 0).ConfigureAwait(false);
        }
    }
}