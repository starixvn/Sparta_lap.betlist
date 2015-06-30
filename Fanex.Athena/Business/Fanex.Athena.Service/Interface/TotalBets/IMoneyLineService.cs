// -----------------------------------------------------------------------
// <copyright file="IMoneyLineService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.TotalBets
{
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using Models.DTO.TotalBets;
    using Models.MetaModel.TotalBets;

    public interface IMoneyLineService
    {
        [SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", Justification = "Reviewd.")]
        IList<MoneyLine> GetMoneyLineDataList(MoneyLineMetaModel metaModel, out MoneyLine totalData);
    }
}