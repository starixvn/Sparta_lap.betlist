// --------------------------------------------------------------------------
// <copyright file="BaseApiController.cs" company="">
// Copyright (c) . All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.BetList.Controller
{
    using System.Web.Mvc;
    using System.Web.SessionState;
    using Common.Utility.Attributes;
    using Model.DTO;
    using ONEbook.UM.Filters;

    [SessionState(SessionStateBehavior.ReadOnly)]
    [CustomAuth]
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