// -----------------------------------------------------------------------
// <copyright file="DrawFilterType.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
namespace Fanex.Athena.Model.Core.DataDomain
{
    /// <summary>
    /// Draw filter type constants.
    /// </summary>
    public sealed class DrawFilterType
    {
        /// <summary>
        /// Normal, value = 1.
        /// </summary>
        public const int Normal = 1;

        /// <summary>
        /// NoDraws, value = 2.
        /// </summary>
        public const int NoDraws = 2;

        /// <summary>
        /// No Draws For Turnover Only, value = 3.
        /// </summary>
        public const int NoDrawsForTurnoverOnly = 3;

        private DrawFilterType() 
        { 
        }
    }
}