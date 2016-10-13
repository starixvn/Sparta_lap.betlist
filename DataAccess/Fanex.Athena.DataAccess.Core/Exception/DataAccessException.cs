// -----------------------------------------------------------------------
// <copyright file="DataAccessException.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Core.Exception
{
    using System;
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

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}