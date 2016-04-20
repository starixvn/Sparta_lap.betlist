// -----------------------------------------------------------------------
// <copyright file="BalanceUpDataAccess.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Configuration
{
    using System;
    using System.Data;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.Logging;

    /// <summary>
    /// Balance data access.
    /// </summary>
    public class MoveBetDataAccess : BaseDataAccess, IMoveBetDataAccess
    {
        public MoveBet GetMoveBet()
        {
            const string UspName = "Lrf_ShowNotificationWhenProcessingData";
            try
            {
                using (var db = new ObjectDb(UspName))
                {
                    var moveBet = new MoveBet();
                    db.AddOutParameter("AvailableWinlostDate", DbType.DateTime, 10);
                    db.AddOutParameter("EstimatedTime", DbType.DateTime, 10);
                    db.ExecuteNonQuery();
                    moveBet.AvailableWinlostDate = Converter.ToDateTime(db.GetParameterValue("AvailableWinlostDate"));
                    var estimatedTime = db.GetParameterValue("EstimatedTime");
                    moveBet.EstimatedTime = estimatedTime == null || estimatedTime == DBNull.Value ? (DateTime?)null : Converter.ToDateTime(estimatedTime);
                    return moveBet;
                }
            }
            catch (ObjectDbException sqlException)
            {
                LogService.Error(sqlException.Message, sqlException);
                throw new DataAccessException(sqlException.Message, UspName);
            }
        }
    }
}