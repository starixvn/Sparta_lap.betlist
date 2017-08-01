// --------------------------------------------------------------------------
// <copyright file="Product.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
namespace Fanex.Athena.Model.Core.DTO.Common
{
    using System;
    using Fanex.Athena.Model.Core.Helper;

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

        public const string VG = "VG";

        public const string BT = "BT";

        public const string MGCLC = "MGCLC";

        public const string MGCRNG = "MGCRNG";

        public const string VB = "VB";

        public const string AB = "AB";

        public const string FW = "FW";

        public const string CO = "CO";

        public const string CB = "CB";

        public const string VS2 = "VS2";

        #endregion Field

        #region Constructor

        public Product()
        {
            ProductCode = SB;
            ProductName = nameof(SB);
            AllowCommission = true;
            AllowWinloss = true;
        }

        #endregion Constructor

        #region Property

        public int ProductID { get; set; }

        public string ProductName { get; set; }

        public string ProductCode { get; set; }

        public bool AllowCommission { get; set; }

        public bool AllowWinloss { get; set; }

        public bool IsOfferedSingleWallet { get; set; }

        /// <summary>
        /// Bet type validation - check if bet type is correct with product.
        /// </summary>
        /// <param name="bettype">Bet type value.</param>
        /// <returns>Bet type is valid or not.</returns>
        public bool ValidateBettype(int bettype)
        {
            return Validator.IsValidBettype(ProductCode, bettype);
        }

        #endregion Property
    }
}