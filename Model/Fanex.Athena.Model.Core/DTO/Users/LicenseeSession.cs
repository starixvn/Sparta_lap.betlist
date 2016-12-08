// -----------------------------------------------------------------------
// <copyright file="LicenseeSession.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Users
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// LicenseeSession class.
    /// </summary>
    [Serializable]
    public class LicenseeSession
    {
        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public IList<string> LicenseeType { get; set; }

        public IList<string> CurrencyName { get; set; }

        public bool IsAllowedCustomerComm { get; set; }

        public bool IsEnableBA { get; set; }
    }
}