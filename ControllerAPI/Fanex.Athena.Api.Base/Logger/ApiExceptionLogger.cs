namespace Fanex.Athena.Api.Base.Logger
{
    using System.Web.Http.ExceptionHandling;
    using Service.Core.Utility;

    public class ApiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            var logger = new CoreLogger();
            logger.Error(context.Exception);
        }
    }
}