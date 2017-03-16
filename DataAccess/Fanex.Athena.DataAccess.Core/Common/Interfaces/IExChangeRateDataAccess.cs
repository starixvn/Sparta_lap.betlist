// -----------------------------------------------------------------------
// <copyright file="IExChangeRateDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System;
    using Fanex.Athena.Model.Core.DTO.Common;

    /// <summary>
    /// Exchange rate data access interface.
    /// </summary>
    public interface IExChangeRateDataAccess
    {
        ExChangeRate GetExChangeRate(string currenyCode, DateTime datetime);
    }
}