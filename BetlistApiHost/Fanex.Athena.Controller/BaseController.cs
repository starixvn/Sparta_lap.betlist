// --------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Controller
{
    using System.Web.Mvc;
    using Model.BetList.DTO;
    using ONEbook.UM.Filters;

    [CheckUMFilter]
    public class BaseController : Controller
    {
        protected static void ClearData(TicketResult ticketResult)
        {
            ticketResult.MainTicketList.Clear();
            ticketResult.TicketDataList.Clear();
            ticketResult.MainTicketList.TrimExcess();
            ticketResult.TicketDataList.TrimExcess();
            ticketResult.MainTicketList = null;
            ticketResult.TicketDataList = null;
        }
    }
}