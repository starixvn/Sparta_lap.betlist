using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;

namespace Fanex.Athena.BetList.Service.Interfaces
{
    public interface ITicketBuilderService
    {
        IList<TicketText> BuildTicketText(TicketResult ticketResult, ICommonValueMetaModel metaModel,
            bool isAllowedCustomerComm);

    }
}
