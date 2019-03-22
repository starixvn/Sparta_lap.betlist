// -----------------------------------------------------------------------
// <copyright file="IResultDataAccess.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.Athena.Result.DataAccess
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Fanex.Result.Core.Happy5.DTO;
    using Fanex.Result.Core.NumberGame.DTO;
    using Model.MetaModel;

    [System.CLSCompliant(false)]
    public interface IResultNumberGameDataAccess
    {
        Task<IList<NumberGame>> GetNumberGameMatches(ResultMetaModel metalModel);

        Task<IList<Happy5>> GetHappy5Matches(ResultMetaModel metalModel);

        Task<IList<NumberGame>> GetPopUpNumberGameMatch(int matchId);

        Task<IList<Happy5>> GetPopUpHappy5Match(int matchId);
    }
}