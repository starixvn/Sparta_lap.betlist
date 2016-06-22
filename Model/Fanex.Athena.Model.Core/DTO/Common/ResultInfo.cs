// --------------------------------------------------------------------------
// <copyright file="ResultInfo.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    using System;

    /// <summary>
    /// ResultInfo class.
    /// </summary>
    [Serializable]
    public class ResultInfo : IResultInfo
    {
        public bool IsSuccessful
        {
            get { return string.IsNullOrEmpty(Message) || (Code == 0); }
        }

        #region IResult Members

        public int Code { get; set; }

        public string Message { get; set; }

        public string ExtendedData { get; set; }

        #endregion IResult Members
    }
}