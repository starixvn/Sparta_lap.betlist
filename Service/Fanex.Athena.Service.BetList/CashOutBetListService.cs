// -----------------------------------------------------------------------
// <copyright file="ColossusBetBetListService.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.BetList
{
    using Fanex.Athena.DataAccess.BetList;
    using Fanex.Athena.DataAccess.BetList.Interfaces;
    using Fanex.Athena.Model.BetList.MetaModel;
    using Interfaces;
    using Model.BetList.ViewModel;

    public class CashOutBetListService : ICashOutBetListService
    {
        private readonly ICashOutBetListDataAccess _colossusBetBetListDataAccess;

        public CashOutBetListService()
        {
            _colossusBetBetListDataAccess = new CashOutBetListDataAccess();
        }

        public CashOutBetListService(ICashOutBetListDataAccess colossusBetBetListDataAccess)
        {
            _colossusBetBetListDataAccess = colossusBetBetListDataAccess;
        }

        public CashOutBetListDetailViewModel GetDetail(CashOutBetListDetailMetaModel metaModel)
        {
            return this._colossusBetBetListDataAccess.GetDetail(metaModel);
        }
    }
}