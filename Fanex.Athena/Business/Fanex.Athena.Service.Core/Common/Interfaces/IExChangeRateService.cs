// -----------------------------------------------------------------------
// <copyright file="IExChangeRateService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Common.Interfaces
{
    using System;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IExChangeRateService
    {
        ExChangeRate GetExChangeRate(string currency, DateTime datetime);
    }
}