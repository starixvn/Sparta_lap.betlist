// -----------------------------------------------------------------------
// <copyright file="ServiceException.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Core.Exception
{
    using System;
    using System.Runtime.Serialization;

    /// <summary>
    /// Service exception class.
    /// </summary>
    [Serializable]
    public class ServiceException : Exception
    {
        private readonly string _serviceName;

        public ServiceException(string message, string serviceName)
            : base(message)
        {
            _serviceName = serviceName;
        }

        public ServiceException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _serviceName = info.GetString(nameof(_serviceName));
        }

        public override string Message
        {
            get
            {
                return $"Exception when call Service: {_serviceName}. Exception Message: {base.Message}";
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_serviceName), _serviceName);
            base.GetObjectData(info, context);
        }
    }
}