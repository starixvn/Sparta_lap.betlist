// -----------------------------------------------------------------------
// <copyright file="BalanceViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
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