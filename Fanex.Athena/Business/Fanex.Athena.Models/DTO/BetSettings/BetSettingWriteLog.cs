// -----------------------------------------------------------------------
// <copyright file="BetSettingWriteLog.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.BetSettings
{
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// BetSettingWriteLog class.
    /// </summary>
    public class BetSettingWriteLog
    {
        public BetSettingWriteLog()
        {
            TrackingParams = new List<LogParam>();
        }

        public List<LogParam> TrackingParams { get; set; }

        public int UserId { get; set; }

        public int CustId { get; set; }

        public string SpName { get; set; }

        public string Doing { get; set; }

        public int ActionId { get; set; }

        public string TrackingValue
        {
            get { return GetTrackingValue(); }
        }

        public int Duration { get; set; }

        /// <summary>
        /// Calculate the xml text which contains the updating information.
        /// </summary>
        /// <returns>Return Xml text.</returns>
        private string GetTrackingValue()
        {
            var result = new StringBuilder();
            result.Append("<").Append(SpName);
            foreach (var param in TrackingParams)
            {
                result.Append(" ");
                result.AppendFormat(param.Param + "=\"{0}\"", param.Value);
            }

            return result.Append("/>").ToString();
        }
    }
}