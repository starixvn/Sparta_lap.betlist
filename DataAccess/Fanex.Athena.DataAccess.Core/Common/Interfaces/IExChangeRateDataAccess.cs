// -----------------------------------------------------------------------
// <copyright file="IExChangeRateDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
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