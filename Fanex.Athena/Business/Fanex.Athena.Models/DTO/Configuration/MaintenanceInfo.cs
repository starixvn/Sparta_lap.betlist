// -----------------------------------------------------------------------
// <copyright file="MaintenanceInfo.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Configuration
{
    using System;

    /// <summary>
    /// MaintenanceInfo class.
    /// </summary>
    [Serializable]
    public class MaintenanceInfo
    {
        public MaintenanceInfo()
        {
            IsDownTime = false;

            var nowHour = DateTime.Today.AddHours(DateTime.Now.Hour);
            UmTimeFrom = nowHour.ToShortTimeString();
            UmTimeTo = nowHour.AddHours(1).ToShortTimeString();
        }

        public bool IsDownTime { get; set; }

        public string UmTimeFrom { get; set; }

        public string UmTimeTo { get; set; }
    }
}