namespace Fanex.Athena.Model.BetList.MetaModel
{
    using System;

    public class SingleCustomersBetListMetaModel : BaseBettingTicketMetaModel
    {
        public DateTime FromDate { get; set; }

        public DateTime ToDate { get; set; }

        public string ProductCode { get; set; }

        public string UserName { get; set; }

        public string Status { get; set; }

        public int PlatformType { get; set; }

        public int LiveStatus { get; set; }

        public override bool IsValid => !string.IsNullOrWhiteSpace(UserName);
    }
}