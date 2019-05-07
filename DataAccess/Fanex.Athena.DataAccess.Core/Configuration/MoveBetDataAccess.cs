// -----------------------------------------------------------------------
// <copyright file="BalanceUpDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Configuration
{
    using System;
    using System.Threading.Tasks;
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Logging;

    public class MoveBetDataAccess : BaseDataAccess, IMoveBetDataAccess
    {
        private readonly ILogger _logger;

        [CLSCompliant(false)]
        public MoveBetDataAccess(
            ILogger logger)
        {
            _logger = logger;
        }

        public async Task<MoveBetNotification> GetMoveBetNotificationAsync()
        {
            const string UspName = "Acc_ShowNotificationWhenProcessingData";
            var moveBet = new MoveBetNotification();
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    using (var reader = await db.ExecuteReaderAsync().ConfigureAwait(false))
                    {
                        if (reader.Read())
                        {
                            moveBet.ContentByLanguage.Add("en-US", reader.ToString("Content_EN"));
                            moveBet.ContentByLanguage.Add("zh-CN", reader.ToString("Content_CS"));
                            moveBet.ContentByLanguage.Add("zh-TW", reader.ToString("Content_CH"));
                            moveBet.ContentByLanguage.Add("zh-Hans", reader.ToString("Content_CN"));
                            moveBet.ContentByLanguage.Add("th-TH", reader.ToString("Content_TH"));
                            moveBet.ContentByLanguage.Add("ja-JP", reader.ToString("Content_JP"));
                            moveBet.ContentByLanguage.Add("vi-VN", reader.ToString("Content_VI"));
                        }
                    }
                }
            }
            catch (ObjectDbException sqlException)
            {
                _logger.Error("MoveBetNotification: " + sqlException.Message, sqlException);
            }

            return moveBet;
        }
    }
}