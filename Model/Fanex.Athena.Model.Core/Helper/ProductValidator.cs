namespace Fanex.Athena.Model.Core.Helper
{
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Model.Core.DTO.Common;

    public static class ProductValidator
    {
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
        private const int CSNewMinBetType = 2101;
        private const int CSNewMaxBetType = 2399;
        private const int BGMinBetType = 73;
        private const int BGMaxBetType = 73;
        private const int KenoMinBetType = 1501;
        private const int KenoMaxBetType = 1599;
        private const int MicrogamingMinBetType = 2001;
        private const int MicrogamingMaxBetType = 2009;
        private const int VoidbridgeMinBetType = 4001;
        private const int VoidbridgeMaxBetType = 7999;
        private const int AllBetMinBetType = 2401;
        private const int AllBetMaxBetType = 2499;
        private const int FWMinBetType = 1011;
        private const int FWMaxBetType = 1020;
        private const int VS2MinBetType = 2700;
        private const int VS2MaxBetType = 2899;
        private const int LTMinBetType = 1021;
        private const int LTMaxBetType = 1049;

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public static bool IsValidBettype(string productCode, int bettype)
        {
            if (string.IsNullOrEmpty(productCode))
            {
                return false;
            }

            var code = productCode.ToUpperInvariant();
            switch (code)
            {
                case Product.SB:
                    return bettype <= SBMinBetType ||
                        IntegerHelper.IsInRange(bettype, SBMinOfMaxBettype, SBMaxBetType) ||
                        IntegerHelper.IsInRange(bettype, SBMinSportingSolutionBetType, SBMaxSportingSolutionBetType);

                case Product.BA:
                    return bettype <= SBMinBetType || IntegerHelper.IsInRange(bettype, SBMinOfMaxBettype, SBMaxBetType);

                case Product.RB:
                    return IntegerHelper.IsInRange(bettype, RBMinBetType, RBMaxBetType);

                case Product.NG:
                    return IntegerHelper.IsInRange(bettype, NGMinBetType, NGMaxBetType);

                case Product.VS:
                    return IntegerHelper.IsInRange(bettype, VSMinBetType, VSMaxBetType);

                case Product.LC:
                    return IntegerHelper.IsInRange(bettype, LCMinBetType, LCMaxBetType);

                case Product.BG:
                    return IntegerHelper.IsInRange(bettype, BGMinBetType, BGMaxBetType);

                case Product.CS:
                    return IntegerHelper.IsInRange(bettype, CSMinBetType, CSMaxBetType) ||
                        IntegerHelper.IsInRange(bettype, CSNewMinBetType, CSNewMaxBetType);

                case Product.KN:
                    return IntegerHelper.IsInRange(bettype, KenoMinBetType, KenoMaxBetType);

                case Product.MGCLC:
                case Product.MGCRNG:
                    return IntegerHelper.IsInRange(bettype, MicrogamingMinBetType, MicrogamingMaxBetType);

                case Product.VB:
                    return IntegerHelper.IsInRange(bettype, VoidbridgeMinBetType, VoidbridgeMaxBetType);

                case Product.AB:
                    return IntegerHelper.IsInRange(bettype, AllBetMinBetType, AllBetMaxBetType);

                case Product.FW:
                    return IntegerHelper.IsInRange(bettype, FWMinBetType, FWMaxBetType);

                case Product.VS2:
                    return IntegerHelper.IsInRange(bettype, VS2MinBetType, VS2MaxBetType);

                case Product.LT:
                    return IntegerHelper.IsInRange(bettype, LTMinBetType, LTMaxBetType);

                default:
                    return false;
            }
        }
    }
}