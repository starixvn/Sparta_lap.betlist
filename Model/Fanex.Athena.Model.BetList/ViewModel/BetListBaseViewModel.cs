namespace Fanex.Athena.Model.BetList.ViewModel
{
    using System.Collections.Generic;
    using Core.MetaModel.Product;
    using Core.ViewModel;
    using MetaModel;

    public class BetListBaseViewModel : BaseViewModel
    {
        public BetListMetaModel Param { get; set; }

        public IEnumerable<ProductOption> ProductOptions { get; set; } = new HashSet<ProductOption>();
    }
}