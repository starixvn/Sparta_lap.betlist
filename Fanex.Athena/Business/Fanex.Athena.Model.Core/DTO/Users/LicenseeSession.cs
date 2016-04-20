// -----------------------------------------------------------------------
// <copyright file="LicenseeSession.cs" company="Nexcel">
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

        public int UserId { get; set; }

        public string UserName { get; set; }

        public IList<string> LicenseeType { get; set; }

        public IList<string> CurrencyName { get; set; }

        public bool IsAllowedCustomerComm { get; set; }
    }
}