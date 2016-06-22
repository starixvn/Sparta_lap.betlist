// -----------------------------------------------------------------------
// <copyright file="PlaytechCasinoViewModel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Report.ViewModel
{
    using System.Collections.Generic;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Model.Core.ViewModel;

    /// <summary>
    /// PLAYTECH Casino View Model.
    /// </summary>
    public class PlaytechCasinoViewModel : BaseViewModel
    {
        public PlaytechCasinoMetaModel Param { get; set; }

        public IList<PlaytechCasino> MainData { get; set; }

        public IList<PlaytechCasino> TotalData { get; set; }

        public PlaytechCasino TotalInEur { get; set; }

        public PlaytechCasino TotalInUsd { get; set; }

        public PlaytechCasino TotalInGbp { get; set; }
    }
}