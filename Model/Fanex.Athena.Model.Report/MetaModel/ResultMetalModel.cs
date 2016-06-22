// -----------------------------------------------------------------------
// <copyright file="ResultMetalModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// ResultMetalModel class.
    /// </summary>
    [Serializable]
    public class ResultMetalModel
    {
        public DateTime? BalanceUpDay
        {
            get;
            set;
        }

        [Required]
        public DateTime FromDate { get; set; }

        [Required]
        public DateTime ToDate { get; set; }

        [Required]
        public bool IsOutRight { get; set; }

        [Required]
        public bool IsExportExcel { get; set; }

        [Required]
        public int SportId { get; set; }

        [Required]
        public int LeagueId { get; set; }
    }
}