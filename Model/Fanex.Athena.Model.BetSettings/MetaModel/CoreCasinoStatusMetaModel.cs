using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fanex.Athena.Model.BetSettings.MetaModel
{
    public class CoreCasinoStatusMetaModel
    {
        public int UserId { get; set; }

        public int CustId { get; set; }

        public int Status { get; set; }

        public string Language { get; set; }

        public string Ip { get; set; }
    }
}
