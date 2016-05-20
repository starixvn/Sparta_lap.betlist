// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TopCustomerService.cs" company="Nexcel">
// The top customer service.
// </copyright>
// <summary>
//   The top customer service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Warehouse
{
    using System.Collections.Generic;
    using System.Data;
    using System.Linq;
    using Fanex.Athena.DataAccess.Warehouse;
    using Fanex.Athena.DataAccess.Warehouse.Interfaces;
    using Fanex.Athena.Model.Warehouse.DTO;
    using Fanex.Athena.Model.Warehouse.MetaModel;
    using Fanex.Athena.Service.Warehouse.Interfaces;

    /// <summary>
    ///     The top customer service.
    /// </summary>
    public class TopCustomerService : ITopCustomerService
    {
        #region Constants and Fields

        public const int CacheTimeoutMinute = 1;

        private readonly ITopCustomerDataAccess _topCustDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        public TopCustomerService()
        {
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
            return model.IsValid
                       ? this._topCustDataAccess.GetDataForExcel(model)
                       : new DataTable();
        }

        /// <summary>
        /// Get top customer list.
        /// </summary>
        /// <param name="model">The metal model.</param>
        /// <param name="totalValues">The total dictionary result.</param>
        /// <returns>The top customer list.</returns>
        public IList<TopCustomer> GetTopCustomerList(TopCustomerMetaModel model, ref Dictionary<string, decimal> totalValues)
        {
            var dataList = model.IsValid
                               ? this._topCustDataAccess.GetDataList(model)
                               : new List<TopCustomer>();
            totalValues = this.GetTopCustomerTotalInfo(dataList);
            return dataList;
        }

        /// <summary>
        /// Calculate summary data.
        /// </summary>
        /// <param name="dataList">The data list.</param>
        /// <returns>The summary data.</returns>
        public Dictionary<string, decimal> GetTopCustomerTotalInfo(IList<TopCustomer> dataList)
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
                totalValues["betcount"] = dataList.Sum(x => x.BetCount);
                totalValues["turnover"] = dataList.Sum(x => x.Turnover);
                totalValues["memberwinloss"] = dataList.Sum(x => x.MemberWinLoss);
                totalValues["memberwinlosscomm"] = dataList.Sum(x => x.MemberWinLossCom);
                totalValues["licenseewinloss"] = dataList.Sum(x => x.LicenseeWinLoss);
                totalValues["bacomm"] = dataList.Sum(x => x.BAComm);

                totalValues["totalrows"] = dataList.Count;
            }

            return totalValues;
        }

        #endregion Public Methods and Operators
    }
}