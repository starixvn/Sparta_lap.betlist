// --------------------------------------------------------------------------
// <copyright file="IForecastDataAccess.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Forecast.Interfaces
{
    using System.Threading.Tasks;
    using Model.Forecast.DTO.NewForecastHdpOu;
    using Model.Forecast.MetaModel;
    using Model.Forecast.ViewModel;

    public interface INewForecastDataAccess
    {
        Task<NewForecastHdpOuData> GetNewForecastData(NewForecastHandicapOverUnderMetaModel metaModel, NewForecastHdpOuMatchInfo matchInfo);
    }
}