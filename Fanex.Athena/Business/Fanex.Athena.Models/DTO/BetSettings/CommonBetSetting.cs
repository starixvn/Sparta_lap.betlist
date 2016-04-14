// -----------------------------------------------------------------------
// <copyright file="CommonBetSetting.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetSettings
{
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    /// CommonBetSetting class.
    /// </summary>
    public class CommonBetSetting
    {
        public CommonBetSetting()
        {
            ResultInfo = new ResultInfo { Code = 0, Message = string.Empty };
        }

        public int LicenseeId { get; set; }

        public string Ip { get; set; }

        public int UserId { get; set; }

        public string Language { get; set; }

        public decimal MinBet { get; set; }

        public decimal MaxBet { get; set; }

        public decimal MaxPerMatch { get; set; }

        public int CustId { get; set; }

        public ResultInfo ResultInfo { get; set; }
    }
}