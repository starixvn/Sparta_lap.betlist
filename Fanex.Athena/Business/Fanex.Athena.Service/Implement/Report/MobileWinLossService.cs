// --------------------------------------------------------------------------------------------------------------------
// <copyright file="MobileWinLossService.cs" company="Nexcel">
// The mobile win loss service.
// </copyright>
// <summary>
//   The mobile win loss service.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Report
{
    using System.Collections.Generic;
    using Fanex.Athena.DataAccess.Implement.Report;
    using Fanex.Athena.Models.DTO.Report;
    using Fanex.Athena.Models.MetaModel.Report;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.Athena.Service.Interface.Report;

    public class MobileWinLossService : IMobileWinLossService
    {
        private readonly IBalanceUpService _balanceUpService;

        private readonly MobileWinLossDataAccess _mobileWinLossDataAccess;

        public MobileWinLossService()
        {
            this._balanceUpService = new BalanceUpService();
            this._mobileWinLossDataAccess = new MobileWinLossDataAccess(this._balanceUpService.BalanceUpInfo);
        }

        public IList<MobileWinLossSummary> GetSummaryData(MobileWinLossMetaModel metaModel, out MobileWinLossSummary totalData)
        {
            if (metaModel.IsValid)
            {
                return this._mobileWinLossDataAccess.GetSummaryData(metaModel, out totalData);
            }
            else
            {
                totalData = new MobileWinLossSummary();
                return new List<MobileWinLossSummary>();
            }
        }

        public IList<MobileWinLossMember> GetMemberData(MobileWinLossMetaModel metaModel, out Dictionary<string, MobileWinLossMember> summaryTotal)
        {
            if (metaModel.IsValid)
            {
                return this._mobileWinLossDataAccess.GetMemberData(metaModel, out summaryTotal);
            }
            else
            {
                summaryTotal = new Dictionary<string, MobileWinLossMember>();
                return new List<MobileWinLossMember>();
            }
        }
    }
}