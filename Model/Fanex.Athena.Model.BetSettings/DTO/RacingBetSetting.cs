// -----------------------------------------------------------------------
// <copyright file="RacingBetSetting.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO
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

        public bool IsDataChange { get; set; }

        public bool IsUpdateMultiple { get; set; }
    }
}