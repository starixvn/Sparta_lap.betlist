// -----------------------------------------------------------------------
// <copyright file="RNGCasinoUpdateMemberComMetaModel.cs" company="Nexcel Solution">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Models.MetaModel.Casino
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class RNGCasinoUpdateMemberComMetaModel
    {
        public string SiteName { get; set; }

        public string[] CustomerId { get; set; }

        public decimal Comm { get; set; }

        public string FromIp { get; set; }

        public int ModifyBy { get; set; }

        public string ModifyUserName { get; set; }

        public byte FromSystem 
        {
            get
            {
                return 4;
            }
        }
    }
}
