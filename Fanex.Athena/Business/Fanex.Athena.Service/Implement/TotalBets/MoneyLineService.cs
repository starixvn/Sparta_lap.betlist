// -----------------------------------------------------------------------
// <copyright file="MoneyLineService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.TotalBets
{
    using System.Collections.Generic;
    using DataAccess.Implement.TotalBets;
    using Interface.TotalBets;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

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