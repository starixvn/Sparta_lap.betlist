// -----------------------------------------------------------------------
// <copyright file="GoldDeluxeGame.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO.GoldDeluxe
{
    using System.Collections.Generic;

    public class GoldDeluxeGame
    {
        public string GameType { get; set; }

        public string Name { get; set; }

        public IList<GoldDeluxeGroup> Groups { get; set; }
    }
}