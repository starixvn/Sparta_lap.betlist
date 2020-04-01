namespace Fanex.Athena.Common.Utility.Services.Internal
{
    using System;
    using System.Runtime.Serialization;

    [Serializable]
    public class ResultException : Exception
    {
        public ResultException(string message)
            : base(message)
        {
        }

        protected ResultException(SerializationInfo serializationInfo, StreamingContext steamContext)
            : base(serializationInfo, steamContext)
        {
        }

        public override string Message
            => $"Result API Exception. Exception: {base.Message}";
    }
}