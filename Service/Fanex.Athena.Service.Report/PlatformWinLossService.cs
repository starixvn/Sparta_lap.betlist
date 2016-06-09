// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlatformWinLossService.cs" company="Nexcel">
// The mobile win loss service.
// </copyright>
// <summary>
//   The mobile win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Report
{
    using System.Collections.Generic;
    using Core.Configuration;
    using Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Report;
    using Fanex.Athena.Model.Report.DTO;
    using Fanex.Athena.Model.Report.MetaModel;
    using Fanex.Athena.Service.Report.Interfaces;

    public class PlatformWinLossService : IPlatformWinLossService
    {
        private readonly IBalanceUpService _balanceUpService;

        private readonly PlatformWinLossDataAccess _mobileWinLossDataAccess;

        public PlatformWinLossService()
        {
            this._balanceUpService = new BalanceUpService();
            this._mobileWinLossDataAccess = new PlatformWinLossDataAccess(this._balanceUpService.BalanceUpInfo);
        }

        public IList<PlatformWinLossSummary> GetSummaryData(PlatformWinLossMetaModel metaModel, out PlatformWinLossSummary totalData)
        {
            if (metaModel.IsValid)
            {
                return this._mobileWinLossDataAccess.GetSummaryData(metaModel, out totalData);
            }
            else
            {
                totalData = new PlatformWinLossSummary();
                return new List<PlatformWinLossSummary>();
            }
        }

        public IList<PlatformWinLossMember> GetMemberData(PlatformWinLossMetaModel metaModel, out PlatformWinLossMember totalData)
        {
            if (metaModel.IsValid)
            {
                return this._mobileWinLossDataAccess.GetMemberData(metaModel, out totalData);
            }
            else
            {
                totalData = new PlatformWinLossMember();
                return new List<PlatformWinLossMember>();
            }
        }
    }
}