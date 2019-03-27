// -----------------------------------------------------------------------
// <copyright file="IExChangeRateDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Common.Interfaces
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Athena.Model.Core.DTO.Common;

    public interface IExChangeRateDataAccess
    {
        Task<ExChangeRate> GetExChangeRate(string currenyCode, DateTime datetime);
    }
}