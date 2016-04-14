// -----------------------------------------------------------------------
// <copyright file="NumberGameBetSetting.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetSettings
{
    /// <summary>
    /// NumberGameBetSetting class.
    /// </summary>
    public class NumberGameBetSetting : CommonBetSetting
    {
        public decimal MaxPerBall { get; set; }

        public bool IsDataChange { get; set; }
    }
}