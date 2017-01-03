// -----------------------------------------------------------------------
// <copyright file="MoneyLineService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet
{
    using System.Collections.Generic;
    using DataAccess.TotalBet;
    using DataAccess.TotalBet.Interfaces;
    using Interfaces;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public class MoneyLineService : IMoneyLineService
    {
        private readonly IMoneyLineDataAccess _moneyLineDataAccess;

        public MoneyLineService()
        {
            _moneyLineDataAccess = new MoneyLineDataAccess();
        }

        public MoneyLineService(IMoneyLineDataAccess moneyLineDataAccess)
        {
            _moneyLineDataAccess = moneyLineDataAccess;
        }

        public IList<MoneyLine> GetMoneyLineDataList(MoneyLineMetaModel metaModel, out MoneyLine totalData)
        {
            return _moneyLineDataAccess.GetMoneyLineDataList(metaModel, out totalData);
        }
    }
}