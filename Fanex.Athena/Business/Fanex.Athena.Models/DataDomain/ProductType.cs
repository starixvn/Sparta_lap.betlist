// -----------------------------------------------------------------------
// <copyright file="ProductType.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.DataDomain
{
    using System;

    [Flags]
    public enum ProductTypes
    {
        /// <summary>
        /// None, value = 0.
        /// </summary>
        None = 0,

        /// <summary>
        /// SportsBook product.
        /// </summary>
        Sportsbook = 1,

        /// <summary>
        /// Racing product.
        /// </summary>
        Racing = 2,

        /// <summary>
        /// NumberGame product.
        /// </summary>
        NumberGame = 3,

        /// <summary>
        /// VirtualSports product.
        /// </summary>
        VirtualSports = 4,

        /// <summary>
        /// LiveCasino product.
        /// </summary>
        LiveCasino = 5,

        /// <summary>
        /// RNGCasino product.
        /// </summary>
        RngCasino = 6,

        /// <summary>
        /// Keno product.
        /// </summary>
        Keno = 7,

        /// <summary>
        /// AGCasino product.
        /// </summary>
        AgCasino = 8,

        /// <summary>
        /// Gold Deluxe product: 11.
        /// </summary>
        GoldDeluxe = 11,

        /// <summary>
        /// Macau product.
        /// </summary>
        VegasGames = 12,
        
        /// <summary>
        /// Bet Trade product: 13.
        /// </summary>
        BetTrade = 13,

        /// <summary>
        /// Micro Gaming product: 14.
        /// </summary>
        MicroGaming = 14
    }
}