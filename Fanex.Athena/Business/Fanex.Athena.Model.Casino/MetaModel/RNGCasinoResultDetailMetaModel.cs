// -----------------------------------------------------------------------
// <copyright file="RNGCasinoResultDetailMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Model.Casino.MetaModel
{
    /// <summary>
    /// RNGCasinoResultDetailMetaModel class.
    /// </summary>
    public class RNGCasinoResultDetailMetaModel
    {
        public int UserBetId { get; set; }

        public int GameTypeId { get; set; }

        public string GameTypeName { get; set; }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public string PlayerHistoryNo { get; set; }

        public bool IsValid
        {
            get
            {
                return UserBetId > 0 && !string.IsNullOrEmpty(CurrencyName);
            }
        }
    }
}