namespace Fanex.Athena.Service.Report.Result.Interfaces
{
    using System;
    using Fanex.Athena.Model.Report.DTO.Keno;
    using Fanex.Athena.Model.Report.MetaModel.Keno;

    public interface IKenoApi
    {
        KenoAuthorization Authorize(KenoResultMetaModel setting, string userId);

        KenoExtendedToken ExtendToken(KenoResultMetaModel setting, string userId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        string CreateKenoResultUrl(KenoResultMetaModel setting, string userId, DateTime resultDate);
    }
}