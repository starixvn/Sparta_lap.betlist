// --------------------------------------------------------------------------
// <copyright file="IForecastDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Forecast.Interfaces
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Forecast.DTO.ForecastHdou;
    using Model.Forecast.MetaModel;

    public interface IForecastDataAccess
    {
        Task<IList<ForecastHandicapOverUnder>> GetForecastHandicapOverUnder(ForecastHandicapOverUnderMetaModel metaModel);
    }
}