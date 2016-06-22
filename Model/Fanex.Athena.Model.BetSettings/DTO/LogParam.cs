// -----------------------------------------------------------------------
// <copyright file="LogParam.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.BetSettings.DTO
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