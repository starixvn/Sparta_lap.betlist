// -----------------------------------------------------------------------
// <copyright file="ITurnOver.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Report
{
    using System.Collections.Generic;

    public interface ITurnOver
    {
        IDictionary<string, decimal> TurnOver { get; set; }

        decimal TotalTurnOver { get; }
    }
}