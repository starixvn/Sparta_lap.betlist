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

        public const string SB = "SB";

        public const string RB = "RB";

        public const string NG = "NG";

        public const string BA = "BA";

        public const string LC = "LC";

        public const string VS = "VS";

        public const string CS = "CS";

        public const string BG = "BG";
      
        public const string KN = "KN";

        public const string AGC = "AGC";

        public const string PTCS = "PTCS";

        public const string GD = "GD";

        public const string VB = "VB";
      
        #endregion Field

        #region Constructor

        public Product()
        {
            ProductCode = SB;
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