// --------------------------------------------------------------------------
// <copyright file="CustomerLogMetaModel.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Model.Customer.MetaModel.CustomerLog
{
    using System;
    using Core.MetaModel;

    /// <summary>
    /// CustomerLogMetaModel class.
    /// </summary>
    public class CustomerLogMetaModel : BaseMetaModel
    {
        public CustomerLogMetaModel()
        {
            Year = DateTime.Today.Year;
            Month = DateTime.Today.Month;
            FromDay = 1;
            ToDay = DateTime.Today.Day;
            IsShowOldValue = false;

            // Default is view by all
            ViewById = 0;
        }

        public int ViewById { get; set; }

        public int FromDay { get; set; }

        public int ToDay { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public DateTime FromDate
        {
            get
            {
                return new DateTime(Year, Month, FromDay);
            }
        }

        public DateTime ToDate
        {
            get
            {
                return new DateTime(Year, Month, ToDay);
            }
        }

        public string UserName { get; set; }

        public DateTime LogDate { get; set; }

        public bool IsShowOldValue { get; set; }

        public bool IsAllowBA { get; set; }

        public bool IsAllowCustComm { get; set; }

        public bool IsValid
        {
            get
            {
                return !string.IsNullOrEmpty(UserName);
            }
        }
    }
}