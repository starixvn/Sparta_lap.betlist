// -----------------------------------------------------------------------
// <copyright file="ServiceException.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Exception
{
    using System;

    /// <summary>
    /// Service exception class.
    /// </summary>
    [Serializable]
    public class ServiceException : System.Exception
    {
        private readonly string _serviceName;

        public ServiceException(string message, string serviceName)
            : base(message)
        {
            this._serviceName = serviceName;
        }

        public override string Message
        {
            get
            {
                return string.Format(
                    "Exception when call Service: {0}. Exception Message: {1}",
                    this._serviceName,
                   base.Message);
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}