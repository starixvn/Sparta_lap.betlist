// -----------------------------------------------------------------------
// <copyright file="RNGCasinoSetting.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DataDomain
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// RNGCasinoSetting class.
    /// </summary>
    [System.CLSCompliant(false)]
    public static class RNGCasinoSetting
    {
        public static readonly string _CASINO_GROUP1 = "01";

        public static readonly string _CASINO_GROUP2 = "02";

        private static ReadOnlyCollection<string> _specialGames = new ReadOnlyCollection<string>(
            new List<string>() { "S0001", "S0004", "S0003", "S0007", "S0005" });

        public static ReadOnlyCollection<string> SPECIALGAMES
        {
            get
            {
                return _specialGames;
            }
        }
    }
}