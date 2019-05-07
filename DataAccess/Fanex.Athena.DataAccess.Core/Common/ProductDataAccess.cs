// -----------------------------------------------------------------------
// <copyright file="ProductDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using Extension.Helper;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common;
    using Resources;

    public class ProductDataAccess : BaseDataAccess, IProductDataAccess
    {
        public IList<Product> GetProductAccessList(int licenseeId, bool isActiveCustomerSummaryReport = false)
        {
            var uspName = isActiveCustomerSummaryReport ? "Lrf_LicenseeProduct3rdSel_active" : "Lrf_LicenseeProduct3rdSel";

            // All licensees have product Sb
            var allowedProducts = new List<Product> { new Product { IsOfferedSingleWallet = true, ProductCode = "SB", ProductName = ViewResources.Sportsbook } };

            using (var db = this.GetObjectDb(uspName))
            {
                db.SetParameterValue("licenseeid", licenseeId);

                using (var reader = db.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        allowedProducts.Add(new Product
                        {
                            ProductID = Converter.ToInt(reader["ProductID"]),
                            ProductName = DataHelper.GetProductName(Converter.ToString2(reader["productprefix"])),
                            ProductCode = Converter.ToString2(reader["productprefix"]),
                            AllowCommission = Converter.ToBool(reader["comm"]),
                            AllowWinloss = Converter.ToBool(reader["winloss"]),
                            IsOfferedSingleWallet = Converter.ToBool(reader["IsOfferedSingleWallet"])
                        });
                    }
                }

                return allowedProducts;
            }
        }
    }
}