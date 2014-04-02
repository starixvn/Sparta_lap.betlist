// --------------------------------------------------------------------------------------------------------------------
// <copyright file="WinLossService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// <summary>
//   The win loss service.
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
    using Fanex.Athena.Service.Interface.Warehouse;
    using Fanex.Logging;

    /// <summary>
    ///     The win loss service.
    /// </summary>
    public class WinLossService : IWinLossService
    {
        #region Constants and Fields

        /// <summary>
        ///     The _win loss data access.
        /// </summary>
        private readonly IWinLossDataAccess _winLossDataAccess;

        #endregion Constants and Fields

        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="WinLossService" /> class.
        /// </summary>
        public WinLossService()
        {
            this._winLossDataAccess = new WinLossDataAccess();
        }

        #endregion Constructors and Destructors

        #region Public Methods and Operators

        /// <summary>
        ///     The get data for excel.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="licenseeList">
        ///     The licensee list.
        /// </param>
        /// <returns>
        /// The data table.
        /// </returns>
        public DataTable GetDataForExcel(WinLossMetaModel model)
        {
            try
            {
                return model.IsValid
                           ? this._winLossDataAccess.GetDataForExcel(model)
                           : new DataTable();
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw new ServiceException(ex.Message, "WarehouseWinLossService");
            }
        }

        /// <summary>
        ///     The get data list.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>
        /// <param name="licenseeList">
        ///     The licensee list.
        /// </param>
        /// <param name="totalValues">
        ///     The total values.
        /// </param>
        /// The data list.
        /// <returns>
        /// The data list.
        /// </returns>
        public IList<WinLoss> GetDataList(
            WinLossMetaModel model, ref IDictionary<string, decimal> totalValues)
        {
            try
            {
                var dataList = model.IsValid
                                   ? this._winLossDataAccess.GetDataList(model)
                                   : new List<WinLoss>();
                totalValues = this.GetWinlossTotalInfo(dataList);

                return dataList;
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw new ServiceException(ex.Message, "WarehouseWinLossService");
            }
        }

        /// <summary>
        ///     The get win loss total info.
        /// </summary>
        /// <param name="dataList">
        ///     The data list.
        /// </param>
        /// <returns>
        /// The Total Info.
        /// </returns>
        public Dictionary<string, decimal> GetWinlossTotalInfo(IList<WinLoss> dataList)
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
                        totalValues["licenseewinloss"] += entity.LicenseeWinloss;
                        totalValues["bacomm"] += entity.BaComm;
                    }

                    totalValues["totalrows"] = dataList.Count;
                }

                return totalValues;
            }
            catch (System.Exception ex)
            {
                LogService.Error(ex.Message, ex);
                throw new ServiceException(ex.Message, "WarehouseWinLossService");
            }
        }

        #endregion Public Methods and Operators
    }
}