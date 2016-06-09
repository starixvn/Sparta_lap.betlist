// -----------------------------------------------------------------------
// <copyright file="ILicenseeWL.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System.Collections.Generic;

    public interface ILicenseeWL
    {
        IDictionary<string, decimal> LicenseeWL { get; set; }

        decimal TotalLicenseeWL { get; }
    }
}