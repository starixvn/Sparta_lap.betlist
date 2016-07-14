// --------------------------------------------------------------------------
// <copyright file="Request.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Common
{
    /// <summary>
    /// Request class.
    /// </summary>
    public class Request : IRequest
    {
        #region IRequest Members

        public string Ip { get; set; }

        public string Domain { get; set; }

        public string Browser { get; set; }

        public string Platform { get; set; }

        public string CountryCode { get; set; }

        public string Language { get; set; }

        public virtual bool IsValid
        {
            get { return true; }
        }

        #endregion IRequest Members
    }
}