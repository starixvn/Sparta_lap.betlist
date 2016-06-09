// -----------------------------------------------------------------------
// <copyright file="SystemParlayDetailViewModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetList.ViewModel
{
    using Fanex.Athena.Model.BetList.MetaModel;

    /// <summary>
    /// SystemParlayDetailViewModel class.
    /// </summary>
    public class SystemParlayDetailViewModel
    {
        public string DetailText { get; set; }

        public SystemParlayDetailMetaModel Param { get; set; }
    }
}