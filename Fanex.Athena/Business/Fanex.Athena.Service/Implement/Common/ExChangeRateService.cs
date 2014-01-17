// -----------------------------------------------------------------------
// <copyright file="ExChangeRateService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Implement.Common
{
    using System;
    using Fanex.Athena.DataAccess.Implement.Common;
    using Fanex.Athena.DataAccess.Interface.Common;
    using Fanex.Athena.Models.DTO.Common;
    using Fanex.Athena.Service.Interface.Common;

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