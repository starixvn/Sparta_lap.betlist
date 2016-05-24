// -----------------------------------------------------------------------
// <copyright file="StakeLevel.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

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

                var originalStakeName = StakeLevelName;
                if (!string.IsNullOrEmpty(originalStakeName))
                {
                    var stakeItem = originalStakeName.Split('-');
                    var minStakeStr = stakeItem[0].Replace(",", string.Empty);
                    var maxStakeStr = stakeItem[1].Replace(",", string.Empty);

                    try
                    {
                        var minStake = Convert.ToDecimal(minStakeStr);
                        var maxStake = Convert.ToDecimal(maxStakeStr);
                        minStakeStr = minStake.ToString("#,##0.##");
                        maxStakeStr = maxStake.ToString("#,##0.##");

                    }
                    catch (Exception ex)
                    {
                        throw new Exception("originalStakeName=" + StakeLevelName, ex);
                    }

                    return minStakeStr + "-" + maxStakeStr;
                }

                return string.Empty;


            }
        }

        public bool IsCheck { get; set; }
    }
}