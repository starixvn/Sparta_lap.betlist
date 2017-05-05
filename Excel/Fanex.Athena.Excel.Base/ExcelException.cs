// --------------------------------------------------------------------------
// <copyright file="ExcelException.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// ----------------------------------------------------------------------

namespace Fanex.Athena.Excel.Base
{
    using System;
    using System.Runtime.Serialization;

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

        public ExcelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            _excelFile = info.GetString(nameof(_excelFile));
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
            info.AddValue(nameof(_excelFile), _excelFile);
            base.GetObjectData(info, context);
        }
    }
}