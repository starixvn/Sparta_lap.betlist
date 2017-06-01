// -----------------------------------------------------------------------
// <copyright file="BalanceUpDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Configuration
{
    using Fanex.Athena.DataAccess.Core;
    using Fanex.Athena.DataAccess.Core.Configuration.Interfaces;
    using Fanex.Athena.DataAccess.Core.Exception;
    using Fanex.Athena.Model.Core.DTO.Configuration;
    using Fanex.Common.Extensions;
    using Fanex.Data;
    using Fanex.Logging;

    public class MoveBetDataAccess : BaseDataAccess, IMoveBetDataAccess
    {
        public MoveBetNotification GetMoveBetNotification()
        {
            const string UspName = "Acc_ShowNotificationWhenProcessingData";
            try
            {
                using (var db = this.GetObjectDb(UspName))
                {
                    var moveBet = new MoveBetNotification();
                    using (var reader = db.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            moveBet.ContentByLanguage.Add("en-US", reader.ToString("Content_EN"));
                            moveBet.ContentByLanguage.Add("zh-CN", reader.ToString("Content_CS"));
                            moveBet.ContentByLanguage.Add("zh-TW", reader.ToString("Content_CH"));
                            moveBet.ContentByLanguage.Add("th-TH", reader.ToString("Content_TH"));
                            moveBet.ContentByLanguage.Add("ja-JP", reader.ToString("Content_JP"));
                            moveBet.ContentByLanguage.Add("ko-KR", reader.ToString("Content_KO"));
                            moveBet.ContentByLanguage.Add("vi-VN", reader.ToString("Content_VI"));
                        }
                    }

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