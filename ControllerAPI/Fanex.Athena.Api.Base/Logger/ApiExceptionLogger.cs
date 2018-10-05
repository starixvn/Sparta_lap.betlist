namespace Fanex.Athena.Api.Base.Logger
{
    using System.Web.Http.ExceptionHandling;
    using Service.Core.Utility.Interfaces;

    public class ApiExceptionLogger : ExceptionLogger
    {
        private readonly ICoreLogger _logger;

        public ApiExceptionLogger(ICoreLogger logger)
        {
            _logger = logger;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            _logger.Error(context.Exception);
        }
    }
}