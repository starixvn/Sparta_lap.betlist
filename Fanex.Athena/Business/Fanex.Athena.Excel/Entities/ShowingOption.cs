// --------------------------------------------------------------------------
// <copyright file="ShowingOption.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Dave.Bui" date="2012.4.11">Created</change>
// </history>

namespace Fanex.Athena.Excel.Entities
{
    /// <summary>
    /// Showing option.
    /// </summary>
    public class ShowingOption
    {
        public ShowingOption()
        {
            LicenseeTypeVisible = false;
            RBVisible = false;
            NGVisible = false;
            BAVisible = false;
            CustomerComm = false;
            LCVisible = false;
            CSVisible = false;
            BGVisible = false;
            KNVisible = false;
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

        public bool LicenseeTypeVisible
        {
            get;
            set;
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
    }
}