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

    /// <summary>
    /// Data access exception.
    /// </summary>
    [Serializable]
    public class DataAccessException : System.Exception
    {
        private readonly string _uspName;
        private readonly object _uspParams;

        public DataAccessException(string message, string uspName, object uspParams)
            : base(message)
        {
            this._uspName = uspName;
            this._uspParams = uspParams;
        }

        public DataAccessException(string message, string uspName)
            : base(message)
        {
            this._uspName = uspName;
            this._uspParams = null;
        }

        public DataAccessException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _uspName = info.GetString(nameof(_uspName));
            _uspParams = info.GetValue(nameof(_uspParams), typeof(object));
        }

        public override string Message
        {
            get
            {
                if (this._uspParams == null)
                {
                    return string.Format("Exception when call usp {0}. \nException: {1}", this._uspName, base.Message);
                }
                else
                {
                    var paramJson = new JavaScriptSerializer().Serialize(_uspParams);
                    return string.Format("Exception when call usp {0}. \nParams: {1}. \nException: {2}", this._uspName, paramJson, base.Message);
                }
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