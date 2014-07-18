// --------------------------------------------------------------------------
// <copyright file="ProductAccess.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Models.DTO.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using Fanex.Athena.Extension.Utility;

    /// <summary>
    /// ProductAccess class.
    /// </summary>
    [Serializable]
    public class ProductAccess
    {
        public ProductAccess(IList<Product> productList)
        {
            ProductList = new ReadOnlyCollection<Product>(productList);
        }

        public ReadOnlyCollection<Product> ProductList { get; private set; }

        public bool IsAllowSB
        {
            get
            {
                return ProductList.Any(i => i.ProductCode == Product.Sb);
            }
        }

        public bool IsAllowBa
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.Ba);
            }
        }

        public bool IsAllowNg
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.Ng);
            }
        }

        public bool IsAllowRb
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.Rb);
            }
        }

        public bool IsAllowLc
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.Lc);
            }
        }

        public bool IsAllowVs
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.Vs);
            }
        }

        public bool IsAllowCs
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.Cs);
            }
        }

        public bool IsAllowBg
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.Bg);
            }
        }

        public bool IsAllowKN
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.Kn);
            }
        }

        public bool CanUseRacing
        {
            get
            {
                return AppSettings.EnableRacing
                       && ProductList.Any(p => p.ProductCode == Product.Rb);
            }
        }

        public bool CanUseHorseRacingUk
        {
            get { return CanUseRacing && AppSettings.EnableRacingHrUk; }
        }

        public bool CanUseHorseRacingSa
        {
            get { return CanUseRacing && AppSettings.EnableRacingHrSa; }
        }

        public bool CanUseHorseRacingAust
        {
            get { return CanUseRacing && AppSettings.EnableRacingHrAust; }
        }

        public bool CanUseGreyhoundUk
        {
            get { return CanUseRacing && AppSettings.EnableRacingGreyhoundUk; }
        }

        public bool CanUseGreyhoundAust
        {
            get { return CanUseRacing && AppSettings.EnableRacingGreyhoundAust; }
        }

        public bool CanUseHarnessAust
        {
            get { return CanUseRacing && AppSettings.EnableRacingHarnessAust; }
        }

        public bool CanUseNg
        {
            get
            {
                return CanUseProduct(Product.Ng);
            }
        }

        public bool CanUseBa
        {
            get
            {
                return CanUseProduct(Product.Ba);
            }
        }

        public bool CanUseLc
        {
            get
            {
                return CanUseProduct(Product.Lc);
            }
        }

        public bool CanUseProduct(string productCode)
        {
            return ProductList.Any(p => p.ProductCode == productCode);
        }
    }
}