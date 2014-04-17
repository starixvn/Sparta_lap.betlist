// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopCustomerService.cs" company="Nexcel">
// The top customer service.
// </copyright>
// <summary>
//   The top customer service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using Fanex.Athena.DataAccess.Implement.Warehouse;
    using Fanex.Athena.DataAccess.Interface.Warehouse;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;
    using Fanex.Athena.Service.Exception;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Warehouse;
    using Fanex.Logging;

    /// <summary>
    ///     The top customer service.
    /// </summary>
    public class TopCustomerService : ITopCustomerService
    {
        #region Constants and Fields

        public const int CacheTimeoutMinute = 1;

        private readonly IBalanceUpService _balanceUpService;

        private readonly ITopCustomerDataAccess _topCustDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        public TopCustomerService()
        {
            this._balanceUpService = new BalanceUpService();
            this._topCustDataAccess = new TopCustomerDataAccess();
        }

        #endregion Constructors and Destructors

        #region Public Methods and Operators

        /// <summary>
        /// Get data for excel.
        /// </summary>
        /// <param name="model">The metal model.</param>
        /// <returns>The data table.</returns>
        public DataTable GetDataForExcel(TopCustomerMetaModel model)
        {
            try
            {
                return model.IsValid
                           ? this._topCustDataAccess.GetDataForExcel(model)
                           : new DataTable();
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw new ServiceException(ex.Message, "WarehouseTopCustomerService");
            }
        }

        /// <summary>
        /// Get top customer list.
        /// </summary>
        /// <param name="model">The metal model.</param>
        /// <param name="totalValues">The total dictionary result.</param>
        /// <returns>The top customer list.</returns>
        public IList<TopCustomer> GetTopCustomerList(TopCustomerMetaModel model, ref Dictionary<string, decimal> totalValues)
        {
            try
            {
                var dataList = model.IsValid
                                   ? this._topCustDataAccess.GetDataList(model)
                                   : new List<TopCustomer>();
                totalValues = this.GetTopCustomerTotalInfo(dataList);
                return dataList;
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw new ServiceException(ex.Message, "WarehouseTopCustomerService");
            }
        }

        /// <summary>
        /// Calculate summary data.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The summary data.</returns>
        public Dictionary<string, decimal> GetTopCustomerTotalInfo(IList<TopCustomer> dataList)
        {
            try
            {
                var totalValues = new Dictionary<string, decimal>
                    {
                        { "betcount", 0 },
                        { "turnover", 0 },
                        { "memberwinloss", 0 },
                        { "memberwinlosscomm", 0 },
                        { "licenseewinloss", 0 },
                        { "bacomm", 0 },
                        { "totalrows", 0 }
                    };

                if (dataList != null)
                {
                    foreach (var entity in dataList)
                    {
                        totalValues["betcount"] += entity.BetCount;
                        totalValues["turnover"] += entity.Turnover;
                        totalValues["memberwinloss"] += entity.MemberWinLoss;
                        totalValues["memberwinlosscomm"] += entity.MemberWinLossCom;
                        totalValues["licenseewinloss"] += entity.LicenseeWinLoss;
                        totalValues["bacomm"] += entity.BaComm;
                    }

                    totalValues["totalrows"] = dataList.Count;
                }

                return totalValues;
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw new ServiceException(ex.Message, "WarehouseTopCustomerService");
            }
        }

        #endregion Public Methods and Operators
    }
}