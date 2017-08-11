namespace Fanex.Athena.Model.Report.ViewModel
{
    using MetaModel;
    using Model.Core.ViewModel;

    public class CustomerWinLossViewModel : BaseViewModel
    {
        public CustomerWinLossMetaModel Param { get; set; }

        public bool IsHistory { get; set; }
    }
}