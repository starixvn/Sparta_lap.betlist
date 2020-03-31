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
        public string GetResultUrl(string languageCode = "en-US")
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