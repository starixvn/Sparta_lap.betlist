// -----------------------------------------------------------------------
// <copyright file="ILicenseeWL.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System.Collections.Generic;

    public interface ILicenseeWL
    {
        IDictionary<string, decimal> LicenseeWL { get; set; }

        decimal TotalLicenseeWL { get; }
    }
}