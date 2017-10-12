// -----------------------------------------------------------------------
// <copyright file="DataAccessException.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Exception
{
    using System;
    using System.Runtime.Serialization;
    using System.Web.Script.Serialization;

    [Serializable]
    public sealed class DataAccessException : Exception
    {
        private readonly string _uspName;
        private readonly object _uspParams;

        public DataAccessException(string message, string uspName)
            : base(message)
        {
            _uspName = uspName;
        }

        public DataAccessException(string message, string uspName, object uspParams)
            : base(message)
        {
            _uspName = uspName;
            _uspParams = uspParams;
        }

        private DataAccessException(SerializationInfo info, StreamingContext context) 
            : base(info, context)
        {
            _uspName = info.GetString(nameof(_uspName));
            _uspParams = info.GetValue(nameof(_uspParams), typeof(object));
        }

        public string JsonParam
        {
            get
            {
                return _uspParams == null ? "null" : new JavaScriptSerializer().Serialize(_uspParams);
            }
        }

        public override string Message
        {
            get
            {
                return $"Exception when call usp {_uspName}. \nException: {base.Message}";
            }
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue(nameof(_uspName), _uspName);
            info.AddValue(nameof(_uspParams), _uspParams);
            base.GetObjectData(info, context);
        }
    }
}