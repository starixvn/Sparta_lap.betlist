namespace Fanex.Athena.Model.Report.MetaModel.Result
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Script.Serialization;

    [Serializable]
    public class ResultMetaModel
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

        [ScriptIgnore]
        public int LicenseeId { get; set; }

        [ScriptIgnore]
        public string LanguageCode { get; set; }

        [ScriptIgnore]
        public int UserId { get; set; }
    }
}
