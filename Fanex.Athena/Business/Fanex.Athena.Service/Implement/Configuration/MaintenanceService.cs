// -----------------------------------------------------------------------
// <copyright file="MaintenanceService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Configuration
{
    using System;
    using System.Threading;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Models.DTO.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Caching;
    using Fanex.Logging;
    using Sunplus.UMControllerLibrary;

    /// <summary>
    /// Maintenance service class.
    /// </summary>
    public class MaintenanceService : IMaintenanceService
    {
        #region Constant

        /// <summary>
        ///     Cache time out (in second).
        /// </summary>
        private const int CacheTimeoutMinute = 1;

        private const int WaitInterval = 1000; // 1 seconds

        private const string MaintenanceCacheKey = "Fanex.Cache.LRF.Maintenance";

        #endregion Constant

        #region Field

        private static readonly Mutex Mutex = new Mutex();

        private readonly ICache _cacheManager = CacheService.CreateInstance(CacheType.MemoryCache);

        /// <summary>
        ///     Load balancer object.
        /// </summary>
        private readonly UMBusiness _umservice;

        #endregion Field

        #region Constructor

        public MaintenanceService()
        {
            _umservice = new UMBusiness();
        }

        #endregion Constructor

        #region Property

        public MaintenanceInfo MaintenanceInfo
        {
            get
            {
                var maintenanceInfo = this._cacheManager.Get<MaintenanceInfo>(MaintenanceCacheKey);
                if (maintenanceInfo == null)
                {
                    if (Mutex.WaitOne(WaitInterval))
                    {
                        try
                        {
                            maintenanceInfo = GetMaintenanceInfo();
                            this._cacheManager.Set(
                                MaintenanceCacheKey,
                                maintenanceInfo,
                                DateTime.Now.AddMinutes(CacheTimeoutMinute));
                        }
                        catch
                        {
                            maintenanceInfo = new MaintenanceInfo { IsDownTime = false };
                        }
                        finally
                        {
                            Mutex.ReleaseMutex();
                        }
                    }
                    else
                    {
                        maintenanceInfo = new MaintenanceInfo { IsDownTime = false };
                    }
                }

                return maintenanceInfo;
            }
        }

        #endregion Property

        #region Method

        /// <summary>
        ///     Call UmService service to get maintenance duration.
        /// </summary>
        /// <returns>
        ///     Return true if current time is in maintenance duration.
        /// </returns>
        public MaintenanceInfo GetMaintenanceInfo()
        {
            var maintenanceInfo = new MaintenanceInfo { IsDownTime = false };

            if (string.IsNullOrEmpty(AppSettings.LbUrl))
            {
                return maintenanceInfo;
            }

            try
            {
                var dateFrom = maintenanceInfo.UmTimeFrom;
                var dateTo = maintenanceInfo.UmTimeTo;
                maintenanceInfo.IsDownTime = _umservice.isUnderMaintainence(
                    Sites.LRF,
                    AppSettings.LbUrl,
                    ref dateFrom,
                    ref dateTo);

                if (maintenanceInfo.IsDownTime)
                {
                    maintenanceInfo.UmTimeFrom = dateFrom;
                    maintenanceInfo.UmTimeTo = dateTo;
                }
            }
            catch (System.Exception exception)
            {
                LogService.Error(exception.Message, exception);
            }

            return maintenanceInfo;
        }

        #endregion Method
    }
}