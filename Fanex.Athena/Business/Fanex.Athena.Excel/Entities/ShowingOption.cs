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
            RbVisible = false;
            NgVisible = false;
            BaVisible = false;
            CustomerComm = false;
            LcVisible = false;
            CsVisible = false;
            BgVisible = false;
            KnVisible = false;
        }

        public bool BaVisible
        {
            get;
            set;
        }

        public bool BgVisible
        {
            get;
            set;
        }

        public bool CsVisible
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

        public bool LcVisible
        {
            get;
            set;
        }

        public bool LicenseeCommVisible
        {
            get
            {
                return BaVisible || RbVisible;
            }
        }

        public bool LicenseeTypeVisible
        {
            get;
            set;
        }

        public bool NgVisible
        {
            get;
            set;
        }

        public bool RbVisible
        {
            get;
            set;
        }

        public bool SbVisible
        {
            get;
            set;
        }

        public bool VsVisible
        {
            get;
            set;
        }

        public bool KnVisible
        {
            get;
            set;
        }
    }
}