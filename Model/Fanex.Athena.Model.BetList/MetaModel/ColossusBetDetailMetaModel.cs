// -----------------------------------------------------------------------
// <copyright file="ColossusBetDetailMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.MetaModel
{
    public class ColossusBetDetailMetaModel
    {
        public int LicenseeId { get; set; }
        
        public int UserId { get; set; }

        public int CustId { get; set; }

        public long RefNo { get; set; }

        public string BaseCurrency { get; set; }
    }
}