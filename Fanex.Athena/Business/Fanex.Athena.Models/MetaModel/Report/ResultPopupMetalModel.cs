// -----------------------------------------------------------------------
// <copyright file="ResultPopupMetalModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// ResultPopupMetalModel class.
    /// </summary>
    [Serializable]
    public class ResultPopupMetalModel
    {
        [Required]
        public int MatchId
        {
            get;
            set;
        }

        [Required]
        public int Bettype { get; set; }

        public int? RaceNo { get; set; }

        [Required]
        public int SportType { get; set; }

        public int? IsOutRight { get; set; }

        public long? RefNo { get; set; }

        public DateTime? WinLossDate { get; set; }
    }
}