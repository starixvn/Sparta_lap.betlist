namespace Fanex.Athena.Service.Report.Result.Interfaces
{
    using System;
    using Fanex.Athena.Model.Report.DTO.Keno;
    using Fanex.Athena.Model.Report.MetaModel.Keno;

    public interface IKenoApi
    {
        KenoAuthorization Authorize(KenoResultMetalModel setting, string userId);

        KenoExtendedToken ExtendToken(KenoResultMetalModel setting, string userId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1055:UriReturnValuesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        string CreateKenoResultUrl(KenoResultMetalModel setting, string userId, DateTime resultDate);
    }
}