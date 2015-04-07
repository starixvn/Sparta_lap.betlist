// -----------------------------------------------------------------------
// <copyright file="ProductDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Implement.Common
{
    using System.Collections.Generic;
    using System.Data;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.Athena.DataAccess.Exception;
    using Fanex.Athena.DataAccess.Implement.Base;
    using Fanex.Athena.DataAccess.Interface.Common;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Models.ViewModel;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    ///   The product data access.
    /// </summary>
    public class ProductDataAccess : BaseDataAccess, IProductDataAccess
    {
        #region Public Methods and Operators

        /// <summary>
        ///   The get product access list.
        /// </summary>
        /// <param name="licenseeId"> The licensee id. </param>
        /// <returns> The result value.</returns>
        [SuppressMessage("StyleCop.CSharp.Nexcel.NexcelCustomRules", "SP2101:MethodMustNotContainMoreLinesThan",
            Justification = "Reviewed. Suppression is OK here.")]
        public IList<Product> GetProductAccessList(int licenseeId)
        {
            var uspName = "Lrf_LicenseeProduct3rdSel";

            // All licensees have product Sb
            var allowedProducts = new List<Product>() { new Product { ProductCode = "SB", ProductName = ViewResources.Sportsbook } };

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("licenseeid", DbType.Int32, licenseeId);

                    using (var reader = db.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            allowedProducts.Add(new Product
                                             {
                                                 ProductID = Converter.ToInt(reader["ProductID"]),
                                                 ProductName = GetProductName(Converter.ToString2(reader["productprefix"])),
                                                 ProductCode = Converter.ToString2(reader["productprefix"]),
                                                 AllowCommission = Converter.ToBool(reader["comm"]),
                                                 AllowWinloss = Converter.ToBool(reader["winloss"])
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

        /// <summary>
        /// Allow Tote currency.
        /// </summary>
        /// <param name="custId">Customer ID value.</param>
        /// <returns>The result value.</returns>
        public bool IsAllowedSaToteCurrency(int custId)
        {
            var uspName = "Lrf_CheckHRAllowedCurrency";

            try
            {
                using (var db = new ObjectDb(uspName))
                {
                    db.AddInParameter("custid", DbType.Int32, custId);

                    db.AddOutParameter("allowed", DbType.Boolean, sizeof(bool));

                    db.ExecuteNonQuery();

                    return Converter.ToBool(db.GetParameterValue("allowed"));
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, uspName, new { });
            }
        }

        /// <summary>
        /// Get Product Name.
        /// </summary>
        /// <param name="productCode">Product code.</param>
        /// <returns>Product name.</returns>
        private static string GetProductName(string productCode)
        {
            switch (productCode)
            {
                case Product.SB:
                    return ViewResources.Sportsbook;
                case Product.BA:
                    return ViewResources.BA;
                case Product.RB:
                  return ViewResources.Racing;
                case Product.NG:
                  return ViewResources.NumberGame;
                case Product.VS:
                  return ViewResources.VirtualSports;
                case Product.LC:
                  return ViewResources.LiveCasino;
                case Product.BG:
                  return ViewResources.Bingo;
                case Product.CS:
                  return ViewResources.RNGCasino;
                case Product.KN:
                  return ViewResources.Keno;
                case Product.PTCS:
                  return ViewResources.PlaytechCasino;
                case Product.AGC:
                  return ViewResources.AGCasino;
                case Product.VB:
                    return ViewResources.Voidbridge;
            }

            return productCode;
        }

        #endregion Public Methods and Operators
    }
}