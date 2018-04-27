namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System;
    using CasinoLibrary.Model.UserException;
    using Fanex.Logging;

    public class ApiMethodOperationHandler<T>
    {
        private readonly ILogger _logger;

        public ApiMethodOperationHandler() : this(null)
        {
        }

        public ApiMethodOperationHandler(ILogger logger)
        {
            _logger = logger ?? Logger.Log;
        }

        public delegate V OutAction<in IInput, out V>(IInput input);

#pragma warning disable S2325 // Methods and properties that don't access instance data should be static
        public T Handle<TInput>(TInput input, OutAction<TInput, T> codeBlock)
#pragma warning restore S2325 // Methods and properties that don't access instance data should be static
        {
            T result = default(T);
            try
            {
                result = codeBlock(input);
            }
            catch (Exception exception)
            {
                if (exception is ErrorException)
                {
                    _logger.Error(exception.Message, exception);
                }
                else
                {
                    throw;
                }
            }

            return result;
        }
    }
}