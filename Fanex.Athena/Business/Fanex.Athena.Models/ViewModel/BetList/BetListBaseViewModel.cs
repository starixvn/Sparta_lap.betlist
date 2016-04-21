// -----------------------------------------------------------------------
// <copyright file="BetListBaseViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetList
{
    using Fanex.Athena.Models.MetaModel.BetList;
    using Model.Core.ViewModel;

    /// <summary>
    /// BetListBaseViewModel class.
    /// </summary>
    public class BetListBaseViewModel : BaseViewModel
    {
        public BetListMetaModel Param { get; set; }
    }
}