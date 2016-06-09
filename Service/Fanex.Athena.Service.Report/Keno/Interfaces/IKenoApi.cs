// -----------------------------------------------------------------------
// <copyright file="IKenoApi.cs" company="Nexcel">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Service.Report.Keno.Interfaces
{
    using System;
    using Fanex.Athena.Model.Report.DTO.Keno;
    using Fanex.Athena.Model.Report.MetaModel.Keno;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IKenoApi
    {
        KenoAuthorization Authorize(KenoResultMetalModel setting, string userId);

        KenoExtendedToken ExtendToken(KenoResultMetalModel setting, string userId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        string CreateKenoResultUrl(KenoResultMetalModel setting, string userId, DateTime resultDate);
    }
}