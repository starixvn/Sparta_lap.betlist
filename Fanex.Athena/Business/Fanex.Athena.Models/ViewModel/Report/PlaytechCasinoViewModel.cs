// -----------------------------------------------------------------------
// <copyright file="PlaytechCasinoViewModel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.ViewModel.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;

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
