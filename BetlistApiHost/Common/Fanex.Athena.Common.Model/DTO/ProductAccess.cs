using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Fanex.Athena.Common.Constant;

namespace Fanex.Athena.Common.Model.DTO
{
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
                return ProductList.Any(i => i.ProductCode == ProductCodes.SB);
            }
        }

        public bool IsAllowBA
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == ProductCodes.BA);
            }
        }

        public bool IsAllowNG
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == ProductCodes.NG);
            }
        }

        public bool IsAllowVS
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == ProductCodes.VS);
            }
        }

        public bool IsAllowCS
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == ProductCodes.CS);
            }
        }

        public bool IsAllowKN
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == ProductCodes.KN);
            }
        }

        public bool IsAllowBT
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == ProductCodes.BT);
            }
        }

        public bool IsAllowAB
        {
            get
            {
                return ProductList.Any(p => p.ProductCode == ProductCodes.AB);
            }
        }

        public bool IsAllow(int productId)
        {
            return ProductList.Any(p => p.ProductID == productId);
        }

        public bool IsAllow(string productCode)
        {
            return ProductList.Any(p => p.ProductCode == productCode);
        }
    }
}