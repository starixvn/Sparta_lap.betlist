// -----------------------------------------------------------------------
// <copyright file="StakeLevel.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO.RngCasino
{
    /// <summary>
    /// StakeLevel class.
    /// </summary>
    public class StakeLevel
    {
        public int StakeLevelId { get; set; }

        public string StakeLevelName { get; set; }

        public string StakeLevelNameFormated
        {
            get
            {
                return StakeLevelName ?? string.Empty;
            }
        }

        public bool IsCheck { get; set; }
    }
}