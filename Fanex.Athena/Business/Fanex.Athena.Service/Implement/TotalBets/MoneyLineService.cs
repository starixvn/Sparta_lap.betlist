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

        protected readonly MoneyLineDataAccess MoneyLineDataAccess;

        #endregion Fields

        #region Constructor

        public MoneyLineService()
        {
            MoneyLineDataAccess = new MoneyLineDataAccess();
        }

        #endregion Constructor

        public IList<MoneyLine> GetMoneyLineDataList(MoneyLineMetaModel metaModel)
        {
            return MoneyLineDataAccess.GetMoneyLineDataList(metaModel);
        }
    }
}