namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System;
    using CasinoLibrary.Model.UserException;

    public class ApiMethodOperationHandler<T>
    {
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
                    Logging.Logger.Log.Error(exception.Message, exception);
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