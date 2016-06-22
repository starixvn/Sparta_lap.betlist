// -----------------------------------------------------------------------
// <copyright file="TotalBetsBaseMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.MetaModel
{
    using Fanex.Athena.Model.Core.MetaModel;

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