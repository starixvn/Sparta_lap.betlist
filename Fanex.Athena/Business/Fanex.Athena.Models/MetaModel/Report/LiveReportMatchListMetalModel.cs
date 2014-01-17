// -----------------------------------------------------------------------
// <copyright file="LiveReportMatchListMetalModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Report
{
    using System;
    using System.ComponentModel.DataAnnotations;

    /// <summary>
    /// LiveReportMatchListMetalModel class.
    /// </summary>
    [Serializable]
    public class LiveReportMatchListMetalModel : BaseReportMetaModel
    {
        [Required]
        public int SportId { get; set; }

        public string ProductCode { get; set; }

        public override bool IsValid
        {
            get
            {
                return FromDate <= ToDate;
            }
        }
    }
}