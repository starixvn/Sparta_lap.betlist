// -----------------------------------------------------------------------
// <copyright file="TotalBetsBaseMetaModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.TotalBet.MetaModel
{
    using Fanex.Athena.Model.Core.MetaModel;

    public class TotalBetsBaseMetaModel : BaseMetaModel
    {
        private const int DefaultPageSize = 1000;

        public TotalBetsBaseMetaModel()
        {
            CurrencyId = 0;
            PageIndex = 1;
            PageSize = DefaultPageSize;
        }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        public int TotalRows { get; set; }
    }
}