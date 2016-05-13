// -----------------------------------------------------------------------
// <copyright file="MoveBet.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Configuration
{
    using System;

    /// <summary>
    /// MoveBet class.
    /// </summary>
    [Serializable]
    public class MoveBet
    {
        public DateTime AvailableWinlostDate { get; set; }

        public DateTime? EstimatedTime { get; set; }
    }
}