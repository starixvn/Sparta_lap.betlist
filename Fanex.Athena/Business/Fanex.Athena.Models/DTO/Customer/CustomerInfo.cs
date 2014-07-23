// -----------------------------------------------------------------------
// <copyright file="CustomerInfo.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer
{
    /// <summary>
    /// CustomerInfo class.
    /// </summary>
    public class CustomerInfo
    {
        public bool Suspend { get; set; }

        public bool Closed { get; set; }

        public bool AutoSuspend { get; set; }

        public bool DisableAutoMpt { get; set; }

        public bool DisableHorseracing { get; set; }

        public bool DisableOpenTote { get; set; }

        public bool DisableFI { get; set; }

        public bool SyncCasino { get; set; }

        public bool DisableCasino { get; set; }

        public bool SyncLiveCasino { get; set; }

        public bool DisableLiveCasino { get; set; }

        public bool DisableOutright { get; set; }

        public bool IsInternal { get; set; }
    }
}