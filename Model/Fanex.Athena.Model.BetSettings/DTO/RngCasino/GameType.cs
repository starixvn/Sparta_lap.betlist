// -----------------------------------------------------------------------
// <copyright file="GameType.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO.RngCasino
{
    using System.Collections.Generic;

    /// <summary>
    /// GameType class.
    /// </summary>
    public class GameType
    {
        public int GameTypeId { get; set; }

        public string GameTypeName { get; set; }

        public IList<StakeLevel> StakeLevelList { get; set; }
    }
}