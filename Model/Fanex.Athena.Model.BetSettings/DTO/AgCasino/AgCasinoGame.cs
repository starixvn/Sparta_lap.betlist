// -----------------------------------------------------------------------
// <copyright file="AgCasinoGame.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO.AgCasino
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AgCasinoGame
    {
        public string GameType { get; set; }

        public string Name { get; set; }

        public IList<AgCasinoGroup> Groups { get; set; }
    }
}