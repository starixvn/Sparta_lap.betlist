// -----------------------------------------------------------------------
// <copyright file="BalanceViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// BalanceViewModel class.
    /// </summary>
    public class BalanceViewModel : BaseViewModel
    {
        public Balance MainData { get; set; }

        public BalanceMetaModel Param { get; set; }
    }
}