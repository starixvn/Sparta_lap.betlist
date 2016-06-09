// -----------------------------------------------------------------------
// <copyright file="BetListBaseViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.ViewModel
{
    using Fanex.Athena.Model.BetList.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// BetListBaseViewModel class.
    /// </summary>
    public class BetListBaseViewModel : BaseViewModel
    {
        public BetListMetaModel Param { get; set; }
    }
}