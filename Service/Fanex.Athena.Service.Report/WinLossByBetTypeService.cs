// -----------------------------------------------------------------------
// <copyright file="WinLossByBetTypeService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.Extension.Utility;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Core.Configuration;
    using Fanex.Athena.Service.Core.Configuration.Interfaces;
    using Fanex.Athena.Service.Report.Interfaces;

    public class WinLossByBetTypeService : IWinLossByBetTypeServive
    {
        private readonly IBalanceUpService _balanceUpService;

        private readonly WinLossByBetTypeDataAccess _winLossByBetTypeDataAccess;

        public WinLossByBetTypeService()
        {
            this._balanceUpService = new BalanceUpService();
            this._winLossByBetTypeDataAccess = new WinLossByBetTypeDataAccess(this._balanceUpService.BalanceUpInfo);
        }

        public SortableList<WinLossByBetType> GetDataList(WinLossByBetTypeMetaModel metaModel)
        {
            return metaModel.IsValid
                       ? _winLossByBetTypeDataAccess.GetDataList(metaModel)
                       : new SortableList<WinLossByBetType>();
        }
    }
}