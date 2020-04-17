namespace Fanex.Athena.Result.Controller
{
    using System;
    using System.Web.Mvc;
    using Fanex.Athena.Result.Service.Interfaces;
    using Fanex.Logging;

    public class ResultIndo4DReportController : BaseResultController
    {
        private readonly ILogger _logger;

        private readonly IIndo4DResult _indo4DResult;

        public ResultIndo4DReportController(ILogger logger, IIndo4DResult indo4DResult)
        {
            _logger = logger;
            _indo4DResult = indo4DResult;
        }

        [HttpPost]
#pragma warning disable S3995 // URI return values should not be strings
        public string GetResultUrl(string languageCode = "en-US")
#pragma warning restore S3995 // URI return values should not be strings
        {
            try
            {
                return _indo4DResult.GetResult(languageCode);
            }
            catch (Exception exception)
            {
                _logger.ErrorAsync(exception.Message, exception);

                return string.Empty;
            }
        }
    }
}