// -----------------------------------------------------------------------
// <copyright file="BetListBaseViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.ViewModel
{
    using System.Collections.Generic;
    using Core.MetaModel.Product;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.Core.ViewModel;

    public class BetListBaseViewModel : BaseViewModel
    {
        public BetListMetaModel Param { get; set; }

        public IEnumerable<ProductOption> ProductOptions { get; set; } = new HashSet<ProductOption>();
    }
}