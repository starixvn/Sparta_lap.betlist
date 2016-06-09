// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeGame.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO.AgCasino
{
    using System.Collections.Generic;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class GoldDeluxeGame
    {
        public string GameType { get; set; }

        public string Name { get; set; }

        public IList<GoldDeluxeGroup> Groups { get; set; }
    }
}