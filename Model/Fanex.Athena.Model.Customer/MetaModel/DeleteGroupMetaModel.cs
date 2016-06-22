// -----------------------------------------------------------------------
// <copyright file="DeleteGroupMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.MetaModel
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class DeleteGroupMetaModel
    {
        public int GroupId { get; set; }

        public int LicenseeId { get; set; }

        public int UserId { get; set; }

        public string UserName { get; set; }

        public string Ip { get; set; }

        public string Language { get; set; }

        public bool IsRemoveCommSetting { get; set; }

        public bool IsResetDefault { get; set; }

        public bool IsAllowedCSComm { get; set; }
    }
}
