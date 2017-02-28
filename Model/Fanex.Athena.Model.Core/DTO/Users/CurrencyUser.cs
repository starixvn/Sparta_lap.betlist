// -----------------------------------------------------------------------
// <copyright file="CurrencyUser.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Fanex.Athena.Model.Core.DTO.Users
{
    public class CurrencyUser
    {
        public CurrencyUser()
        {
            CurrencyId = 0;
        }

        public int CurrencyId { get; set; }

        public string CurrencyName { get; set; }

        public bool IsActive { get; set; }

        public int AgentId { get; set; }

        public IEnumerable<int> AgentIdList { get; set; }
    }
}