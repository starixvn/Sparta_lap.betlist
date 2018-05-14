namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System;
    using CasinoLibrary.Model.UserException;
    using Core.Utility;
    using Core.Utility.Interfaces;

    public class ApiMethodOperationHandler<T>
    {
        private readonly ICoreLogger _logger;

        public ApiMethodOperationHandler() : this(null)
        {
        }

        public ApiMethodOperationHandler(ICoreLogger logger)
        {
            _logger = logger ?? new CoreLogger();
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
                    _logger.Error(exception, exception.Message);
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