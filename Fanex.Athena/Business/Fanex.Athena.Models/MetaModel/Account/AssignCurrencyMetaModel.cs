// -----------------------------------------------------------------------
// <copyright file="AssignCurrencyMetaModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Account
{
    /// <summary>
    /// AssignCurrencyMetaModel class.
    /// </summary>
    public class AssignCurrencyMetaModel
    {
        public AssignCurrencyMetaModel()
        {
            LicenseeId = 0;
        }

        public int UserId { get; set; }

        public int LicenseeId { get; set; }

        public string CurrencyList { get; set; }

        public int ModifyBy { get; set; }

        public string LangKey { get; set; }

        public string IP { get; set; }
    }
}