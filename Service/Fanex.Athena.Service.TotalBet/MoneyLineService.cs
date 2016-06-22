// -----------------------------------------------------------------------
// <copyright file="MoneyLineService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    /// <summary>
    /// Money live service class.
    /// </summary>
    public class MoneyLineService : IMoneyLineService
    {
        #region Fields

        private readonly MoneyLineDataAccess _moneyLineDataAccess;

        #endregion Fields

        #region Constructor

        public MoneyLineService()
        {
            _moneyLineDataAccess = new MoneyLineDataAccess();
        }

        #endregion Constructor

        public IList<MoneyLine> GetMoneyLineDataList(MoneyLineMetaModel metaModel, out MoneyLine totalData)
        {
            return _moneyLineDataAccess.GetMoneyLineDataList(metaModel, out totalData);
        }
    }
}