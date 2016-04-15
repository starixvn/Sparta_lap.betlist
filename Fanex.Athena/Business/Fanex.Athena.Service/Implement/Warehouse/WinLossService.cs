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
    using System.Linq;
    using Fanex.Athena.DataAccess.Implement.Warehouse;
    using Fanex.Athena.DataAccess.Interface.Warehouse;
    using Fanex.Athena.Models.DTO.Warehouse;
    using Fanex.Athena.Models.MetaModel.Warehouse;
    using Fanex.Athena.Service.Core.Exception;
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
        ///     The get data list.
        /// </summary>
        /// <param name="model">
        ///     The model.
        /// </param>        
        /// <param name="totalValues">
        ///     The total values.
        /// </param>
        /// The data list.
        /// <returns>
        /// The data list.
        /// </returns>
        public IList<WinLoss> GetDataList(WinLossMetaModel model, ref IDictionary<string, decimal> totalValues)
        {
            var dataList = model.IsValid
                               ? this._winLossDataAccess.GetDataList(model)
                               : new List<WinLoss>();
            totalValues = this.GetWinlossTotalInfo(dataList);

            return dataList.OrderBy(x => x.GroupByString).ToList();
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
                    totalValues["bacomm"] += entity.BAComm;
                }

                totalValues["totalrows"] = dataList.Count;
            }

            return totalValues;
        }

        #endregion Public Methods and Operators
    }
}