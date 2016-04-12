// --------------------------------------------------------------------------
// <copyright file="BetListController.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.Athena.Api.BetList
{
    #region Using

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Http;
    using Fanex.Athena.Extension.Helper;
    using Fanex.Athena.Models.DataDomain;
    using Fanex.Athena.Models.DTO.BetList;
    using Fanex.Athena.Models.MetaModel.BetList;
    using Fanex.Athena.Service.Implement.BetList;
    using Fanex.Athena.Service.Implement.Configuration;
    using Fanex.Athena.Service.Interface.BetList;
    using Fanex.Athena.Service.Interface.Configuration;
    using Fanex.BetList.Core.Builder;
    using Fanex.BetList.Core.Entities;
    using Fanex.BetList.Core.TicketHelper;

    #endregion Using

    public partial class BetListController : ApiBetListBaseController
    {
    }
}