// -----------------------------------------------------------------------
// <copyright file="ProductDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Extensions;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Logging;
    using Resources;

    public class ProductDataAccess : BaseDataAccess, IProductDataAccess
    {
        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<Product> GetProductAccessList(int licenseeId, bool isActiveCustomerSummaryReport = false)
        {
            var uspName = isActiveCustomerSummaryReport ? "Lrf_LicenseeProduct3rdSel_active" : "Lrf_LicenseeProduct3rdSel";

            // All licensees have product Sb
            var allowedProducts = new List<Product> { new Product { IsOfferedSingleWallet = true, ProductCode = "SB", ProductName = ViewResources.Sportsbook } };

            try
            {
                using (var db = this.GetObjectDb(uspName))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, licenseeId);

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
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName);
            }
        }
    }
}