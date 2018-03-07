namespace Fanex.Athena.Service.Report.Result.Interfaces
{
    using System;
    using Model.Report.DTO.Keno;
    using Model.Report.MetaModel.Keno;

    public interface IKenoApi
    {
        KenoExtendedToken ExtendToken(KenoResultMetaModel setting, string userId);

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Sonar Code Smell", "S3995:UriReturnValuesShouldNotBeStrings",
            Justification = "Reviewed. Suppression is OK here.")]
        string CreateKenoResultUrl(KenoResultMetaModel setting, string userId, DateTime resultDate);
    }
}