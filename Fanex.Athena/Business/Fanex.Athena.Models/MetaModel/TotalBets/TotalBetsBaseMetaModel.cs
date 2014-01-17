// -----------------------------------------------------------------------
// <copyright file="TotalBetsBaseMetaModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.TotalBets
{
    /// <summary>
    /// TotalBetsBaseMetaModel class.
    /// </summary>
    public class TotalBetsBaseMetaModel : BaseMetaModel
    {
        public TotalBetsBaseMetaModel()
        {
            LicenseeType = null;
            CurrencyId = 0;
            PageIndex = 1;
            PageSize = 1000;
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }
    }
}