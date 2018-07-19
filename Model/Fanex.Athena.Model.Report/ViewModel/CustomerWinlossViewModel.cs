namespace Fanex.Athena.Model.Report.ViewModel
{
    using System;
    using MetaModel;
    using Model.Core.ViewModel;

    public class CustomerWinLossViewModel : BaseViewModel
    {
        public CustomerWinLossMetaModel Param { get; set; }

        public bool IsHistory { get; set; }

        public bool IsFakeLicensee { get; set; }

        public DateTime MinDate { get; set; }

        public DateTime MaxDate { get; set; }
    }
}