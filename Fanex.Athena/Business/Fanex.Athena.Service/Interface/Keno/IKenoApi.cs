// -----------------------------------------------------------------------
// <copyright file="IKenoApi.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Interface.Keno
{
    using System;
    using Fanex.Athena.Models.DTO.Keno;
    using Fanex.Athena.Models.MetaModel.Keno;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IKenoApi
    {
        KenoAuthorization Authorize(KenoResultMetalModel setting, string userId);

        KenoExtendedToken ExtendToken(KenoResultMetalModel setting, string userId);

        string CreateKenoResultUrl(KenoResultMetalModel setting, string userId, DateTime resultDate);
    }
}