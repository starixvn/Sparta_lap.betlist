// -----------------------------------------------------------------------
// <copyright file="IMaintenanceService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

/*
 * Created 20100205@Lee - Maintenance configuration.
 * Dependencies: LoadBalancer.dll-->LoadBalancerLibrary
 * Revision:
 * ?@? - ...
 *
 */

namespace Fanex.Athena.Service.Interface.Configuration
{
    using Fanex.Athena.Models.DTO.Configuration;

    public interface IMaintenanceService
    {
        #region Property

        MaintenanceInfo MaintenanceInfo { get; }

        #endregion Property
    }
}