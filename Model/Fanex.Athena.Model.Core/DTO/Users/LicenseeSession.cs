// -----------------------------------------------------------------------
// <copyright file="LicenseeSession.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Users
{
    using System;
    using Configuration;

    [Serializable]
    public class LicenseeSession
    {
        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public bool IsAllowedCustomerComm { get; set; }

        public bool IsEnableBA { get; set; }

        public FakeLicenseeSetting FakeLicenseeSetting { get; set; }
    }
}