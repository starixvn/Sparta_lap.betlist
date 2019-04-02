// --------------------------------------------------------------------------
// <copyright file="IOddEvenDrawNoDrawDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.TotalBet.Interfaces
{
    using System.Threading.Tasks;
    using Model.Core.ResultModel;
    using Model.TotalBet.DTO;
    using Model.TotalBet.MetaModel;

    public interface IOddEvenDrawNoDrawDataAccess
    {
        Task<ReportResultModel<OddEvenDrawNoDraw>> GetOddEvenDrawNoDrawDataList(OddEvenDrawNoDrawMetaModel metaModel);
    }
}