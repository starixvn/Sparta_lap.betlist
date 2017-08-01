// --------------------------------------------------------------------------
// <copyright file="ProductAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;

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
                return ProductList.Any(i => i.ProductCode == Product.SB);
            }
        }

        public bool IsAllowBA
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.BA);
            }
        }

        public bool IsAllowNG
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.NG);
            }
        }

        public bool IsAllowRB
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.RB);
            }
        }

        public bool IsAllowLC
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.LC);
            }
        }

        public bool IsAllowVS
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.VS);
            }
        }

        public bool IsAllowCS
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.CS);
            }
        }

        public bool IsAllowBG
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.BG);
            }
        }

        public bool IsAllowKN
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.KN);
            }
        }

        public bool IsAllowBT
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.BT);
            }
        }

        public bool IsAllowLT
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == Product.LT);
            }
        }

        public bool IsAllow(int productId)
        {
            return ProductList.Any(p => p.ProductID == productId);
        }
    }
}