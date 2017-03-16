// --------------------------------------------------------------------------
// <copyright file="CustomerLog.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.DTO.CustomerLog
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