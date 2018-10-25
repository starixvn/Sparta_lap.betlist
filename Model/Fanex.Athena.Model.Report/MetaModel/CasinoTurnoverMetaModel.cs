namespace Fanex.Athena.Model.Report.MetaModel
{
    using System;
    using System.Linq;

    public class CasinoTurnoverMetaModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public int CurrencyId { get; set; }

        public string BaseCurrency { get; set; }

        public bool IsConvertAll { get; set; }

        public string[] ProductCode { get; set; }

        public string UserName { get; set; }

        public int[] Status { get; set; }

        public bool IsValid => FromDate <= ToDate && ProductCode.Any() && Status.Any();
    }
}