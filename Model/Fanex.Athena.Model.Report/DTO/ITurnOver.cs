// -----------------------------------------------------------------------
// <copyright file="ITurnOver.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.DTO
{
    using System.Collections.Generic;

    public interface ITurnOver
    {
        IDictionary<string, decimal> TurnOver { get; set; }

        decimal TotalTurnOver { get; }
    }
}