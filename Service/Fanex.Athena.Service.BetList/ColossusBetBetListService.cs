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

    public class ColossusBetBetListService : IColossusBetBetListService
    {
        private readonly IColossusBetBetListDataAccess _colossusBetBetListDataAccess;

        public ColossusBetBetListService()
        {
            _colossusBetBetListDataAccess = new ColossusBetBetListDataAccess();
        }

        public ColossusBetDetailViewModel GetData(ColossusBetDetailMetaModel metaModel)
        {
            return this._colossusBetBetListDataAccess.GetData(metaModel);
        }
    }
}