// -----------------------------------------------------------------------
// <copyright file="BetListBaseViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.BetList.Model.ViewModel
{
    using System.Collections.Generic;
    using Common.Model.MetaModel;
    using Fanex.Athena.BetList.Model.MetaModel;

    public class BetListBaseViewModel
    {
        public BetListMetaModel Param { get; set; }

        public IEnumerable<ProductOption> ProductOptions { get; set; } = new HashSet<ProductOption>();
    }
}