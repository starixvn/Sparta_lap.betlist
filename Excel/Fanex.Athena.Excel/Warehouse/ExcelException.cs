// --------------------------------------------------------------------------
// <copyright file="ExcelException.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>
// ----------------------------------------------------------------------

namespace Fanex.Athena.Excel
{
    using System;

    /// <summary>
    /// Excel exception .
    /// </summary>
    [Serializable] 
    public class ExcelException : Exception
    {
        private readonly string _excelFile;

        public ExcelException(string message, string excelFile)
            : base(message)
        {
            _excelFile = excelFile;
        }

        public override string Message
        {
            get
            {
                return string.Format(
                            "Exception when export excel {0}. Exception Message: {1}",
                            _excelFile,
                            base.Message);
            }
        }

        public override void GetObjectData(System.Runtime.Serialization.SerializationInfo info, System.Runtime.Serialization.StreamingContext context)
        {
            base.GetObjectData(info, context);
        }
    }
}