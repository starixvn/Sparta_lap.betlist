// -----------------------------------------------------------------------
// <copyright file="IMoneyLineService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.TotalBet.Interfaces
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public interface IMoneyLineService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewd.")]
        IList<MoneyLine> GetMoneyLineDataList(MoneyLineMetaModel metaModel, out MoneyLine totalData);
    }
}