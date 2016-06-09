// -----------------------------------------------------------------------
// <copyright file="EsportSettingType.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DataDomain
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public enum EsportSettingType : int
    {
        /// <summary>
        /// Licensee's not offered.
        /// </summary>
        NoneOffering = -1,

        /// <summary>
        /// Licensee's offered.
        /// </summary>
        LicenseeOffering = 0,

        /// <summary>
        ///  Licensee's offered but not offered for member.
        /// </summary>
        NoneMemberOffering = 1
    }
}