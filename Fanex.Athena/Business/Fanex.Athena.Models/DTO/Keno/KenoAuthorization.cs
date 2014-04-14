// -----------------------------------------------------------------------
// <copyright file="KenoAuthorization.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Keno
{

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class KenoAuthorization
    {
        public int ErrorCode { get; set; }

        public string ErrorMsg { get; set; }

        public KenoAuthorizedData Data { get; set; }
    }
}
