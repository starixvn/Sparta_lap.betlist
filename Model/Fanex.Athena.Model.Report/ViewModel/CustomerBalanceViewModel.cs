// -----------------------------------------------------------------------
// <copyright file="CustomerBalanceViewModel.cs" company="">
// warehouse winloss form parameters
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// CustomerBalanceViewModel class.
    /// </summary>
    public class CustomerBalanceViewModel : BaseViewModel
    {
        public CustomerBalanceMetaModel Param { get; set; }
    }
}