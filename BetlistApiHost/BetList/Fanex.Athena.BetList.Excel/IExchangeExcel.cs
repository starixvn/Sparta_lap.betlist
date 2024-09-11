using Fanex.Athena.BetList.Model.DTO;
using Fanex.Athena.BetList.Model.MetaModel;
using System;

namespace Fanex.Athena.BetList.Excel
{
    public interface IExchangeExcel
    {
        byte[] ExportRunningExcel(string template, ExchangeResult<ExchangeRunningTicket> data, ExchangeRunningTicketMetaModel metaModel);
    }
}
