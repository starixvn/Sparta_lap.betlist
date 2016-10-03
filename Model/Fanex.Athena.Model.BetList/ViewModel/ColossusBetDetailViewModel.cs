namespace Fanex.Athena.Model.BetList.ViewModel
{
    using System.Collections.Generic;
    using DTO;

    public class ColossusBetDetailViewModel
    {
        public IEnumerable<ColossusBetDetail> DataList { get; set; }

        public decimal TotalSoldPercent { get; set; }

        public string IframeUrl { get; set; }
    }
}