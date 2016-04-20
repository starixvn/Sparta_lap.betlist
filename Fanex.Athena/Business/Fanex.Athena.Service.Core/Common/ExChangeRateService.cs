// -----------------------------------------------------------------------
// <copyright file="ExChangeRateService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common
{
    using System;
    using Fanex.Athena.DataAccess.Core.Common;
    using Fanex.Athena.DataAccess.Core.Common.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Common;
    using Fanex.Athena.Service.Core.Common.Interfaces;

    /// <summary>
    /// The Exchange rate service class.
    /// </summary>
    public class ExChangeRateService : IExChangeRateService
    {
        private readonly IExChangeRateDataAccess _exchangeRateDataAccess;

        public ExChangeRateService()
        {
            this._exchangeRateDataAccess = new ExChangeRateDataAccess();
        }

        public ExChangeRate GetExChangeRate(string currency, DateTime datetime)
        {
            return _exchangeRateDataAccess.GetExChangeRate(currency, datetime);
        }
    }
}