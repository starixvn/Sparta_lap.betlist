// -----------------------------------------------------------------------
// <copyright file="StakeLevel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetSettings.RngCasino
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
                var originalStakeName = StakeLevelName;
                if (!string.IsNullOrEmpty(originalStakeName))
                {
                    var stakeItem = originalStakeName.Split('-');
                    var minStakeStr = stakeItem[0].Replace(",", string.Empty);
                    var maxStakeStr = stakeItem[1].Replace(",", string.Empty);

                    var minStake = Fanex.Common.Converter.ToDecimal(minStakeStr);
                    var maxStake = Fanex.Common.Converter.ToDecimal(maxStakeStr);

                    minStakeStr = minStake.ToString("#,##0.##");
                    maxStakeStr = maxStake.ToString("#,##0.##");

                    return minStakeStr + "-" + maxStakeStr;
                }

                return string.Empty;
            }
        }

        public bool IsCheck { get; set; }
    }
}