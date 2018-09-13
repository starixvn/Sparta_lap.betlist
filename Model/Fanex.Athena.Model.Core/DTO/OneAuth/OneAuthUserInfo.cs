using System.Collections.Generic;
using Fanex.Athena.Model.Core.DTO.Users;

namespace Fanex.Athena.Model.Core.DTO.OneAuth
{
    public class OneAuthUserInfo
    {
        public byte UserLevel { get; set; }

        public bool HasSubsidiary { get; set; }

        public string OperatorID { get; set; }

        public IEnumerable<CurrencyUser> CurrencyUserList { get; set; }
    }
}