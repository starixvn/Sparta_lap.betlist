// --------------------------------------------------------------------------
// <copyright file="CustomerLog.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Shadow.Nguyen" date="2013.1.24">Created</change>
// </history>

namespace Fanex.Athena.Models.DTO.Customer.CustomerLog
{
    using System;

    /// <summary>
    /// CustomerLog class.
    /// </summary>
    public class CustomerLog
    {
        public int ActionId { get; set; }

        public long LogId { get; set; }

        public DateTime LogDate { get; set; }

        public string EditedBy { get; set; }

        public string ActionName { get; set; }

        public string TrackingValue { get; set; }

        public string TrackingValueOld { get; set; }

        public string Details { get; set; }

        public string DetailsOld { get; set; }
    }
}