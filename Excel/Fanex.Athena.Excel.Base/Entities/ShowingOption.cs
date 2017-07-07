// --------------------------------------------------------------------------
// <copyright file="ShowingOption.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Excel.Base.Entities
{
    public class ShowingOption
    {
        public ShowingOption()
        {
            GrandTotalVisible = true;
            RBVisible = false;
            NGVisible = false;
            BAVisible = false;
            CustomerComm = false;
            LCVisible = false;
            CSVisible = false;
            BGVisible = false;
            KNVisible = false;
        }

        public bool GrandTotalVisible
        {
            get;
            set;
        }

        public bool BAVisible
        {
            get;
            set;
        }

        public bool BGVisible
        {
            get;
            set;
        }

        public bool CSVisible
        {
            get;
            set;
        }

        public bool CustomerComm
        {
            get;
            set;
        }

        public bool CustomerCommVisible
        {
            get;
            set;
        }

        public bool LCVisible
        {
            get;
            set;
        }

        public bool LicenseeCommVisible
        {
            get
            {
                return BAVisible || RBVisible;
            }
        }

        public bool NGVisible
        {
            get;
            set;
        }

        public bool RBVisible
        {
            get;
            set;
        }

        public bool SBVisible
        {
            get;
            set;
        }

        public bool VSVisible
        {
            get;
            set;
        }

        public bool KNVisible
        {
            get;
            set;
        }

        public bool BTVisible
        {
            get;
            set;
        }

        public bool COVisible
        {
            get;
            set;
        }

        public bool BRVSVisible
        {
            get;
            set;
        }
    }
}