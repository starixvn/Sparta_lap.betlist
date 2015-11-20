// -----------------------------------------------------------------------
// <copyright file="Validator.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.Helper
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.Athena.Models.DTO.Common;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class Validator
    {
        #region BetType Range

        private const int SBMinBetType = 30;
        private const int SBMaxBetType = 999;
        private const int SBMinOfMaxBettype = 120;
        private const int SBMinSportingSolutionBetType = 1301;
        private const int SBMaxSportingSolutionBetType = 1334;
        private const int NGMinBetType = 81;
        private const int NGMaxBetType = 119;
        private const int RBMinBetType = 31;
        private const int RBMaxBetType = 33;
        private const int VSMinBetType = 1201;
        private const int VSMaxBetType = 1299;
        private const int LCMinBetType = 1101;
        private const int LCMaxBetType = 1199;
        private const int CSMinBetType = 71;
        private const int CSMaxBetType = 71;
        private const int BGMinBetType = 73;
        private const int BGMaxBetType = 73;
        private const int KenoMinBetType = 1501;
        private const int KenoMaxBetType = 1599;
        private const int MicrogamingMinBetType = 2001;
        private const int MicrogamingMaxBetType = 2009;

        #endregion BetType Range

        /// <summary>
        /// Validate bet type by product.
        /// </summary>
        /// <param name="productCode">The product code.</param>
        /// <param name="bettype">The bet type.</param>
        /// <returns>True or false.</returns>
        public static bool IsValidBettype(string productCode, int bettype)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                return false;
            }

            var code = productCode.ToUpper();
            switch (code)
            {
                case Product.SB:
                    return bettype <= SBMinBetType || (bettype >= SBMinOfMaxBettype && bettype <= SBMaxBetType) ||
                        (bettype >= SBMinSportingSolutionBetType && bettype <= SBMaxSportingSolutionBetType);
                case Product.BA:
                    return bettype <= SBMinBetType || (bettype >= SBMinOfMaxBettype && bettype <= SBMaxBetType);
                case Product.RB:
                    return bettype >= RBMinBetType && bettype <= RBMaxBetType;
                case Product.NG:
                    return bettype >= NGMinBetType && bettype <= NGMaxBetType;
                case Product.VS:
                    return bettype >= VSMinBetType && bettype <= VSMaxBetType;
                case Product.LC:
                    return bettype >= LCMinBetType && bettype <= LCMaxBetType;
                case Product.BG:
                    return bettype >= BGMinBetType && bettype <= BGMaxBetType;
                case Product.CS:
                    return bettype >= CSMinBetType && bettype <= CSMaxBetType;
                case Product.KN:
                    return bettype >= KenoMinBetType && bettype <= KenoMaxBetType;
                case Product.MGC:
                    return bettype >= MicrogamingMinBetType && bettype <= MicrogamingMaxBetType;
            }

            return false;
        }
    }
}
