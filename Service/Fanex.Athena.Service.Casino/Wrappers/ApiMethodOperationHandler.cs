using CasinoLibrary.Model.UserException;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.Service.Casino.Wrappers
{
    public class ApiMethodOperationHandler<T> where T : class
    {
        public delegate V OutAction<in IInput,out V>(IInput input);
        public T Handle<TInput>(TInput input, OutAction<TInput,T> codeBlock)
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
