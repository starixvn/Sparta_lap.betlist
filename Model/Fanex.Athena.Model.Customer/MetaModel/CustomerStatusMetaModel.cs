// -----------------------------------------------------------------------
// <copyright file="CustomerStatusMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.MetaModel
{
    /// <summary>
    /// CustomerStatusMetaModel class.
    /// </summary>
    public class CustomerStatusMetaModel
    {
        public int CustId { get; set; }

        public int LicenseeId { get; set; }

        public string LicenseeName { get; set; }

        public bool IsOn { get; set; }

        public string Ip { get; set; }

        public int ModifyById { get; set; }

        public string ModifyByUsername { get; set; }
    }
}