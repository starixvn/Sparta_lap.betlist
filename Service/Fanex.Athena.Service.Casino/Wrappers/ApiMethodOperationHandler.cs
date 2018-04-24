namespace Fanex.Athena.Service.Casino.Wrappers
{
    using System;
    using CasinoLibrary.Model.UserException;

    public class ApiMethodOperationHandler<T>
    {
        public delegate V OutAction<in IInput, out V>(IInput input);

        public T Handle<TInput>(TInput input, OutAction<TInput, T> codeBlock)
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