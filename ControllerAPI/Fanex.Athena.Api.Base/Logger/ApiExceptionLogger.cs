namespace Fanex.Athena.Api.Base.Logger
{
    using System.Web.Http.ExceptionHandling;
    using Service.Core.Helpers;

    public class ApiExceptionLogger : ExceptionLogger
    {
        public override void Log(ExceptionLoggerContext context)
        {
            LogHelper.WriteError(context.Exception);
        }
    }
}