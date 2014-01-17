// -----------------------------------------------------------------------
// <copyright file="SystemParlayDetailViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.BetList
{
    using Fanex.Athena.Models.MetaModel.BetList;

    /// <summary>
    /// SystemParlayDetailViewModel class.
    /// </summary>
    public class SystemParlayDetailViewModel
    {
        public string DetailText { get; set; }

        public SystemParlayDetailMetaModel Param { get; set; }
    }
}