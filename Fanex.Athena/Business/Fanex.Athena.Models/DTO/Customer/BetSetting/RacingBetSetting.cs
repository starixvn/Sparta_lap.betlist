// -----------------------------------------------------------------------
// <copyright file="RacingBetSetting.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer.BetSetting
{
    /// <summary>
    /// RacingBetSetting class.
    /// </summary>
    public class RacingBetSetting : CommonBetSetting
    {
        public RacingBetSetting(int custId, CommonBetSetting setting)
        {
            CustId = custId;
            UserId = setting.UserId;
            Ip = setting.Ip;
            Language = setting.Language;
        }

        public RacingBetSetting(
            int custId,
            int userId,
            decimal minBet,
            decimal maxBet,
            decimal maxPerMatch,
            bool isUpdateMultiple,
            bool isDataChange,
            string ip,
            string language)
        {
            CustId = custId;
            UserId = userId;
            MinBet = minBet;
            MaxBet = maxBet;
            MaxPerMatch = maxPerMatch;
            IsUpdateMultiple = isUpdateMultiple;
            IsDataChange = isDataChange;
            Ip = ip;
            Language = language;
        }

        public bool IsDataChange { get; set; }

        public bool IsUpdateMultiple { get; set; }
    }
}