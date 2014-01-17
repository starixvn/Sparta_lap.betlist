// -----------------------------------------------------------------------
// <copyright file="IExChangeRateService.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Common
{
    using System;
    using Fanex.Athena.Models.DTO.Common;

    public interface IExChangeRateService
    {
        ExChangeRate GetExChangeRate(string currency, DateTime datetime);
    }
}