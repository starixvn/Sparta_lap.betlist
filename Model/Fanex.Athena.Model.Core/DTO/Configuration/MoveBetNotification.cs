// -----------------------------------------------------------------------
// <copyright file="MoveBet.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Model.Core.DTO.Configuration
{
    using System;
    using System.Collections.Generic;

    [Serializable]
    public class MoveBetNotification
    {
        public MoveBetNotification()
        {
            ContentByLanguage = new Dictionary<string, string>();
        }

        public IDictionary<string, string> ContentByLanguage { get; set; }
    }
}