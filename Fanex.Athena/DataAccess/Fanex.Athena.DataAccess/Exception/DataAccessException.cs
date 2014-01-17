// -----------------------------------------------------------------------
// <copyright file="DataAccessException.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------
// <history>
//  <change who="Bravo" date="2013.09.19">Apply Style Cop</change>
// </history>
// -----------------------------------------------------------------------

namespace Fanex.Athena.DataAccess.Exception
{
    using System;

    /// <summary>
    /// Data access exception.
    /// </summary>
    [Serializable]
    public class DataAccessException : System.Exception
    {
        private readonly string _uspName;
        private readonly dynamic _uspParams;

        public DataAccessException(string message, string uspName, dynamic uspParams)
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
                return this._uspParams == null
                    ? string.Format("Exception when call usp {0}. Exception: {1}", this._uspName, base.Message)
                    : string.Format("Exception when call usp {0}. Params: {1}. Exception: {2}", this._uspName, this._uspParams, base.Message);
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}