// -----------------------------------------------------------------------
// <copyright file="GameKind.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer.BetSetting.RngCasino
{
    using System.Collections.Generic;

    /// <summary>
    /// GameKind class.
    /// </summary>
    public class GameKind
    {
        public int GameKindId { get; set; }

        public string GameKindName { get; set; }

        public IList<GameType> GameTypeList { get; set; }
    }
}