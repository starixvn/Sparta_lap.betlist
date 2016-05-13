// -----------------------------------------------------------------------
// <copyright file="RNGCasinoSetting.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DataDomain
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    /// <summary>
    /// RNGCasinoSetting class.
    /// </summary>
    public static class RNGCasinoSetting
    {
        public const string CasinoGroup1 = "01";

        public const string CasinoGroup2 = "02";

        private static ReadOnlyCollection<string> _specialGames = new ReadOnlyCollection<string>(
            new List<string>() { "S0001", "S0004", "S0003", "S0007", "S0005" });

        public static ReadOnlyCollection<string> SpecialGames
        {
            get
            {
                return _specialGames;
            }
        }
    }
}