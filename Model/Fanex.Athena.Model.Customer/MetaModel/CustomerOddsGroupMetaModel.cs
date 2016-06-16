// -----------------------------------------------------------------------
// <copyright file="CustomerOddsGroupMetaModel.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.MetaModel
{
    /// <summary>
    /// CustomerOddsGroupMetaModel class.
    /// </summary>
    public class CustomerOddsGroupMetaModel
    {
        public int LicenseeId { get; set; }

        public int CustId { get; set; }

        public int UserId { get; set; }

        public string OddsGroup { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }        
    }
}