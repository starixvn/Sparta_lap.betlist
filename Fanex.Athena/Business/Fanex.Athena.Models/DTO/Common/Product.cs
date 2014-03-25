// --------------------------------------------------------------------------
// <copyright file="Product.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
namespace Fanex.Athena.Models.DTO.Common
{
    using System;
    using Fanex.Athena.Extension.Utility;

    /// <summary>
    /// Product class.
    /// </summary>
    [Serializable]
    public class Product
    {
        #region Field

        public const string Sb = "SB";

        public const string Rb = "RB";

        public const string Ng = "NG";

        public const string Ba = "BA";

        public const string Lc = "LC";

        public const string Vs = "VS";

        public const string Cs = "CS";

        public const string Bg = "BG";
      
        public const string Kn = "KN";
      
        #endregion Field

        #region Constructor

        public Product()
        {
            ProductCode = Sb;
            ProductName = "SB";
            AllowCommission = true;
            AllowWinloss = true;
        }

        #endregion Constructor

        #region Property

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public bool AllowCommission { get; set; }

        public bool AllowWinloss { get; set; }

        /// <summary>
        /// Bet type validation - check if bet type is correct with product.
        /// </summary>
        /// <param name="bettype">Bet type value.</param>
        /// <returns>Bet type is valid or not.</returns>
        public bool ValidateBettype(int bettype)
        {
            return Util.ValidateBettpe(ProductCode, bettype);
        }

        #endregion Property
    }
}