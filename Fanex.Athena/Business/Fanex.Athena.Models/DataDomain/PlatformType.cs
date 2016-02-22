// -----------------------------------------------------------------------
// <copyright file="PlatformType.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
using Fanex.Athena.Models.ViewModel;

namespace Fanex.Athena.Models.DataDomain
{
    /// <summary>
    /// Site type constants.
    /// </summary>
    public static class PlatformType
    {
        /// <summary>
        /// All platform, value = 0.
        /// </summary>
        public const int All = 0;

        /// <summary>
        /// Asia platform, value = 1.
        /// </summary>
        public const int Asia = 1;

        /// <summary>
        /// NewAsia platform, value = 2.
        /// </summary>
        public const int NewAsia = 2;

        /// <summary>
        /// Euro platform, value = 3.
        /// </summary>
        public const int Euro = 3;

        /// <summary>
        /// WAP platform, value = 4.
        /// </summary>
        public const int Wap = 4;

        /// <summary>
        /// SMARTPHONE platform, value = 5.
        /// </summary>
        public const int SmartPhone = 5;

        public static string GetPlatformName(int type)
        {
            switch (type)
            {
                case Asia: return ViewResources.Asia;
                case NewAsia: return ViewResources.NewAsia;
                case Euro: return ViewResources.Euro;
                case Wap: return ViewResources.Wap;
                case SmartPhone: return ViewResources.SmartPhone.ToUpper();
                default: return ViewResources.All;
            }
        }
    }
}