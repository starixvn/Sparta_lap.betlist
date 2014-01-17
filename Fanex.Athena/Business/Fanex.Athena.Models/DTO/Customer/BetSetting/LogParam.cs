// -----------------------------------------------------------------------
// <copyright file="LogParam.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Customer.BetSetting
{
    /// <summary>
    /// LogParameter class.
    /// </summary>
    public class LogParam
    {
        public LogParam()
        {
            Param = string.Empty;
            Value = null;
        }

        public string Param { get; set; }

        public object Value { get; set; }
    }
}